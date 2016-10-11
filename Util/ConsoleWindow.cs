using System;
using System.Runtime.InteropServices;

namespace Util
{
    public class ConsoleWindow
    {
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int cx, int cy, bool repaint);

        public static void Show()
        {
            AllocConsole();
            var console = GetConsoleWindow();
            MoveWindow(console, 650, 50, 700, 600, true);
        }
    }
}