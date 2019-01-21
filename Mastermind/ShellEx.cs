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


        public static PegPattern PromptPegPattern(string question, IPegCollectionOptions options)
        {
            var pegs = Shell.Prompt(question, null, null, line =>
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    return null;
                }
                return Regex
                    .Split(line, @"\s+")
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => int.Parse(x))
                    .ToArray();
            }, value => value != null
                && value.Length == options.Size
                && value.All(options.Palette.Contains)
                && (options.AllowDuplicates || value.Length == value.Distinct().Count())
                );
            return new PegPattern(options.Palette, pegs);
        }

        public static PlayerType PromptPlayerType(string playerTypeName, PlayerType? defaultPlayerType = null)
        {
            return Shell.PromptEnum($"{playerTypeName} player type", defaultPlayerType);
        }
    }
}
