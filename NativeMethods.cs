using System;
using System.Runtime.InteropServices;

namespace RestlessMouse
{
  internal static class NativeMethods
  {

    [Flags]
    public enum MouseEventFlag : UInt32
    {
      Move = 0x0001,
      LeftDown = 0x0002,
      LeftUp = 0x0004,
      RightDown = 0x0008,
      RightUp = 0x0010,
      MiddleDown = 0x0020,
      MiddleUp = 0x0040,
      XDown = 0x0080,
      XUp = 0x0100,
      Wheel = 0x0800,
      HWheel = 0x1000,
      MoveNoCoalesce =0x2000,
      VirtualDesk = 0x4000,
      Absolute = 0x8000,
    }

    [Flags]
    public enum InputType : UInt32
    {
      Mouse = 0,
      Keyboard = 1,
      Hardware = 2,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MouseInput
    {
      public Int32 dx;
      public Int32 dy;
      public UInt32 mouseData;
      public MouseEventFlag dwFlags;
      public UInt32 time;
      public UIntPtr dwExtraInfo;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct Input
    {
      public InputType type;
      public MouseInput mi;
      internal static int Size => Marshal.SizeOf(typeof(Input));
    }

    [DllImport("User32.dll", SetLastError = true)]
    public static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);

  }
}
