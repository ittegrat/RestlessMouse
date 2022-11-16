using System.Drawing;
using System.Windows.Forms;

namespace RestlessMouse
{
  internal class HelpForm : Form
  {

    const string helpText = @"
Usage: restlessmouse [option]...
Command line options override saved preferences.

Options (add 'x' to disable):
  -d N: number of pixels to move
  -q[x]: quiet (zero movement)
  -r[x]: start running
  -t SEC: timer interval
  -h, --help: print this help
";

    readonly Label header = new Label { Text = "Command line parameters", AutoSize = true };
    readonly Label help = new Label { Text = helpText, AutoSize = true };

    public HelpForm() {

      var font = new Font("Courier New", 9);

      var left = 10;
      var top = 6;

      header.Left = left; header.Top = top;
      header.Font = new Font(font, FontStyle.Bold);
      Controls.Add(header);

      top += 10;
      help.Left = left; help.Top = top;
      help.Font = font;
      Controls.Add(help);

      Text = "Restless Mouse";
      Icon = Program.GetTrayIcon(false);
      Size = new Size(400, 208);
      FormBorderStyle = FormBorderStyle.FixedSingle;
      MinimizeBox = false;
      MaximizeBox = false;

    }

  }
}
