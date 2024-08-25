using System.Numerics;
using Aurora.Config;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace Aurora.UI.Windows;

internal class ConfigWindow : Window
{
  private readonly ConfigurationService _configService;

  public ConfigWindow(ConfigurationService configService) : base($"{Plugin.Name} Config Window [{configService.Version}]###{WindowCode.ConfigWindow}", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.AlwaysAutoResize)
  {
    _configService = configService;

    SizeConstraints = new WindowSizeConstraints
    {
      MaximumSize = new Vector2(500, 5000),
      MinimumSize = new Vector2(500, 200)
    };

    StateManager.Instance.AddWindow(this);
  }

  public override void Draw()
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
