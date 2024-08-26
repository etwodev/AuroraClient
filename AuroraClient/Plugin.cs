using Aurora.Config;
using Aurora.Core;
using Aurora.Interop;
using Aurora.UI;
using Aurora.UI.Windows;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Aurora;

/// <summary>
/// <c>Plugin</c> acts as an entrypoint into Aurora. We build the services, then
/// start the Async Tasks that need to be done on plugin initilisation.
/// </summary>
public class Plugin : IDalamudPlugin
{
  public const string Name = "Aurora";
  private readonly IHost _host;

  public Plugin(IDalamudPluginInterface pluginInterface, IPluginLog pluginLog)
  {
    var dalamudServices = new DalamudServices(pluginInterface);

    _host = Host.CreateDefaultBuilder()
    .ConfigureLogging(SetupLogging(pluginLog))
    .UseContentRoot(pluginInterface.ConfigDirectory.FullName)
    .ConfigureServices(SetupServices(dalamudServices))
    .Build();

    _host.StartAsync().GetAwaiter().GetResult();
  }

  private static Action<ILoggingBuilder> SetupLogging(IPluginLog pluginLog)
  {
    return (ILoggingBuilder lb) =>
    {
      lb.ClearProviders();
      lb.AddDalamudLogging(pluginLog);
      lb.SetMinimumLevel(LogLevel.Trace);
    };
  }

  private static Action<IServiceCollection> SetupServices(DalamudServices dalamudServices)
  {
    return (IServiceCollection serviceCollection) =>
    {
      // Dalamud
      serviceCollection.AddSingleton(dalamudServices.PluginInterface);
      serviceCollection.AddSingleton(dalamudServices.Framework);
      serviceCollection.AddSingleton(dalamudServices.GameInteropProvider);
      serviceCollection.AddSingleton(dalamudServices.ClientState);
      serviceCollection.AddSingleton(dalamudServices.SigScanner);
      serviceCollection.AddSingleton(dalamudServices.ObjectTable);
      serviceCollection.AddSingleton(dalamudServices.DataManager);
      serviceCollection.AddSingleton(dalamudServices.CommandManager);
      serviceCollection.AddSingleton(dalamudServices.ToastGui);
      serviceCollection.AddSingleton(dalamudServices.TargetManager);
      serviceCollection.AddSingleton(dalamudServices.TextureProvider);
      serviceCollection.AddSingleton(dalamudServices.Log);
      serviceCollection.AddSingleton(dalamudServices.ChatGui);
      serviceCollection.AddSingleton(dalamudServices.KeyState);

      // Core
      serviceCollection.AddSingleton<AuroraPlugin>();
      serviceCollection.AddSingleton<ConfigurationService>();

      // UI
      serviceCollection.AddSingleton<StateManager>();
      serviceCollection.AddSingleton<WindowFactory, MainWindow>();
      serviceCollection.AddSingleton<WindowFactory, ConfigWindow>();

      // Host Services
      serviceCollection.AddHostedService(p => p.GetRequiredService<AuroraPlugin>());
    };
  }

  public void Dispose()
  {
    _host.StopAsync().GetAwaiter().GetResult();
    _host.Dispose();
  }
}
