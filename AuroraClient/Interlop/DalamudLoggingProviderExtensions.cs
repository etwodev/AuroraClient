using Aurora.Config;
using Dalamud.Plugin.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Aurora.Interop;

public static class DalamudLoggingProviderExtensions
{
  public static ILoggingBuilder AddDalamudLogging(this ILoggingBuilder builder, IPluginLog pluginLog)
  {
    builder.ClearProviders();

    builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, DalamudLoggingProvider>
        (b => new DalamudLoggingProvider(b.GetRequiredService<ConfigurationService>(), pluginLog)));
    return builder;
  }
}