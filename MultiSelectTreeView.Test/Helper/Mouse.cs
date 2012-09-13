namespace MultiSelectTreeView.Test.Model.Helper
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows;
    using System.Windows.Automation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Class for Keyboard use
    /// </summary>
    public static class Mouse
    {
        #region Constants
        const int SM_SWAPBUTTON = 23;
        const int SM_XVIRTUALSCREEN = 76;
        const int SM_YVIRTUALSCREEN = 77;
        const int SM_CXVIRTUALSCREEN = 78;
        const int SM_CYVIRTUALSCREEN = 79;
        const int MOUSEEVENTF_VIRTUALDESK = 0x4000;
        #endregion

        #region Specific tree methods
        internal static void ExpandCollapseClick(Element element)
        {
            bool oldExpandState = element.IsExpanded;
            AutomationElement toggleButton = element.Ae.FindDescendantByAutomationId(ControlType.Button, "Expander");

            try
            {
                Click(toggleButton.GetClickablePoint());
            }
            catch (Exception ex)
            {
            }
            SleepAfter();

            if (oldExpandState == element.IsExpanded)
                throw new InvalidOperationException("Changing expand state did not work");
        }

        internal static void Click(Element element)
        {
            Click(element.Ae);
            SleepAfter();
        }

        internal static void ShiftClick(Element element)
        {
            Keyboard.HoldShift();
            Click(element.Ae);
            SleepBetween();
            Keyboard.ReleaseShift();
        }

        internal static void CtrlClick(Element element)
        {
            Keyboard.HoldCtrl();
            Click(element.Ae);
            SleepBetween();
            Keyboard.ReleaseCtrl();
        }

        private static void SleepAfter()
        {
            Thread.Sleep(500);
        }

        private static void SleepBetween()
        {
            Thread.Sleep(10);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Inject pointer input into the system
        /// </summary>
        /// <param name="x">x coordinate of pointer, if Move flag specified</param>
        /// <param name="y">y coordinate of pointer, if Move flag specified</param>
        /// <param name="data">wheel movement, or mouse X button, depending on flags</param>
        /// <param name="flags">flags to indicate which type of input occurred - move, button press/release, wheel move, etc.</param>
        /// <remarks>x, y are in pixels. If Absolute flag used, are relative to desktop origin.</remarks>
        /// <outside_see conditional="false">
        /// This API does not work inside the secure execution environment.
        /// <exception cref="System.Security.Permissions.SecurityPermission"/>
        /// </outside_see>
        private static void SendMouseInput(double x, double y, int data, SendMouseInputFlags flags)
        {
            //CASRemoval:AutomationPermission.Demand( AutomationPermissionFlag.Input );

            int intflags = (int)flags;

            if ((intflags & (int)SendMouseInputFlags.Absolute) != 0)
            {
                int vscreenWidth = GetSystemMetrics(SM_CXVIRTUALSCREEN);
                int vscreenHeight = GetSystemMetrics(SM_CYVIRTUALSCREEN);
                int vscreenLeft = GetSystemMetrics(SM_XVIRTUALSCREEN);
                int vscreenTop = GetSystemMetrics(SM_YVIRTUALSCREEN);

                // Absolute input requires that input is in 'normalized' coords - with the entire
                // desktop being (0,0)...(65535,65536). Need to convert input x,y coords to this
                // first.
                //
                // In this normalized world, any pixel on the screen corresponds to a block of values
                // of normalized coords - eg. on a 1024x768 screen,
                // y pixel 0 corresponds to range 0 to 85.333,
                // y pixel 1 corresponds to range 85.333 to 170.666,
                // y pixel 2 correpsonds to range 170.666 to 256 - and so on.
                // Doing basic scaling math - (x-top)*65536/Width - gets us the start of the range.
                // However, because int math is used, this can end up being rounded into the wrong
                // pixel. For example, if we wanted pixel 1, we'd get 85.333, but that comes out as
                // 85 as an int, which falls into pixel 0's range - and that's where the pointer goes.
                // To avoid this, we add on half-a-"screen pixel"'s worth of normalized coords - to
                // push us into the middle of any given pixel's range - that's the 65536/(Width*2)
                // part of the formula. So now pixel 1 maps to 85+42 = 127 - which is comfortably
                // in the middle of that pixel's block.
                // The key ting here is that unlike points in coordinate geometry, pixels take up
                // space, so are often better treated like rectangles - and if you want to target
                // a particular pixel, target its rectangle's midpoint, not its edge.
                x = ((x - vscreenLeft) * 65536) / vscreenWidth + 65536 / (vscreenWidth * 2);
                y = ((y - vscreenTop) * 65536) / vscreenHeight + 65536 / (vscreenHeight * 2);

                intflags |= MOUSEEVENTF_VIRTUALDESK;
            }

            INPUT mi = new INPUT();
            mi.type = Const.INPUT_MOUSE;
            mi.union.mouseInput.dx = (int)x;
            mi.union.mouseInput.dy = (int)y;
            mi.union.mouseInput.mouseData = data;
            mi.union.mouseInput.dwFlags = intflags;
            mi.union.mouseInput.time = 0;
            mi.union.mouseInput.dwExtraInfo = new IntPtr(0);
            //Console.WriteLine("Sending");
            if (SendInput(1, ref mi, Marshal.SizeOf(mi)) == 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }
        #endregion

        #region Public methods
        public static Point Loaction()
        {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        /// <summary>
        /// Move the mouse to a point. 
        /// </summary>
        /// <param name="pt">The point that the mouse will move to.</param>
        /// <remarks>pt are in pixels that are relative to desktop origin.</remarks>
        /// 
        /// <outside_see conditional="false">
        /// This API does not work inside the secure execution environment.
        /// <exception cref="System.Security.Permissions.SecurityPermission"/>
        /// </outside_see>
        public static void MoveTo(Point pt)
        {
            SendMouseInput(pt.X, pt.Y, 0, SendMouseInputFlags.Move | SendMouseInputFlags.Absolute);
        }

        /// <summary>
        /// Move the mouse to an element. 
        /// </summary>
        /// <param name="el">The element that the mouse will move to</param>
        /// <exception cref="NoClickablePointException">If there is not clickable point for the element</exception>
        /// 
        /// <outside_see conditional="false">
        /// This API does not work inside the secure execution environment.
        /// <exception cref="System.Security.Permissions.SecurityPermission"/>
        /// </outside_see>
        public static void MoveTo(AutomationElement el)
        {
            if (el == null)
            {
                throw new ArgumentNullException("el");
            }
            MoveTo(el.GetClickablePoint());
        }

        /// <summary>
        /// Move the mouse to a point and click.  The primary mouse button will be used
        /// this is usually the left button except if the mouse buttons are swaped.
        /// </summary>
        /// <param name="pt">The point to click at</param>
        /// <remarks>pt are in pixels that are relative to desktop origin.</remarks>
        /// 
        /// <outside_see conditional="false">
        /// This API does not work inside the secure execution environment.
        /// <exception cref="System.Security.Permissions.SecurityPermission"/>
        /// </outside_see>
        public static void MoveToAndClick(Point pt)
        {
            SendMouseInput(pt.X, pt.Y, 0, SendMouseInputFlags.Move | SendMouseInputFlags.Absolute);

            RawClick(pt, true);
        }

        /// <summary>
        /// Move the mouse to a point and double click.  The primary mouse button will be used
        /// this is usually the left button except if the mouse buttons are swaped.
        /// </summary>
        /// <param name="pt">The point to click at</param>
        /// <remarks>pt are in pixels that are relative to desktop origin.</remarks>
        /// 
        /// <outside_see conditional="false">
        /// This API does not work inside the secure execution environment.
        /// <exception cref="System.Security.Permissions.SecurityPermission"/>
        /// </outside_see>
        public static void MoveToAndDoubleClick(Point pt)
        {
            SendMouseInput(pt.X, pt.Y, 0, SendMouseInputFlags.Move | SendMouseInputFlags.Absolute);

            RawClick(pt, true);
            Thread.Sleep(300);
            RawClick(pt, true);
        }

        private static void RawClick(Point pt, bool left)
        {
            // send SendMouseInput works in term of the phisical mouse buttons, therefore we need
            // to check to see if the mouse buttons are swapped because this method need to use the primary
            // mouse button.
            if (ButtonsSwapped() != left)
            {
                // the mouse buttons are not swaped the primary is the left
                SendMouseInput(pt.X, pt.Y, 0, SendMouseInputFlags.LeftDown | SendMouseInputFlags.Absolute);
                SendMouseInput(pt.X, pt.Y, 0, SendMouseInputFlags.LeftUp | SendMouseInputFlags.Absolute);
            }
            else
            {
                // the mouse buttons are swaped so click the right button which as actually the primary
                SendMouseInput(pt.X, pt.Y, 0, SendMouseInputFlags.RightDown | SendMouseInputFlags.Absolute);
                SendMouseInput(pt.X, pt.Y, 0, SendMouseInputFlags.RightUp | SendMouseInputFlags.Absolute);
            }
        }

        private static bool ButtonsSwapped()
        {
            return GetSystemMetrics(SM_SWAPBUTTON) != 0;
        }

        public static void MoveToAndRightClick(Point pt)
        {
            SendMouseInput(pt.X, pt.Y, 0, SendMouseInputFlags.Move | SendMouseInputFlags.Absolute);

            RawClick(pt, false);
        }

        public static void Click(Point point)
        {
            MoveToAndClick(point);
        }

        private static void Click(AutomationElement element)
        {
            try
            {
                MoveToAndClick(element.GetClickablePoint());
            }
            catch (Exception ex)
            {

            }
        }

        internal static void DoubleClick(Element element)
        {
            DoubleClick(element.Ae);
            SleepAfter();
        }

        private static void DoubleClick(AutomationElement element)
        {
            MoveToAndDoubleClick(element.GetClickablePoint());
        }

        private static void RightClick(Point point)
        {
            MoveToAndRightClick(point);
        }
        #endregion

        #region Native types
        /// <summary>
        /// Flags for Input.SendMouseInput, indicate whether movent took place,
        /// or whether buttons were pressed or released.
        /// </summary>
        [Flags]
        public enum SendMouseInputFlags
        {
            /// <summary>Specifies that the pointer moved.</summary>
            Move = 0x0001,
            /// <summary>Specifies that the left button was pressed.</summary>
            LeftDown = 0x0002,
            /// <summary>Specifies that the left button was released.</summary>
            LeftUp = 0x0004,
            /// <summary>Specifies that the right button was pressed.</summary>
            RightDown = 0x0008,
            /// <summary>Specifies that the right button was released.</summary>
            RightUp = 0x0010,
            /// <summary>Specifies that the middle button was pressed.</summary>
            MiddleDown = 0x0020,
            /// <summary>Specifies that the middle button was released.</summary>
            MiddleUp = 0x0040,
            /// <summary>Specifies that the x button was pressed.</summary>
            XDown = 0x0080,
            /// <summary>Specifies that the x button was released. </summary>
            XUp = 0x0100,
            /// <summary>Specifies that the wheel was moved</summary>
            Wheel = 0x0800,
            /// <summary>Specifies that x, y are absolute, not relative</summary>
            Absolute = 0x8000,
        };

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };
        #endregion

        #region Native methods
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int metric);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SendInput(int nInputs, ref INPUT mi, int cbSize);
        #endregion
    }
}
