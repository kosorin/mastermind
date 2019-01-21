using Mastermind.Players;
using System;

namespace Mastermind
{
    internal static class Program
    {
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

            Board lastBoard = null;
            do
            {
                Shell.Clear();
                try
                {
                    var options = CreateGameOptions(lastBoard?.Options);
                    if (options == null)
                    {
                        return;
                    }

                    var codemaker = CreateCodemaker(options, lastBoard?.Codemaker);
                    var codebreaker = CreateCodebreaker(options, lastBoard?.Codebreaker);
                    var board = new Board(options, codemaker, codebreaker);

                    board.Run();

                    lastBoard = board;
                }
                catch (Exception e)
                {
                    using (ShellColorizer.Set(ConsoleColor.White, ConsoleColor.Red))
                    {
                        Shell.WriteLine($"Error: {e.Message}");
                    }
                    return;
                }
            } while (Shell.PromptBool("Play again?"));
        }

        private static GameOptions CreateGameOptions(GameOptions lastOptions)
        {
            var options = new GameOptions();

            options.Palette = new Palette(Shell.PromptInt("Number of peg color", lastOptions?.Palette.PegCount ?? 6, (1, 10)));
            ShellEx.WriteKeyValue("Peg colors", options.Palette.PegCount);

            options.AllowDuplicates = Shell.PromptBool("Allow duplicates", lastOptions?.AllowDuplicates ?? true);
            ShellEx.WriteKeyValue("Duplicates", options.AllowDuplicates ? "yes" : "no");

            var maxSize = options.AllowDuplicates ? 10 : options.Palette.PegCount;
            options.Size = Shell.PromptInt("Length of secret code", Math.Min(maxSize, lastOptions?.Size ?? 4), (1, maxSize));
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

        public static ICodemaker CreateCodemaker(IGameOptions options, ICodemaker lastCodemaker)
        {
            var type = ShellEx.PromptPlayerType("Codemaker", lastCodemaker?.Type);
            ShellEx.WriteKeyValue("Codemaker", type.ToString().ToLower());

            switch (type)
            {
            case PlayerType.Human:
                return new HumanCodemaker(options);
            case PlayerType.Computer:
                PegPattern code;
                var codeType = Shell.PromptEnum<ComputerCodeType>("Choose secret code", ComputerCodeType.Random);
                switch (codeType)
                {
                case ComputerCodeType.Random:
                    code = options.Palette.GetRandomPattern(options);
                    break;
                case ComputerCodeType.Custom:
                    code = ShellEx.PromptPegPattern("Enter secret code", options);
                    break;
                default:
                    throw new Exception($"Unknown code type");
                }
                ShellEx.WriteKeyValue("Code", codeType.ToString().ToLower());
                return new ComputerCodemaker(options, code);
            default: throw new Exception($"Unknown player type: {type}");
            }
        }

        public static ICodebreaker CreateCodebreaker(IGameOptions options, ICodebreaker lastCodebreaker)
        {
            var type = ShellEx.PromptPlayerType("Codebreaker", lastCodebreaker?.Type);
            ShellEx.WriteKeyValue("Codebreaker", type.ToString().ToLower());

            switch (type)
            {
            case PlayerType.Human:
                return new HumanCodebreaker(options);
            case PlayerType.Computer:
                PegPattern initialGuess;
                var initialGuessType = Shell.PromptEnum<ComputerInitialGuessType>("Choose initial guess", ComputerInitialGuessType.Default);
                switch (initialGuessType)
                {
                case ComputerInitialGuessType.Default:
                    initialGuess = null;
                    break;
                case ComputerInitialGuessType.Random:
                    initialGuess = options.Palette.GetRandomPattern(options);
                    break;
                case ComputerInitialGuessType.Custom:
                    initialGuess = ShellEx.PromptPegPattern("Enter initial guess", options);
                    break;
                default:
                    throw new Exception($"Unknown initial guess type");
                }
                ShellEx.WriteKeyValue("Initial guess", initialGuessType.ToString().ToLower());
                return new ComputerCodebreaker(options, initialGuess);
            default: throw new Exception($"Unknown player type: {type}");
            }
        }

        private enum ComputerCodeType
        {
            Random,
            Custom,
        }

        private enum ComputerInitialGuessType
        {
            Default,
            Random,
            Custom,
        }
    }
}
