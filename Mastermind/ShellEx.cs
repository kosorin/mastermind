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


        public static PegPattern PromptPegPattern(string question, Palette palette, int size, bool allowDuplicates, bool allowRandom = false)
        {
            var options = (string[])null;
            var defaultOption = (int?)null;
            if (allowRandom)
            {
                options = new[] { "random" };
                defaultOption = 0;
            }

            var pegs = Shell.Prompt(question, options, defaultOption, line =>
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
                && value.Length == size
                && value.All(palette.Contains)
                && (allowDuplicates || value.Length == value.Distinct().Count())
                ));

            if (pegs == null && allowRandom)
            {
                return palette.GetRandomPattern(size, allowDuplicates);
            }
            return new PegPattern(palette, pegs);
        }

        public static PlayerType PromptPlayerType(string playerPositionName)
        {
            return Shell.PromptEnum<PlayerType>($"{playerPositionName} player type");
        }
    }
}
