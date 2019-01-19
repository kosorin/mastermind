using System;
using System.Collections.Generic;
using System.Linq;

namespace Mastermind
{
    public class Solver
    {
        private readonly Board _board;
        private readonly Palette _palette;
        private readonly int _size;
        private readonly int _maxTurns;

        private readonly Random _random;
        private IList<PegPattern> _patterns;

        public Solver(Board board, int maxTurns = 0)
        {
            _board = board ?? throw new ArgumentNullException(nameof(board));
            _palette = _board.Palette;
            _size = _board.Size;
            _maxTurns = maxTurns > 0 ? maxTurns : _size * _size;

            _random = new Random();
        }

        public bool Solve()
        {
            if (_board.CurrentTurn > 0)
            {
                throw new InvalidOperationException("The game is already in progress.");
            }

            _patterns = _palette.GetAllPatterns(_size, _board.Options.AllowDuplicates);

            var guessPattern = BuildInitialGuessPattern();
            do
            {
                var guessResult = _board.Guess(guessPattern);
                if (guessResult.Check(_size))
                {
                    return true;
                }

                ProcessGuessResult(guessResult, guessPattern);

                guessPattern = BuildNextGuessPattern();
                if (guessPattern == null)
                {
                    return false;
                }

            } while (_board.CurrentTurn < _maxTurns);
            return false;
        }

        private PegPattern BuildInitialGuessPattern()
        {
            if (_board.Options.AllowDuplicates)
            {
                var left = _size / 2;
                var right = _size - left;
                var pegs = _palette.GetPegs();
                return new PegPattern(_palette, Enumerable.Repeat(pegs[0], left).Concat(Enumerable.Repeat(pegs[1], right)).ToList());
            }
            else
            {
                return new PegPattern(_palette, _palette.GetPegs().Take(_size).ToList());
            }
        }

        private PegPattern BuildNextGuessPattern()
        {
            if (_patterns.Count == 0)
            {
                return null;
            }
            return _patterns[_random.Next(_patterns.Count)];
        }

        private void ProcessGuessResult(GuessResult guessResult, PegPattern guessPattern)
        {
            var badPatterns = _patterns.Where(x => guessPattern.CompareTo(x) != guessResult);
            for (int i = _patterns.Count - 1; i >= 0; i--)
            {
                var pattern = _patterns[i];
                if (guessPattern.CompareTo(pattern) != guessResult)
                {
                    _patterns.RemoveAt(i);
                }
            }
        }
    }
}
