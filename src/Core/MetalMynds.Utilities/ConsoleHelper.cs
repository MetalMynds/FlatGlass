using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace MetalMynds.Utilities
{
    public class ConsoleHelper
    {

        [DllImport("kernel32.dll")]
        protected static extern Boolean AllocConsole();
        [DllImport("kernel32.dll")]
        protected static extern Boolean FreeConsole();

        protected static Boolean BaseAllocated = false;

        public ConsoleHelper()
        {

        }

        public static Boolean Show() {

            try
            {
                BaseAllocated = AllocConsole();
            }
            catch
            {
                BaseAllocated = false;
            }

            return BaseAllocated;

        }

        public static Boolean Hide()
        {
            if (BaseAllocated)
            {
                FreeConsole();
                BaseAllocated = false;
            }

            return BaseAllocated;

        }

        public static Boolean IsVisible { get { return BaseAllocated; } }

        public static void WriteHighligtedLine(String Text, String Hightlight, ConsoleColor HighlightBackground, ConsoleColor HighlightForeground)
        {
            String[] splits = new String[] {Hightlight};

            String[] parts = Text.Split(splits, StringSplitOptions.None);

            if (parts.Length == 2) {
                Console.Write(parts[0]);
                WriteColored(Hightlight,HighlightBackground, HighlightForeground);
                Console.Write(parts[1]);
                Console.Write(String.Format("\r\n"));
            } else {
                Console.WriteLine(Text);
            }

        }

        public static void WriteColored(String Text, ConsoleColor Background, ConsoleColor Foreground) 
        {
            ConsoleColor existingBackground = Console.BackgroundColor;
            ConsoleColor existingForeground = Console.ForegroundColor;

            Console.ForegroundColor = Foreground;
            Console.BackgroundColor = Background;
            Console.Write(Text);
            Console.ForegroundColor = existingForeground;
            Console.BackgroundColor = existingBackground;
        }

        public static void WriteColoredLine(String Text, ConsoleColor Background, ConsoleColor Foreground)
        {
            ConsoleColor existingBackground = Console.BackgroundColor;
            ConsoleColor existingForeground = Console.ForegroundColor;

            Console.ForegroundColor = Foreground;
            Console.BackgroundColor = Background;
            Console.WriteLine(Text);            
            Console.BackgroundColor = existingBackground;
            Console.ForegroundColor = existingForeground;
        }

    }
}
