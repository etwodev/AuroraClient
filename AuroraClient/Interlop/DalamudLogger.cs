using Aurora.Config;
using Dalamud.Plugin.Services;

using Microsoft.Extensions.Logging;
using System.Text;

namespace Aurora.Interop;

#pragma warning disable CS8633
internal sealed class DalamudLogger : ILogger
{
  private readonly ConfigurationService _configService;
  private readonly string _name;
  private readonly IPluginLog _pluginLog;

  public DalamudLogger(string name, ConfigurationService configService, IPluginLog pluginLog)
  {
    _name = name;
    _configService = configService;
    _pluginLog = pluginLog;
  }

  public IDisposable BeginScope<TState>(TState state)
  {
    return default!;
  }

  public bool IsEnabled(LogLevel logLevel)
  {
    return (int)_configService.LogLevel <= (int)logLevel;
  }

  public string ParseException(Exception? exception)
  {
    if (exception == null) return string.Empty;

    StringBuilder sb = new();

    sb.AppendLine($"| Exception: '{exception.Message}'");
    sb.AppendLine(exception.StackTrace);
    var innerException = exception.InnerException;

    while (innerException != null)
    {
      sb.AppendLine($"InnerException {innerException}: {innerException.Message}");
      sb.AppendLine(innerException.StackTrace);
      innerException = innerException.InnerException;
    }

    return sb.ToString();
  }

  public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
  {
    if (!IsEnabled(logLevel)) return;

    if (formatter == null) throw new ArgumentNullException(nameof(formatter));

    var logLevelCast = (int)logLevel;
    var msg = formatter(state, exception);
    var formatString = $"[{_name}] [{logLevelCast}] {msg} {ParseException(exception)}";

    switch (logLevel)
    {
      case LogLevel.Critical:
        _pluginLog.Fatal(formatString);
        break;
      case LogLevel.Error:
        _pluginLog.Error(formatString);
        break;
      case LogLevel.Warning:
        _pluginLog.Warning(formatString);
        break;
      case LogLevel.Information:
        _pluginLog.Information(formatString);
        break;
      case LogLevel.Debug:
        _pluginLog.Debug(formatString);
        break;
      case LogLevel.Trace:
        _pluginLog.Verbose(formatString);
        break;
      default:
        _pluginLog.Information(formatString);
        break;
    }
  }
}
