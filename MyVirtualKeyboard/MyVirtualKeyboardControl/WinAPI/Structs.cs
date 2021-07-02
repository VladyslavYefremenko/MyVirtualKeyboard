using MyVirtualKeyboardControl.Enums;
using System;
using System.Runtime.InteropServices;

namespace MyVirtualKeyboardControl.WinAPI
{
    [StructLayout(LayoutKind.Sequential)]
    public struct INPUT
    {
        internal uint type;
        internal InputUnion inputUnion;
        internal static int Size { get => Marshal.SizeOf(typeof(INPUT)); }
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct InputUnion
    {
        [FieldOffset(0)]
        internal MOUSEINPUT mi;
        [FieldOffset(0)]
        internal KEYBDINPUT ki;
        [FieldOffset(0)]
        internal HARDWAREINPUT hi;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MOUSEINPUT
    {
        internal int dx;
        internal int dy;
        internal MouseEventDataXButtons mouseData;
        internal MOUSEEVENTF dwFlags;
        internal uint time;
        internal UIntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct KEYBDINPUT
    {
        internal VirtualKeyCode wVK;
        internal ushort wScan;
        internal KEYEVENTF dwFlags;
        internal uint time;
        internal UIntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HARDWAREINPUT
    {
        internal int uMsg;
        internal short wParamL;
        internal short wParamH;
    }
}
