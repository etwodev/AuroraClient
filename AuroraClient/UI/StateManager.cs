using Aurora.Config;
using Aurora.UI.Windows;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;

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
    _pluginInterface.UiBuilder.OpenConfigUi += ShowWindowAction(WindowCode.ConfigWindow);
    _pluginInterface.UiBuilder.OpenMainUi += ShowWindowAction(WindowCode.MainWindow);
  }

  public void Dispose()
  {
    _pluginInterface.UiBuilder.Draw -= _windowSystem.Draw;
    _pluginInterface.UiBuilder.OpenConfigUi -= ShowWindowAction(WindowCode.ConfigWindow);
    _pluginInterface.UiBuilder.OpenMainUi -= ShowWindowAction(WindowCode.MainWindow);

    _windowSystem.RemoveAllWindows();

    Instance = null!;
  }

  private Action ShowWindowAction(WindowCode id) => () => ShowWindow(id);

  public void ShowWindow(WindowCode id)
  {
    foreach (var window in _windowSystem.Windows)
    {
      if (window.WindowName.EndsWith(id.ToString()))
      {
        window.IsOpen = true;
      }
    }
  }

  public void ToggleWindow(WindowCode id)
  {
    foreach (var window in _windowSystem.Windows)
    {
      if (window.WindowName.EndsWith(id.ToString()))
      {
        window.Toggle();
      }
    }
  }

  public void AddWindow(Window window) => _windowSystem.AddWindow(window);
}
