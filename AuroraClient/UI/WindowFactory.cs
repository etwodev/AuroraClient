


using Aurora.Config;
using Aurora.UI;
using Aurora.UI.Windows;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Aurora.UI;

public enum WindowCode
{
  MainWindow,
  ConfigWindow
}

public abstract class WindowFactory : Window
{
  protected readonly ILogger<WindowFactory> _logger;
  public readonly WindowCode Code;

  protected WindowFactory(ILogger<WindowFactory> logger, string name, WindowCode code) : base($"{name}###aurora_window_{code}", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.AlwaysAutoResize)
  {
    _logger = logger;

    Code = code;

    StateManager.Instance.AddWindow(this);
  }

  public override void Draw() => DrawInternal();

  protected abstract void DrawInternal();
}
