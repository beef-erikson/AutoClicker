using System;
using System.Runtime.InteropServices;
using System.Windows;
//using System.Windows.Forms;
using System.Windows.Input;

namespace AutoClicker
{
    public class GlobalHotkey : Window
    {
        // Fields 
        private int modifier;
	    private int key;
	    private IntPtr handler;
	    private int id;
        
        // Register hotkey
        [DllImport("user32.dll")]
        static extern bool RegisterHotKey(IntPtr windowsHandler, int hotkey_id, int Modifiers, int virtualkey);

        // Unregister hotkey
        [DllImport("user32.dll")]
        static extern bool UnregisterHotKey(IntPtr windowsHandler, int hotkey_id);


    /*
	    public GlobalHotkey(int modifier, Keys key, Form form)
	    {
	        this.modifier = modifier;
	        this.key = (int) key;
            this.handler = form.Handle;
	        id = this.GetHashCode();
	    }
    */

    /// <summary>
    /// Constants for storing keys
    /// See https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
    /// </summary>
    public static class Constants
        {
            // Modifiers
            public const int NOMOD = 0x0000;
            public const int ALT = 0x0001;
            public const int CTRL = 0x0002;
            public const int SHIFT = 0x0004;
            public const int WIN = 0x0008;

            // Keys
            public const int KEY_F1 = 0x70;
            public const int KEY_F2 = 0x71;
            public const int KEY_F3 = 0x72;
            public const int KEY_F4 = 0x73;
            public const int KEY_F5 = 0x74;
            public const int KEY_F6 = 0x75;
            public const int KEY_F7 = 0x76;
            public const int KEY_F8 = 0x77;
            public const int KEY_F9 = 0x78;
            public const int KEY_F10 = 0x79;
            public const int KEY_F11 = 0x7A;
            public const int KEY_F12 = 0x7B;

            // Windows message id for hotkey
            public const int WM_HOTKEY_MSG_ID = 0x0312;
        }
    }
}
