using Mastermind.Players;
using System;

namespace Mastermind
{
    internal static class Program
    {
        private const int _headerWidth = 12;

        private static void Main()
        {
            Shell.Initialize(new ShellOptions
            {
                DefaultForegroundColor = ConsoleColor.DarkGray,
                DefaultBackgroundColor = ConsoleColor.Gray,
                PromptColor = ConsoleColor.Black,
                PromptQuestionColor = ConsoleColor.Red,
                PromptOptionColor = ConsoleColor.Magenta,
            });

            do
            {
                Shell.Clear();
                try
                {
                    var options = CreateGameOptions();
                    if (options == null)
                    {
                        return;
                    }
                    var codemaker = CreateCodemaker(options);
                    var codebreaker = CreateCodebreaker(options);
                    var board = new Board(codemaker, codebreaker);
                    board.Run();
                }
                catch (Exception e)
                {
                    using (ShellColorizer.Set(ConsoleColor.White, ConsoleColor.Red))
                    {
                        Shell.WriteLine($"Error: {e.Message}");
                    }
                }
            } while (Shell.PromptBool("Play again?"));
        }

        private static GameOptions CreateGameOptions()
        {
            var options = new GameOptions();

            options.Palette = new Palette(Shell.PromptInt("Number of peg color", 6, (1, 12)));
            ShellEx.WriteKeyValue("Peg colors", options.Palette.PegCount);

            options.AllowDuplicates = Shell.PromptBool("Allow duplicates", true);
            ShellEx.WriteKeyValue("Duplicates", options.AllowDuplicates ? "yes" : "no");

            options.Size = Shell.PromptInt("Length of secret code", 4, (1, options.AllowDuplicates ? 16 : Math.Min(options.Palette.PegCount, 12)));
            ShellEx.WriteKeyValue("Code length", options.Size);

            if (!CheckExecutionTime(options.Palette.PegCount, options.Size))
            {
                return null;
            }
            return options;
        }

        private static bool CheckExecutionTime(int palette, int size)
        {
            if (Math.Pow(palette, size) > 1_000_000)
            {
                return Shell.PromptBool("It can take some time to solve. Continue?", true);
            }
            return true;
        }

        public static ICodemaker CreateCodemaker(IGameOptions options)
        {
            var type = ShellEx.PromptPlayerType("Codemaker");
            ShellEx.WriteKeyValue("Codemaker", type.ToString().ToLower());

            switch (type)
            {
            case PlayerType.Human:
                return new HumanCodemaker(options);
            case PlayerType.Computer:
                var code = ShellEx.PromptPegPattern("Enter secret code", options.Palette, options.Size, options.AllowDuplicates, true);
                return new ComputerCodemaker(options, code);
            default: throw new Exception($"Unknown player type: {type}");
            }
        }

        public static ICodebreaker CreateCodebreaker(IGameOptions options)
        {
            var type = ShellEx.PromptPlayerType("Codebreaker");
            ShellEx.WriteKeyValue("Codebreaker", type.ToString().ToLower());

            switch (type)
            {
            case PlayerType.Human:
                return new HumanCodebreaker(options);
            case PlayerType.Computer:
                PegPattern initialGuess = null;
                if (Shell.PromptBool("Do you want to set initial guess?", false))
                {
                    initialGuess = ShellEx.PromptPegPattern("Enter initial guess", options.Palette, options.Size, options.AllowDuplicates, true);
                }
                return new ComputerCodebreaker(options, initialGuess);
            default: throw new Exception($"Unknown player type: {type}");
            }
        }
    }
}
