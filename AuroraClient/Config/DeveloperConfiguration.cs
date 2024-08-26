using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Aurora.Config;

internal class DeveloperConfiguration
{
  public bool ForceDebug { get; set; } = false;
  public LogLevel LogLevel { get; set; } = LogLevel.Debug;
}
