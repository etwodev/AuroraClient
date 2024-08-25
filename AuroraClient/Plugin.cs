using Aurora.Config;
using Aurora.Core;
using Aurora.UI;
using Aurora.UI.Windows;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Aurora;

/// <summary>
/// <c>Plugin</c> acts as an entrypoint into Aurora. We build the services, then
/// start the Async Tasks that need to be done on plugin initilisation.
/// </summary>
public class Plugin : IDalamudPlugin
{
  public const string Name = "Aurora";
  public static IPluginLog Log { get; private set; } = null!;
  public static IFramework Framework { get; private set; } = null!;

  private static ServiceProvider? _services = null;


  public Plugin(IDalamudPluginInterface pluginInterface)
  {
    var dalamudServices = new DalamudServices(pluginInterface);
    Log = dalamudServices.Log;
    Framework = dalamudServices.Framework;

    Log.Info($"Starting {Name}");

    LoadServices(dalamudServices);

    Log.Info($"Started {Name}");
  }

  private void LoadServices(DalamudServices dalamudServices)
  {
    try
    {
      var serviceCollection = SetupServices(dalamudServices);
      _services = serviceCollection.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true });

      InitializeSingletonServices(serviceCollection);
    }
    catch (Exception e)
    {
      Log.Error(e, $"Failed to start {Name}");
      _services?.Dispose();
      throw;
    }
  }

  // Separate method to initialize singleton services
  private void InitializeSingletonServices(IEnumerable<ServiceDescriptor> serviceCollection)
  {
    if (_services == null)
    {
      _services?.Dispose();
      throw new Exception("Services have not been initilised!");
    }

    foreach (var service in serviceCollection)
    {
      if (service.Lifetime == ServiceLifetime.Singleton)
      {
        Log.Debug($"Initializing {service.ServiceType}...");
        _services.GetRequiredService(service.ServiceType);
      }
    }
  }

  private static ServiceCollection SetupServices(DalamudServices dalamudServices)
  {
    ServiceCollection serviceCollection = new();

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
    serviceCollection.AddSingleton<ConfigurationService>();

    // UI
    serviceCollection.AddSingleton<StateManager>();
    serviceCollection.AddSingleton<MainWindow>();
    serviceCollection.AddSingleton<ConfigWindow>();

    return serviceCollection;
  }

  public void Dispose() => _services?.Dispose();
}
