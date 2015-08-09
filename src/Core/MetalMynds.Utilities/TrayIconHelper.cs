// -----------------------------------------------------------------------
// <copyright file="TrayIconHelper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MetalMynds.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Runtime.InteropServices;
    using Microsoft.Win32;
    using System.Collections;
    using System.Diagnostics;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class TrayIconHelper
    {

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        public static void RefreshIcons()
        {
            IntPtr systemTrayContainerHandle = FindWindow("Shell_TrayWnd", null);
            IntPtr systemTrayHandle = FindWindowEx(systemTrayContainerHandle, IntPtr.Zero, "TrayNotifyWnd", null);
            IntPtr sysPagerHandle = FindWindowEx(systemTrayHandle, IntPtr.Zero, "SysPager", null);
            IntPtr notificationAreaHandle = FindWindowEx(sysPagerHandle, IntPtr.Zero, "ToolbarWindow32", "Notification Area");
            if (notificationAreaHandle == IntPtr.Zero)
            {
                notificationAreaHandle = FindWindowEx(sysPagerHandle, IntPtr.Zero, "ToolbarWindow32", "User Promoted Notification Area");
                IntPtr notifyIconOverflowWindowHandle = FindWindow("NotifyIconOverflowWindow", null);
                IntPtr overflowNotificationAreaHandle = FindWindowEx(notifyIconOverflowWindowHandle, IntPtr.Zero, "ToolbarWindow32", "Overflow Notification Area");
                RefreshTrayArea(overflowNotificationAreaHandle);
            }
            RefreshTrayArea(notificationAreaHandle);
        }

        private static void RefreshTrayArea(IntPtr windowHandle)
        {
            const uint wmMousemove = 0x0200;
            RECT rect;
            GetClientRect(windowHandle, out rect);
            for (var x = 0; x < rect.right; x += 5)
                for (var y = 0; y < rect.bottom; y += 5)
                    SendMessage(windowHandle, wmMousemove, 0, (y << 16) + x);
        }

        public static void Main()
        {

            string prName = null;
            RegistryKey myKey = Registry.CurrentUser.OpenSubKey("Software\\Classes\\Local Settings\\Software\\Microsoft\\Windows\\CurrentVersion\\TrayNotify", true);
            byte[] all = (byte[])myKey.GetValue("IconStreams");
            byte[] allwithoutheader = new byte[all.Length - 20];
            byte[] header = new byte[20];

            //Parse Header
            //------------------------
            for (int i = 0; i < all.Length; i++)
            {
                if (i <= 19)
                {
                    header[i] = all[i];
                }
                else
                {
                    allwithoutheader[i - 20] = all[i];
                }
            }
            //------------------------
            ArrayList blocklist = storeBlocks(allwithoutheader);
            Console.WriteLine("Name(or part) of program:");
            prName = Console.ReadLine();

            string alldataString = null;
            bool found = false;
            int value = 0;
            int blocknumber = 0;

            for (int i = 0; i < blocklist.Count; i++)
            {
                for (int j = 0; j < 1639; j++)
                {
                    if (j < 528)
                    {
                        alldataString += System.Convert.ToChar(((List<byte>)(blocklist[i]))[j]);
                        if (j == 526)
                        {
                            string transformed = Transform(alldataString);
                            string trimmed = System.Text.RegularExpressions.Regex.Replace(transformed, "\0+", "");
                            if (trimmed.Contains(prName))
                            {
                                alldataString = null;
                                Console.WriteLine("\n===================\n" + prName + " block found!"); //Block in ArrayList
                                Console.WriteLine("current setting --> " + ((List<byte>)(blocklist[i]))[528]);
                                Console.WriteLine("===================\nsettings: \n2 = show icon and notifications, \n1 = hide icon and notifications, \n0 = only show notifications\n===================");
                                blocknumber = i;
                                found = true;
                                break;
                            }
                        }
                    }
                }
            }

            if (found == false)
            {
                Console.WriteLine("Nothing found");
                Console.ReadLine();
                Environment.Exit(1);
            }

            Console.WriteLine("new setting:");
            value = Convert.ToInt32(Console.ReadLine());
            ((List<byte>)(blocklist[blocknumber]))[528] = (byte)(value);

            byte[] alloriginal = getOriginalFormat(blocklist, allwithoutheader.Length);
            byte[] combined = Combine(header, alloriginal);

            myKey.SetValue("IconStreams", combined);
            Console.WriteLine("Successfully wrote to Registry, please restart explorer.exe");


            Process[] pp = Process.GetProcessesByName("explorer");
            foreach (Process p in pp)
            {

                p.Kill();
            }
            Process.Start("explorer.exe");
        }

        private static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

        private static ArrayList storeBlocks(byte[] all)
        {
            ArrayList main = new ArrayList();
            List<byte> bytelist = new List<byte>();

            int help = 1639;
            int j = 0;
            for (int i = 0; i < all.Length; i++)
            {
                bytelist.Add(all[i]);
                if (i == help)
                {
                    main.Add(bytelist);
                    bytelist = new List<byte>();
                    help = help + 1640;
                }
            }
            return main;
        }

        private static byte[] getOriginalFormat(ArrayList list, int bufferSize)
        {
            int help = 0;
            byte[] original = new byte[bufferSize];
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < (((List<byte>)(list[i])).Count); j++)
                {

                    original[help] = (((List<byte>)(list[i]))[j]);
                    help++;
                }
            }
            return original;
        }
        private static string Transform(string value)
        {
            char[] array = value.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                int number = (int)array[i];

                if (number >= 'a' && number <= 'z')
                {
                    if (number > 'm')
                    {
                        number -= 13;
                    }
                    else
                    {
                        number += 13;
                    }
                }
                else if (number >= 'A' && number <= 'Z')
                {
                    if (number > 'M')
                    {
                        number -= 13;
                    }
                    else
                    {
                        number += 13;
                    }
                }
                array[i] = (char)number;
            }
            return new string(array);
        }
    }

}

