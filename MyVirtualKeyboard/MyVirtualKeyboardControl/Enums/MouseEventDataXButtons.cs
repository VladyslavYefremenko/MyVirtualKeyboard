using System;

namespace MyVirtualKeyboardControl.Enums
{
    [Flags]
    public enum MouseEventDataXButtons : uint
    {
        Nothing = 0x00000000,
        XBUTTON1 = 0x00000001,
        XBUTTON2 = 0x00000002
    }
}
