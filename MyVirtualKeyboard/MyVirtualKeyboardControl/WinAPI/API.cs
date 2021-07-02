using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MyVirtualKeyboardControl.WinAPI
{
    public static class API
    {
        [DllImport("user32.dll")]
        internal static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        internal static extern int ToUnicode(uint virtualKeyCode, uint scanCode, byte[] keyboardState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)] StringBuilder receivingBuffer,
            int bufferSize, uint flags);

        [DllImport("user32.dll")]
        internal static extern uint GetKeyboardLayoutList(int nBuff, [Out] IntPtr[] lpList);

        [DllImport("user32.dll")]
        static internal extern UInt32 ActivateKeyboardLayout(IntPtr hkl, UInt32 flags);
    }
}
