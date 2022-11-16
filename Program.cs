using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace RestlessMouse
{
  internal static class Program
  {

    static readonly Configuration config = new Configuration();
    static readonly NotifyIcon ni = new NotifyIcon();
    static readonly Timer timer = new Timer();

    static bool flip = false;
    static bool help = false;

    [STAThread]
    static void Main(string[] args) {

      // Only a single instace is allowed to run
      var singleInstance = new System.Threading.Mutex(true, "RestlessMouseSingleInstance", out bool granted);
      if (!granted) {
        Debug.WriteLine("Restless Mouse is already running. Exiting.");
        return;
      }

      // Application setup
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      Application.ApplicationExit += (s, e) => {
        ni.Visible = false;
        ni.Dispose();
      };

      // Command line parsing
      if (args.Length > 0) {
        for (var i = 0; i < args.Length; ++i) {
          switch (args[i].Substring(0, 2)) {
            case "--":
              switch (args[i].Substring(2)) {
                case "help":
                  help = true;
                  break;
                default:
                  throw new ArgumentException($"Invalid option: {args[i]}");
              }
              break;
            case "-d":
              ++i; config.Delta = Int32.Parse(args[i]);
              break;
            case "-h":
              help = true;
              break;
            case "-q":
              config.Quiet = args[i].Length < 3;
              break;
            case "-r":
              config.AutoRun = args[i].Length < 3;
              break;
            case "-t":
              ++i; config.TimerInterval = 1000 * Int32.Parse(args[i]);
              break;
            default:
              throw new ArgumentException($"Invalid option: {args[i]}");
          }
        }
        config.Commit(false);
      }

      // Show help and exit
      if (help) {
        new HelpForm().ShowDialog();
        return;
      }

      // Timer setup
      timer.Interval = config.TimerInterval;
      timer.Tick += (s, e) => ZigZag();

      // ContextMenu setup
      var conf = new MenuItem { Name = "conf", Text = "Configure" };
      var exit = new MenuItem { Name = "exit", Text = "Exit" };
      var hlp = new MenuItem { Name = "help", Text = "Help" };
      var run = new MenuItem { Name = "run", Text = "Run" };
      var stop = new MenuItem { Name = "stop", Text = "Stop" };
      ni.ContextMenu = new ContextMenu(new MenuItem[] { conf, run, stop, hlp, exit, });
      conf.Click += Configure;
      exit.Click += (s, e) => Application.Exit();
      hlp.Click += (s, e) => new HelpForm().ShowDialog();
      run.Click += (s, e) => Start();
      stop.Click += (s, e) => Stop();

      ni.Visible = true;
      ni.DoubleClick += (s, e) => { if (timer.Enabled) Stop(); else Start(); };

      // AutoRun
      if (config.AutoRun)
        Start();
      else
        Stop();

      // Start message loop
      Application.Run();

    }
    static void Configure(object sender, EventArgs e) {
      var te = timer.Enabled;
      if (te) Stop();
      var cf = new ConfigForm(config);
      cf.FormClosed += (s, fe) => {
        timer.Interval = config.TimerInterval;
        if (te) Start();
      };
      cf.ShowDialog();
    }
    internal static Icon GetTrayIcon(bool run) {
      var icoName = $"RestlessMouse.QMouse{(run ? "Run" : String.Empty)}.ico";
      var asm = Assembly.GetExecutingAssembly();
      using (var mrs = asm.GetManifestResourceStream(icoName)) {
        return new Icon(mrs);
      }
    }
    static void Start() {
      ni.Text = $"Restless Mouse\nRunning ({Math.Round((double)timer.Interval / 100) / 10}s)";
      ni.Icon = GetTrayIcon(true);
      EnableRunMenu(false);
      timer.Start();
    }
    static void Stop() {
      timer.Stop();
      ni.Icon = GetTrayIcon(false);
      ni.Text = "Restless Mouse\nStopped";
      EnableRunMenu(true);
    }
    static void EnableRunMenu(bool run) {
      var mi = ni.ContextMenu.MenuItems;
      mi["run"].Enabled = run;
      mi["stop"].Enabled = !run;
    }

    static void MoveMouse(int delta) {
      var input = new NativeMethods.Input {
        type = NativeMethods.InputType.Mouse,
        mi = new NativeMethods.MouseInput {
          dx = delta,
          dy = delta,
          mouseData = 0,
          dwFlags = NativeMethods.MouseEventFlag.Move,
          time = 0,
          dwExtraInfo = UIntPtr.Zero
        }
      };
      var ans = NativeMethods.SendInput(1, new[] { input }, NativeMethods.Input.Size);
      if (ans != 1)
        throw new Win32Exception();
    }
    static void ZigZag() {
      if (config.Quiet)
        MoveMouse(0);
      else if (flip)
        MoveMouse(config.Delta);
      else
        MoveMouse(-config.Delta);
      flip = !flip;
    }

  }
}
