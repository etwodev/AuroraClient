using Dalamud.Configuration;

namespace Aurora.Config;

internal class Configuration : IPluginConfiguration
{
  public const int CurrentVersion = 0;
  public const ConfigStates CurrentConfigurationState = ConfigStates.FirstLaunch;

  public int Version { get; set; } = CurrentVersion;
  public ConfigStates ConfigurationState { get; set; } = CurrentConfigurationState;

  // Developer
  public DeveloperConfiguration Developer { get; set; } = new DeveloperConfiguration();
}

public enum ConfigStates
{
  FirstLaunch = -1,
  ClientUpdate,
}
