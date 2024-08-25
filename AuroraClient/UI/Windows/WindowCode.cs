namespace Aurora.UI.Windows;

public enum WindowCode
{
  MainWindow,
  ConfigWindow
}

public static class WindowCodeExtensions
{
  public static string ToString(this WindowCode code)
  {
    return code switch
    {
      WindowCode.MainWindow => "###aurora_main_window",
      WindowCode.ConfigWindow => "###aurora_config_window",
      _ => throw new ArgumentOutOfRangeException()
    };
  }

  public static WindowCode FromString(string value)
  {
    return value switch
    {
      "###aurora_main_window" => WindowCode.MainWindow,
      "###aurora_config_window" => WindowCode.ConfigWindow,
      _ => throw new ArgumentException("Invalid string code")
    };
  }
}
