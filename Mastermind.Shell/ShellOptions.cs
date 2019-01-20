using System;
using System.Text;

namespace Mastermind
{
    public class ShellOptions
    {
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public ConsoleColor DefaultForegroundColor { get; set; } = ConsoleColor.Gray;

        public ConsoleColor DefaultBackgroundColor { get; set; } = ConsoleColor.Black;

        public ConsoleColor PromptColor { get; set; } = ConsoleColor.White;

        public ConsoleColor PromptQuestionColor { get; set; } = ConsoleColor.Red;

        public ConsoleColor PromptOptionColor { get; set; } = ConsoleColor.Yellow;
    }
}
