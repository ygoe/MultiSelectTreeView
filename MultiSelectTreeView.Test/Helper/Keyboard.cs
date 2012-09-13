using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Input;
using System.ComponentModel;

namespace MultiSelectTreeView.Test.Model.Helper
{
    static class Keyboard
    {
        public static void Right()
        {
            Press(Key.Right);
        }

        public static void Left()
        {
            Press(Key.Left);
        }

        public static void Up()
        {
            Press(Key.Up);
        }

        public static void Down()
        {
            Press(Key.Down);
        }

        public static void ShiftUp()
        {
            Press(Key.Up, Key.LeftShift);
        }

        public static void ShiftDown()
        {
            Press(Key.Down, Key.LeftShift);
        }

        public static void CtrlUp()
        {
            Press(Key.Up, Key.LeftCtrl);
        }

        public static void CtrlDown()
        {
            Press(Key.Down, Key.LeftCtrl);
        }

        public static void Space()
        {
            Press(Key.Space);
        }

        public static void CtrlSpace()
        {
            Press(Key.Space, Key.LeftCtrl);
        }

        public static void HoldShift()
        {
            Hold(Key.LeftShift);
        }

        public static void ReleaseShift()
        {
            Release(Key.LeftShift);
        }

        public static void HoldCtrl()
        {
            Hold(Key.LeftCtrl);
        }

        public static void ReleaseCtrl()
        {
            Release(Key.LeftCtrl);
        }

        #region Private methods
        private static void Hold(Key key)
        {
            SendKeyboardInput(key, true);
            SleepBetween();
        }

        private static void Release(Key key)
        {
            SendKeyboardInput(key, false);
            SleepAfter();
        }

        private static void Press(Key key)
        {
            SendKeyboardInput(key, true);
            SleepBetween();
            SendKeyboardInput(key, false);
            SleepAfter();
        }

        private static void Press(Key key, Key modifierKey)
        {
            SendKeyboardInput(modifierKey, true);
            SendKeyboardInput(key, true);
            SleepBetween();
            SleepBetween();
            SendKeyboardInput(key, false);
            SendKeyboardInput(modifierKey, false);
            SleepAfter();
        }

        private static void SleepBetween()
        {
            Thread.Sleep(10);
        }

        private static void SleepAfter()
        {
            Thread.Sleep(500);
        }

        /// <summary>
        /// Inject keyboard input into the system
        /// </summary>
        /// <param name="key">indicates the key pressed or released. Can be one of the constants defined in the Key enum</param>
        /// <param name="press">true to inject a key press, false to inject a key release</param>
        /// 
        /// <outside_see conditional="false">
        /// This API does not work inside the secure execution environment.
        /// <exception cref="System.Security.Permissions.SecurityPermission"/>
        /// </outside_see>
        private static void SendKeyboardInput(Key key, bool press)
        {
            //CASRemoval:AutomationPermission.Demand( AutomationPermissionFlag.Input );

            INPUT ki = new INPUT();
            ki.type = Const.INPUT_KEYBOARD;
            ki.union.keyboardInput.wVk = (short)KeyInterop.VirtualKeyFromKey(key);
            ki.union.keyboardInput.wScan = (short)MapVirtualKey(ki.union.keyboardInput.wVk, 0);
            int dwFlags = 0;
            if (ki.union.keyboardInput.wScan > 0)
                dwFlags |= KEYEVENTF_SCANCODE;
            if (!press)
                dwFlags |= KEYEVENTF_KEYUP;
            ki.union.keyboardInput.dwFlags = dwFlags;
            if (IsExtendedKey(key))
            {
                ki.union.keyboardInput.dwFlags |= KEYEVENTF_EXTENDEDKEY;
            }
            ki.union.keyboardInput.time = 0;
            ki.union.keyboardInput.dwExtraInfo = new IntPtr(0);
            if (SendInput(1, ref ki, Marshal.SizeOf(ki)) == 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        private static bool IsExtendedKey(Key key)
        {
            // From the SDK:
            // The extended-key flag indicates whether the keystroke message originated from one of
            // the additional keys on the enhanced keyboard. The extended keys consist of the ALT and
            // CTRL keys on the right-hand side of the keyboard; the INS, DEL, HOME, END, PAGE UP,
            // PAGE DOWN, and arrow keys in the clusters to the left of the numeric keypad; the NUM LOCK
            // key; the BREAK (CTRL+PAUSE) key; the PRINT SCRN key; and the divide (/) and ENTER keys in
            // the numeric keypad. The extended-key flag is set if the key is an extended key. 
            //
            // - docs appear to be incorrect. Use of Spy++ indicates that break is not an extended key.
            // Also, menu key and windows keys also appear to be extended.
            return key == Key.RightAlt
                || key == Key.RightCtrl
                || key == Key.NumLock
                || key == Key.Insert
                || key == Key.Delete
                || key == Key.Home
                || key == Key.End
                || key == Key.Prior
                || key == Key.Next
                || key == Key.Up
                || key == Key.Down
                || key == Key.Left
                || key == Key.Right
                || key == Key.Apps
                || key == Key.RWin
                || key == Key.LWin;

            // Note that there are no distinct values for the following keys:
            // numpad divide
            // numpad enter
        }
        #endregion

        #region Constants
        public const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        public const int KEYEVENTF_KEYUP = 0x0002;
        public const int KEYEVENTF_SCANCODE = 0x0008;
        #endregion

        #region Native methods
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int MapVirtualKey(int nVirtKey, int nMapType);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SendInput(int nInputs, ref INPUT mi, int cbSize);
        #endregion
    }
}
