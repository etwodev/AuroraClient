using Dalamud.Plugin;

namespace Aurora.Config;

internal class ConfigurationService : IDisposable
{
  public static ConfigurationService Instance { get; private set; } = null!;
  public Configuration Configuration { get; private set; } = null!;

  /// <summary>
  ///    This event is triggered whenever <c>ApplyChange()</c> is called on a
  ///    configuration.
  /// </summary>
  public event OnChangeDelegate? OnChange;
  public delegate void OnChangeDelegate();

  private readonly IDalamudPluginInterface pluginInterface;

  public ConfigurationService(IDalamudPluginInterface pluginInterface)
  {
    if (Instance == null) Instance = this;

    this.pluginInterface = pluginInterface;
    Configuration = this.pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
  }

  /// <summary>
  /// This method saves the service configuration to storage.
  /// <para>NOTE: This method should not be called outside of the service.</para>
  /// </summary>
  private void Save() => this.pluginInterface.SavePluginConfig(Configuration);

  public void Dispose() => Save();

  /// <summary>
  /// This method applies any changes made, and then calls the <c>OnChange()</c> handler.
  /// </summary>
  /// <param name="save">Whether or not to save the plugin configuration. Defaults to <c>true</c>.</param>
  public void ApplyChange(bool save = true)
  {
    if (save)
      Save();

    OnChange?.Invoke();
  }

  /// <summary>
  /// This method resets ALL configuration values.
  /// </summary>
  /// <param name="hard">Whether or not to save the configuration after reset. Defaults to <c>true</c>.</param>
  public void Reset(bool hard = true)
  {
    Configuration = new Configuration();

    ApplyChange(hard);
  }

#if DEBUG
  private static bool isDebug => true;
#else
  private static bool isDebug => false;
#endif

  private static readonly string version = typeof(Plugin).Assembly.GetName().Version?.ToString() ?? "(Unknown Version)";
  public bool IsDebug => isDebug || Configuration.Developer.ForceDebug;
  public string Version => IsDebug ? "(Debug)" : $"v{version}";
}
