using Aurora.Config;
using Aurora.Core;
using Aurora.UI;
using Aurora.UI.Windows;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Aurora
{
  public class Plugin : IDalamudPlugin
  {
    public const string Name = "Aurora";

    private static ServiceProvider? services = null;

    public static IPluginLog Log { get; private set; } = null!;
    public static IFramework Framework { get; private set; } = null!;


    public Plugin(IDalamudPluginInterface pluginInterface)
    {
      var dalamudServices = new DalamudServices(pluginInterface);
      Log = dalamudServices.Log;
      Framework = dalamudServices.Framework;


      dalamudServices.Framework.RunOnTick(() =>
      {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        Log.Info($"Starting {Name}...");

        try
        {
          // Setup plugin services
          var serviceCollection = SetupServices(dalamudServices);
          services = serviceCollection.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true });

          // Initialize the singletons
          foreach (var service in serviceCollection)
          {
            if (service.Lifetime == ServiceLifetime.Singleton)
            {
              Log.Debug($"Initializing {service.ServiceType}...");
              services.GetRequiredService(service.ServiceType);
            }
          }

          Log.Info($"Started {Name} in {stopwatch.ElapsedMilliseconds}ms");
        }
        catch (Exception e)
        {
          Log.Error(e, $"Failed to start {Name} in {stopwatch.ElapsedMilliseconds}ms");
          services?.Dispose();
          throw;
        }
      }, delayTicks: 2);
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

    public void Dispose() => services?.Dispose();
  }
}
