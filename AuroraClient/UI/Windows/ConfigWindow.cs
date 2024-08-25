using System.Numerics;
using Aurora.Config;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace Aurora.UI.Windows;

internal class ConfigWindow : Window
{
  private readonly ConfigurationService configService;

  public ConfigWindow(ConfigurationService configService) : base($"{Plugin.Name} Config Window [{configService.Version}]{Constants.ConfigWindowCode}", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.AlwaysAutoResize)
  {
    this.configService = configService;

    SizeConstraints = new WindowSizeConstraints
    {
      MaximumSize = new Vector2(270, 5000),
      MinimumSize = new Vector2(270, 200)
    };

    StateManager.Instance.AddWindow(this);
  }

  public override void Draw()
  {
    ImGui.Text($"{Plugin.Name} running {configService.Version}");

    bool forceDebug = configService.Configuration.Developer.ForceDebug;
    if (ImGui.Checkbox("Enable Debug Mode", ref forceDebug))
    {
      configService.Configuration.Developer.ForceDebug = forceDebug;
      configService.ApplyChange();
    }
  }
}
