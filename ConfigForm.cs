using System;
using System.Drawing;
using System.Windows.Forms;

namespace RestlessMouse
{
  internal class ConfigForm : Form
  {

    readonly Configuration config;

    readonly CheckBox autoRun = new CheckBox { Text = "Autorun", AutoSize = true };
    readonly CheckBox quiet = new CheckBox { Text = "Quiet mode", AutoSize = true };

    readonly Label timerIntervalL = new Label { Text = "Timer Interval", AutoSize = true };
    readonly NumericUpDown timerInterval = new NumericUpDown {
      Minimum = new Decimal(0.5),
      Maximum = 90,
      DecimalPlaces = 1,
      Increment = new Decimal(0.5),
      Size = new Size(45, 20)
    };

    readonly Label deltaL = new Label { Text = "Delta pixels", AutoSize = true };
    readonly NumericUpDown delta = new NumericUpDown {
      Minimum = 1,
      Maximum = 20,
      Size = new Size(45, 20)
    };

    public ConfigForm(Configuration cfg) {

      config = cfg;

      var left = 8;
      var top = 10;

      autoRun.Left = left; autoRun.Top = top;
      autoRun.Checked = config.AutoRun;
      Controls.Add(autoRun);

      timerInterval.Left = left + 100; timerInterval.Top = top - 1;
      var v = Math.Round((decimal)config.TimerInterval / 100) / 10;
      timerInterval.Value = Math.Min(Math.Max(v, timerInterval.Minimum), timerInterval.Maximum);
      timerIntervalL.Left = timerInterval.Left + 50; timerIntervalL.Top = top + 2;
      Controls.Add(timerInterval);
      Controls.Add(timerIntervalL);

      top += 22;
      quiet.Left = left; quiet.Top = top;
      quiet.Checked = config.Quiet;
      Controls.Add(quiet);

      delta.Left = left + 100; delta.Top = top - 1;
      delta.Value = config.Delta;
      deltaL.Left = delta.Left + 50; deltaL.Top = top + 2;
      Controls.Add(delta);
      Controls.Add(deltaL);

      Text = "Restless Mouse";
      Icon = Program.GetTrayIcon(false);
      Size = new Size(270, 90);
      FormBorderStyle = FormBorderStyle.FixedSingle;
      MinimizeBox = false;
      MaximizeBox = false;
      FormClosing += (s, e) => UpdateConfig();

    }

    void UpdateConfig() {
      if (autoRun.Checked != config.AutoRun) config.AutoRun = autoRun.Checked;
      if (quiet.Checked != config.Quiet) config.Quiet = quiet.Checked;
      var ti = (int)(1000 * timerInterval.Value); if (ti != config.TimerInterval) config.TimerInterval = ti;
      if (delta.Value != config.Delta) config.Delta = (int)delta.Value;
      config.Commit(true);
    }

  }
}
