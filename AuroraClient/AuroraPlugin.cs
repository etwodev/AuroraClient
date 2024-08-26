

using Aurora.UI;
using Aurora.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Aurora.UI.Windows;

namespace Aurora;

public class AuroraPlugin : IHostedService
{
  private readonly IServiceScopeFactory _serviceScopeFactory;
  private readonly ILogger<AuroraPlugin> _logger;

  public AuroraPlugin(ILogger<AuroraPlugin> logger, IServiceScopeFactory serviceScopeFactory)
  {
    _serviceScopeFactory = serviceScopeFactory;
    _logger = logger;
  }

  public Task StartAsync(CancellationToken cancellationToken)
  {
    try
    {
      _logger.LogDebug("Initializing services...");

      using (var scope = _serviceScopeFactory.CreateScope())
      {
        scope.ServiceProvider.GetRequiredService<StateManager>();
        scope.ServiceProvider.GetRequiredService<ConfigurationService>();
        scope.ServiceProvider.GetServices<WindowFactory>();
      }

      _logger.LogDebug("Services initialized.");

      return Task.CompletedTask;
    }
    catch (Exception e)
    {
      _logger.LogCritical(e, "Failed to initilise services!");
      return Task.FromException(e);
    }
  }

  public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
