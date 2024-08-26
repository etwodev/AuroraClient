using System.Numerics;
using Aurora.Config;
using ImGuiNET;
using Microsoft.Extensions.Logging;

namespace Aurora.UI.Windows;

internal class MainWindow : WindowFactory
{
  private readonly ConfigurationService _configService;

  public MainWindow(ILogger<MainWindow> logger, ConfigurationService configService) : base(logger, $"{Plugin.Name} Main Window", WindowCode.MainWindow)
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

    if (ImGui.Button("Show Boilerplate Settings"))
    {
      StateManager.Instance.ToggleWindow(WindowCode.ConfigWindow);
    }
  }
}
