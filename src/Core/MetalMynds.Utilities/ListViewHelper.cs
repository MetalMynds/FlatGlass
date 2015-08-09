using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MetalMynds.Utilities
{
    public class ListViewHelper
    {

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        private static extern Int32 SendMessage(IntPtr hwnd, Int32 wMsg, Int32 wParam, Int32 lParam);

        private const int LVM_FIRST = 0x1000;
        private const int LVM_SETICONSPACING = LVM_FIRST + 53;


        public static void SetIconSpacing(ListView ListView,int Height, int Width)
        {
             
            SendMessage(ListView.Handle, LVM_SETICONSPACING, 0, Width * 65536 + Height);

            ListView.Refresh();

        }

        public static void AutoSizeColumns(ListView ListView)
        {

            foreach (ColumnHeader header in ListView.Columns)
            {

                int contentSize = int.MinValue;
                int headerSize = int.MinValue;

                header.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                headerSize = header.Width;

                header.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                contentSize = header.Width;

                if (headerSize > contentSize)
                {
                    header.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                }

            }

        }

        public static void SelectAll(ListView ListView) 
        {
            foreach (ListViewItem item in ListView.Items)
            {
                item.Selected = true;
            }
        }
    }
}
