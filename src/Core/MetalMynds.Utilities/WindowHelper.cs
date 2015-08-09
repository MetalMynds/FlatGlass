using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MetalMynds.Utilities
{
    public class WindowHelper
    {
        [DllImport("User32.dll")]
        protected static extern Boolean LockWindowUpdate(IntPtr Handle);

        [DllImport("User32.dll")]
        protected static extern IntPtr GetDesktopWindow();

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        protected extern static IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        protected extern static IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, ref Point lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        protected static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential)]
        protected struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        private const int WM_SETREDRAW = 0x000B;
        private const int WM_USER = 0x400;
        private const int EM_GETEVENTMASK = (WM_USER + 59);
        private const int EM_SETEVENTMASK = (WM_USER + 69);

        private const int TCM_HITTEST = 0x130D;

        protected IntPtr BaseEventMask = IntPtr.Zero;
        protected Boolean BaseFrozen = false;
        protected Control BaseControl = null;
        protected Boolean BaseUseDesktop = false;

        public WindowHelper()
        {
            BaseUseDesktop = true;
        }

        public WindowHelper(Control Control)
        {
            BaseControl = Control;
        }

        public static int HitTest(IntPtr Handle, Point Point)
        {
            return (int)SendMessage(Handle, TCM_HITTEST, 0, ref Point);
        }

        public static IntPtr GetDesktopWindowHandle()
        {
            return GetDesktopWindow();
        }

        //IntPtr eventMask = IntPtr.Zero;
        //try {
        //// Stop redrawing: SendMessage(richTextBox1.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
        //// Stop sending of events: eventMask = SendMessage(richTextBox1.Handle, EM_GETEVENTMASK, 0, IntPtr.Zero);
        //// change colors and stuff in the RichTextBox }
        //finally {
        //// turn on events SendMessage(richTextBox1.Handle, EM_SETEVENTMASK, 0, eventMask);
        //// turn on redrawing SendMessage(richTextBox1.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
        //}

        protected virtual IntPtr GetHandle()
        {
            if (BaseUseDesktop)
            {
                return GetDesktopWindow();
            }
            else
            {
                if (BaseControl != null)
                {
                    return BaseControl.Handle;
                }
                else
                {
                    throw new InvalidOperationException(String.Format("Window Helper: Cannot Get Handle if Control reference is null."));
                }
            }
        }

        protected virtual IntPtr Disable(IntPtr Handle)
        {
            // Stop redrawing:
            SendMessage(Handle, WM_SETREDRAW, 0, IntPtr.Zero);
            // Stop sending of events:
            return SendMessage(Handle, EM_GETEVENTMASK, 0, IntPtr.Zero);
        }

        protected virtual void Enable(IntPtr Handle, IntPtr EventMask)
        {
            // turn on events
            SendMessage(Handle, EM_SETEVENTMASK, 0, EventMask);
            // turn on redrawing
            SendMessage(Handle, WM_SETREDRAW, 1, IntPtr.Zero);
        }

        public void Freeze()
        {
            IntPtr handle = GetHandle();

            //if (!LockWindowUpdate(handle))
            //{
            //    String windowName;

            //    if (!BaseUseDesktop)
            //    {
            //        if (BaseControl != null)
            //        {
            //            windowName = BaseControl.Name;
            //        }
            //        else
            //        {
            //            windowName = "{null}";
            //        }
            //    }
            //    else
            //    {
            //        windowName = "Desktop";
            //    }

            //    throw new InvalidOperationException(String.Format("WindowHelper: Unable to Lock Window [{0}] Handle [{1}]!", windowName, GetHandle()));

            //}

            BaseEventMask = Disable(handle);

            BaseFrozen = true;
        }

        public void Thaw()
        {
            IntPtr handle = GetHandle();
            //LockWindowUpdate(IntPtr.Zero);
            Enable(handle, BaseEventMask);
            BaseFrozen = false;
        }

        public Boolean IsFrozen { get { return BaseFrozen; } }

        public Boolean IsDesktop { get { return BaseUseDesktop; } }

        public Control Target { get { return BaseControl; } }

        public IntPtr Handle { get { return GetHandle(); } }

        public static Boolean TryGetWindowDetails(IntPtr Hwnd, out Size Dimensions, out Point Position)
        {
            RECT rectangle;

            if (GetWindowRect(Hwnd, out rectangle))
            {
                Dimensions = new Size(rectangle.Right - rectangle.Left, rectangle.Bottom - rectangle.Top);
                Position = new Point(rectangle.Left, rectangle.Top);

                return true;
            }
            else
            {
                Dimensions = new Size();
                Position = new Point();
                return false;
            }
        }

        public static void PositionBottomRightDesktop(Form Target)
        {
            Rectangle desktopDimenions = Screen.PrimaryScreen.WorkingArea;

            Point topLeft = new Point(desktopDimenions.Width - Target.Width - 5, desktopDimenions.Height - Target.Height - 5);

            Target.SetDesktopLocation(topLeft.X, topLeft.Y);
        }

    }
}