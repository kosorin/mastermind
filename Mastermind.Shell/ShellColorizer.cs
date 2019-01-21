using System;

namespace Mastermind
{
    public class ShellColorizer : IDisposable
    {
        private readonly ConsoleColor _foregroundColor;
        private readonly ConsoleColor _backgroundColor;

        private ShellColorizer(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            _foregroundColor = foregroundColor;
            _backgroundColor = backgroundColor;
        }

        public void Dispose()
        {
            SetCore(_foregroundColor, _backgroundColor);
        }


        private static ConsoleColor DefaultForegroundColor { get; set; }

        private static ConsoleColor DefaultBackgroundColor { get; set; }


        internal static void Initialize(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            DefaultForegroundColor = foregroundColor;
            DefaultBackgroundColor = backgroundColor;

            Clear();
        }

        internal static void Clear()
        {
            Console.ForegroundColor = DefaultForegroundColor;
            Console.BackgroundColor = DefaultBackgroundColor;
            Console.Clear();
        }


        public static ShellColorizer Set(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            var colorizer = new ShellColorizer(Console.ForegroundColor, Console.BackgroundColor);
            SetCore(foregroundColor, backgroundColor);
            return colorizer;
        }

        public static ShellColorizer SetForeground(ConsoleColor color)
        {
            return Set(color, Console.BackgroundColor);
        }

        public static ShellColorizer SetBackground(ConsoleColor color)
        {
            return Set(Console.ForegroundColor, color);
        }


        private static void SetCore(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
        }
    }
}
