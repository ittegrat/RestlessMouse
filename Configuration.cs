using System;
using System.Collections.Generic;
using System.Configuration;

namespace RestlessMouse
{
  internal class Configuration
  {

    readonly Dictionary<string, string> changes = new Dictionary<string, string>();

    bool autoRun;
    public bool AutoRun {
      get { return autoRun; }
      set { autoRun = SetSetting(nameof(AutoRun), value); }
    }

    int delta;
    public int Delta {
      get { return delta; }
      set { delta = SetSetting(nameof(Delta), value); }
    }

    bool quiet;
    public bool Quiet {
      get { return quiet; }
      set { quiet = SetSetting(nameof(Quiet), value); }
    }

    int timerInterval;
    public int TimerInterval {
      get { return timerInterval; }
      set { timerInterval = SetSetting(nameof(TimerInterval), value); }
    }

    public Configuration() {
      autoRun = GetSetting(nameof(AutoRun), false);
      delta = GetSetting(nameof(Delta), 4);
      quiet = GetSetting(nameof(Quiet), false);
      timerInterval = GetSetting(nameof(TimerInterval), 30000);
    }
    public void Commit(bool permanent) {
      if (permanent && changes.Count > 0) {
        var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        foreach (var c in changes) {
          var s = config.AppSettings.Settings[c.Key];
          if (s != null)
            s.Value = c.Value;
          else
            config.AppSettings.Settings.Add(c.Key, c.Value);
        }
        config.Save(ConfigurationSaveMode.Modified);
        ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
      }
      changes.Clear();
    }

    T GetSetting<T>(string key, T @default) {
      try {
        var value = ConfigurationManager.AppSettings[key];
        if (value != null)
          return (T)Convert.ChangeType(value, typeof(T));
      }
      catch { }
      return @default;
    }
    T SetSetting<T>(string key, T value) {
      //if (SaveOnSet) {
      //  var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
      //  var kv = config.AppSettings.Settings[key];
      //  if (kv != null)
      //    kv.Value = value.ToString();
      //  else
      //    config.AppSettings.Settings.Add(key, value.ToString());
      //  config.Save(ConfigurationSaveMode.Modified);
      //  ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
      //}
      changes[key] = value.ToString();
      return value;
    }

  }
}
