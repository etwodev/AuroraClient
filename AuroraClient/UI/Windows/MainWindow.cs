using System.Numerics;
using Aurora.Config;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace Aurora.UI.Windows;

internal class MainWindow : Window
{
  private readonly ConfigurationService _configService;

  public MainWindow(ConfigurationService configService) : base($"{Plugin.Name} Main Window [{configService.Version}]###{WindowCode.MainWindow}", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.AlwaysAutoResize)
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

    if (ImGui.Button("Show Boilerplate Settings"))
    {
      StateManager.Instance.ToggleWindow(WindowCode.ConfigWindow);
    }
  }
}
