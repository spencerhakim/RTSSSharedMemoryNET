using System;
using System.Runtime.InteropServices;

namespace RTSSDemo
{
    public class ExitHandler
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIdEnableItem, uint uEnable);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport ("user32.dll" )]
        private static extern IntPtr RemoveMenu(IntPtr hMenu, uint nPosition, uint wFlags);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(HandlerRountine handlerRountine, bool add);

        private const uint SC_CLOSE = 0xF060;
        private const uint MF_GRAYED = 0x00000001;
        private const uint MF_BYCOMMAND = 0x00000000;

        public delegate bool HandlerRountine(CtrlType sig);
        private static HandlerRountine _handlerRountine;

        public enum CtrlType
        {
            CtrlCEvent = 0,
            CtrlBreakEvent = 1,
            CtrlCloseEvent = 2,
            CtrlLogoffEvent = 5,
            CtrlShutdownEvent = 6
        }

        public static void Init(HandlerRountine handlerRountine)
        {
            _handlerRountine = handlerRountine;
            SetConsoleCtrlHandler(_handlerRountine, true);
            DisableConsoleClose();
        }

        private static void DisableConsoleClose()
        {
            var hMenu = GetSystemMenu(GetConsoleWindow(), false);
            EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED); //disables the upper-right Close (X) button in the titlebar
            RemoveMenu(hMenu, SC_CLOSE, MF_BYCOMMAND); //removes the Close option in the Alt-Space menu
        }
    }
}