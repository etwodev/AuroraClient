using System.Numerics;
using Aurora.Config;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace Aurora.UI.Windows;

internal class MainWindow : Window
{
  private readonly ConfigurationService configService;

  public MainWindow(ConfigurationService configService) : base($"{Plugin.Name} Main Window [{configService.Version}]{Constants.MainWindowCode}", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.AlwaysAutoResize)
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

    if (ImGui.Button("Show Boilerplate Settings"))
    {
      StateManager.Instance.ToggleWindow(Constants.ConfigWindowCode);
    }
  }
}