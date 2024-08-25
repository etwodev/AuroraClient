using Aurora.Config;
using Aurora.UI.Windows;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;

namespace Aurora.UI;

public interface IIdentifiable
{
  public string WindowId { get; }
}

internal class StateManager : IDisposable
{
  public static StateManager Instance { get; private set; } = null!;
  private readonly IDalamudPluginInterface pluginInterface;
  private readonly ConfigurationService configurationService;

  private readonly WindowSystem windowSystem = new(Plugin.Name);

  public StateManager(IDalamudPluginInterface pluginInterface, ConfigurationService configurationService)
  {
    if (Instance == null) Instance = this;

    this.pluginInterface = pluginInterface;
    this.configurationService = configurationService;

    this.pluginInterface.UiBuilder.Draw += windowSystem.Draw;
    this.pluginInterface.UiBuilder.OpenConfigUi += () => ShowWindow(Constants.ConfigWindowCode);
    this.pluginInterface.UiBuilder.OpenMainUi += () => ShowWindow(Constants.MainWindowCode);
  }

  public void Dispose()
  {
    pluginInterface.UiBuilder.Draw -= windowSystem.Draw;
    pluginInterface.UiBuilder.OpenConfigUi -= () => ToggleWindow(Constants.ConfigWindowCode);
    pluginInterface.UiBuilder.OpenMainUi -= () => ToggleWindow(Constants.MainWindowCode);

    windowSystem.RemoveAllWindows();

    Instance = null!;
  }

  public void ShowWindow(string id)
  {
    foreach (var window in windowSystem.Windows)
    {
      if (window.WindowName.EndsWith(id))
      {
        window.IsOpen = true;
      }
    }
  }

  public void ToggleWindow(string id)
  {
    foreach (var window in windowSystem.Windows)
    {
      if (window.WindowName.EndsWith(id))
      {
        window.Toggle();
      }
    }
  }

  public void AddWindow(Window window) => windowSystem.AddWindow(window);
}
