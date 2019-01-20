using Mastermind.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mastermind
{
    public class Board
    {
        private readonly ICodemaker _codemaker;
        private readonly ICodebreaker _codebreaker;

        private readonly List<(PegPattern, GuessResult)> _turns;

        public Board(ICodemaker codemaker, ICodebreaker codebreaker)
        {
            _codemaker = codemaker ?? throw new ArgumentNullException(nameof(codemaker));
            _codebreaker = codebreaker ?? throw new ArgumentNullException(nameof(codebreaker));

            _turns = new List<(PegPattern, GuessResult)>();
        }

        public bool Run()
        {
            if (_turns.Any())
            {
                throw new InvalidOperationException("The game is already in progress.");
            }

            using (ShellColorizer.SetForeground(ConsoleColor.DarkGreen))
            {
                Shell.WriteLine("START");
            }

            var guess = _codebreaker.BuildInitialGuess();
            while (guess != null)
            {
                Shell.Write($"{_turns.Count + 1,5}. ");
                using (ShellColorizer.SetForeground(ConsoleColor.Red))
                {
                    Shell.WriteLine(guess);
                }
                var guessResult = _codemaker.ProcessGuess(guess);
                Shell.UndoLine();

                _turns.Add((guess, guessResult));
                Shell.Write($"{_turns.Count,5}. ");
                using (ShellColorizer.SetForeground(ConsoleColor.Blue))
                {
                    Shell.Write(guess);
                    Shell.Write(' ');
                }
                using (ShellColorizer.SetForeground(ConsoleColor.Black))
                {
                    Shell.Write(guessResult);
                    Shell.Write(' ');
                }

                if (guessResult.IsVictory(guess.Size))
                {
                    using (ShellColorizer.SetForeground(ConsoleColor.DarkYellow))
                    using (ShellTextStyler.Set(ShellTextStyle.Underline))
                    {
                        Shell.WriteLine("<VICTORY>");
                    }
                    return true;
                }
                else
                {
                    Shell.WriteLine();
                }

                guess = _codebreaker.BuildNextGuess(guessResult);
            }

            using (ShellColorizer.SetForeground(ConsoleColor.Red))
            {
                Shell.WriteLine("It's too difficult to solve :(");
            }
            return false;
        }
    }
}
