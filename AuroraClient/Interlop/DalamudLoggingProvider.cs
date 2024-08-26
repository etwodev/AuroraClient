using Aurora.Config;
using Dalamud.Plugin.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Aurora.Interop;

[ProviderAlias("Dalamud")]
internal sealed class DalamudLoggingProvider : ILoggerProvider
{
  private readonly ConcurrentDictionary<string, DalamudLogger> _loggers =
      new(StringComparer.OrdinalIgnoreCase);

  private readonly ConfigurationService _configService;
  private readonly IPluginLog _pluginLog;

  public DalamudLoggingProvider(ConfigurationService configService, IPluginLog pluginLog)
  {
    _configService = configService;
    _pluginLog = pluginLog;
  }

  public ILogger CreateLogger(string categoryName)
  {
    string FormatCategoryName(string catName)
    {
      if (catName.Length > 15)
      {
        return catName.Substring(0, 6) + "..." + catName[^6..];
      }
      return catName.PadLeft(15);
    }

    string formattedCategoryName = FormatCategoryName(categoryName.Split(".", StringSplitOptions.RemoveEmptyEntries).Last());

    return _loggers.GetOrAdd(formattedCategoryName, name => new DalamudLogger(name, _configService, _pluginLog));
  }

  public void Dispose()
  {
    _loggers.Clear();
    GC.SuppressFinalize(this);
  }
}
