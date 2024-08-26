using Aurora.Config;
using Aurora.UI.Windows;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Microsoft.Extensions.Hosting;

namespace Aurora.UI;

internal class StateManager : IDisposable
{
  public static StateManager Instance { get; private set; } = null!;

  private readonly IDalamudPluginInterface _pluginInterface;
  private readonly WindowSystem _windowSystem = new(Plugin.Name);

  public StateManager(IDalamudPluginInterface pluginInterface)
  {
    if (Instance == null) Instance = this;

    _pluginInterface = pluginInterface;

    _pluginInterface.UiBuilder.Draw += _windowSystem.Draw;
    _pluginInterface.UiBuilder.OpenConfigUi += ToggleWindowAction(WindowCode.ConfigWindow);
    _pluginInterface.UiBuilder.OpenMainUi += ToggleWindowAction(WindowCode.MainWindow);
  }

  public void Dispose()
  {
    _pluginInterface.UiBuilder.Draw -= _windowSystem.Draw;
    _pluginInterface.UiBuilder.OpenConfigUi -= ToggleWindowAction(WindowCode.ConfigWindow);
    _pluginInterface.UiBuilder.OpenMainUi -= ToggleWindowAction(WindowCode.MainWindow);

    _windowSystem.RemoveAllWindows();

    Instance = null!;
  }

  private Action ToggleWindowAction(WindowCode code) => () => ToggleWindow(code);
  public void ToggleWindow(WindowCode code) => GetWindow(code)?.Toggle();

  public void ShowWindow(WindowCode code)
  {
    var window = GetWindow(code);
    if (window != null) window.IsOpen = true;
  }

  public Window? GetWindow(WindowCode code)
  {
    foreach (var window in _windowSystem.Windows)
    {
      if (window.WindowName.EndsWith($"###aurora_window_{code}"))
      {
        return window;
      };
    }
    return null;
  }

  public void AddWindow(WindowFactory window) => _windowSystem.AddWindow(window);
  public void RemoveWindow(WindowFactory window) => _windowSystem.RemoveWindow(window);
}
