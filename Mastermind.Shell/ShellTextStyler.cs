using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Mastermind
{
    public class ShellTextStyler : IDisposable
    {
        private readonly ShellTextStyle _offStyle;
        private readonly ShellTextStyle _onStyle;

        private ShellTextStyler(ShellTextStyle offStyle, ShellTextStyle onStyle)
        {
            _offStyle = offStyle;
            _onStyle = onStyle;
        }

        public void Dispose()
        {
            _currentStyle = _onStyle;
            SetCore(_offStyle, false);
        }


        private static ShellTextStyle _currentStyle = ShellTextStyle.None;

        private static readonly Dictionary<ShellTextStyle, int> _controlCodeTable = new Dictionary<ShellTextStyle, int>
        {
            [ShellTextStyle.Underline] = 4,
        };


        internal static void Initialize()
        {
            var handle = GetStdHandle(STD_OUTPUT_HANDLE);
            GetConsoleMode(handle, out var mode);
            mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
            SetConsoleMode(handle, mode);
        }


        public static ShellTextStyler Set(ShellTextStyle style)
        {
            if (style == ShellTextStyle.None)
            {
                throw new ArgumentOutOfRangeException(nameof(style));
            }

            var styler = new ShellTextStyler(style, _currentStyle);
            _currentStyle = style;
            SetCore(style, true);
            return styler;
        }


        private static void SetCore(ShellTextStyle style, bool on)
        {
            if (style == ShellTextStyle.None)
            {
                throw new ArgumentOutOfRangeException(nameof(style));
            }

            Console.Write($"\x1B[{(on ? "" : "2")}{_controlCodeTable[style]}m");
        }


        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 4;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
    }
}
