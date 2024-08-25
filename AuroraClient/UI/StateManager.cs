using Aurora.Config;
using Aurora.UI.Windows;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;

namespace Aurora.UI;


internal class StateManager : IDisposable
{
  public static StateManager Instance { get; private set; } = null!;
  private readonly IDalamudPluginInterface pluginInterface;

  private readonly WindowSystem windowSystem = new(Plugin.Name);

  public StateManager(IDalamudPluginInterface pluginInterface)
  {
    if (Instance == null) Instance = this;

    this.pluginInterface = pluginInterface;

    this.pluginInterface.UiBuilder.Draw += windowSystem.Draw;
    this.pluginInterface.UiBuilder.OpenConfigUi += ShowWindow(WindowCode.ConfigWindow);
    this.pluginInterface.UiBuilder.OpenMainUi += ShowWindow(WindowCode.MainWindow);
  }

  public void Dispose()
  {
    pluginInterface.UiBuilder.Draw -= windowSystem.Draw;
    pluginInterface.UiBuilder.OpenConfigUi -= ShowWindow(WindowCode.ConfigWindow);
    pluginInterface.UiBuilder.OpenMainUi -= ShowWindow(WindowCode.MainWindow);

    windowSystem.RemoveAllWindows();

    Instance = null!;
  }

  private Action ShowWindow(WindowCode id)
  {
    return () =>
    {
      foreach (var window in windowSystem.Windows)
      {
        if (window.WindowName.EndsWith(id.ToString()))
        {
          window.IsOpen = true;
        }
      }
    };
  }

  public void ToggleWindow(WindowCode id)
  {
    foreach (var window in windowSystem.Windows)
    {
      if (window.WindowName.EndsWith(id.ToString()))
      {
        window.Toggle();
      }
    }
  }

  public void AddWindow(Window window) => windowSystem.AddWindow(window);
}
