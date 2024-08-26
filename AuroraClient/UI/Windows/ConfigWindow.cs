using System.Numerics;
using Aurora.Config;
using ImGuiNET;
using Microsoft.Extensions.Logging;

namespace Aurora.UI.Windows;

internal class ConfigWindow : WindowFactory
{
  private readonly ConfigurationService _configService;

  public ConfigWindow(ILogger<ConfigWindow> logger, ConfigurationService configService) : base(logger, $"{Plugin.Name} Config Window", WindowCode.ConfigWindow)
  {
    _configService = configService;

    SizeConstraints = new WindowSizeConstraints
    {
      MaximumSize = new Vector2(500, 5000),
      MinimumSize = new Vector2(500, 200)
    };
  }

  protected override void DrawInternal()
  {
    ImGui.Text($"{Plugin.Name} running {_configService.Version}");

    bool forceDebug = _configService.Configuration.Developer.ForceDebug;
    if (ImGui.Checkbox("Enable Debug Mode", ref forceDebug))
    {
      _configService.Configuration.Developer.ForceDebug = forceDebug;
      _configService.ApplyChange();
    }
  }
}
