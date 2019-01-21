using Mastermind.Players;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mastermind
{
    public static class ShellEx
    {
        public static void WriteKeyValue(string key, object value)
        {
            using (ShellColorizer.SetForeground(ConsoleColor.Black))
            {
                Shell.Write((key + ":").PadRight(16));
            }
            Shell.WriteLine(value);
        }


        public static PegPattern PromptPegPattern(string question, IPegCollectionOptions options, bool allowRandom = false)
        {
            var pegOptions = (string[])null;
            var defaultPegOption = (int?)null;
            if (allowRandom)
            {
                pegOptions = new[] { "random" };
                defaultPegOption = 0;
            }

            var pegs = Shell.Prompt(question, pegOptions, defaultPegOption, line =>
            {
                if (allowRandom && string.IsNullOrWhiteSpace(line))
                {
                    return null;
                }
                return Regex
                    .Split(line, @"\s+")
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => int.Parse(x))
                    .ToArray();
            }, value => (value == null && allowRandom) || (value != null
                && value.Length == options.Size
                && value.All(options.Palette.Contains)
                && (options.AllowDuplicates || value.Length == value.Distinct().Count())
                ));

            if (pegs == null && allowRandom)
            {
                return options.Palette.GetRandomPattern(options);
            }
            return new PegPattern(options.Palette, pegs);
        }

        public static PlayerType PromptPlayerType(string playerPositionName)
        {
            return Shell.PromptEnum<PlayerType>($"{playerPositionName} player type");
        }
    }
}
