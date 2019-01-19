using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mastermind
{
    public class Board
    {
        private readonly PegPattern _code;
        private readonly List<(PegPattern, GuessResult)> _guessResults;

        public Board(PegPattern code, IGameOptions options)
        {
            Options = options;

            _code = code ?? throw new ArgumentNullException(nameof(code));
            _guessResults = new List<(PegPattern, GuessResult)>();

            if (!Options.AllowDuplicates && _code.Size != _code.Distinct().Count())
            {
                throw new ArgumentException("Code pattern contains duplicates.");
            }
        }


        public IGameOptions Options { get; }

        public Palette Palette => _code.Palette;

        public int Size => _code.Size;

        public IReadOnlyList<(PegPattern, GuessResult)> GuessResults => _guessResults;

        public int CurrentTurn => _guessResults.Count;


        public GuessResult Guess(PegPattern guess)
        {
            var guessResult = guess.CompareTo(_code);
            _guessResults.Add((guess, guessResult));
            return guessResult;
        }

        public string ToPrintString()
        {
            var sb = new StringBuilder();

            var turnMaxWidth = _guessResults.Count.GetWidth();
            var turnSuffix = ". ";

            var matchMaxWidth = _code.Size.GetWidth();
            for (int turn = 0; turn < _guessResults.Count; turn++)
            {
                var (guess, guessResult) = _guessResults[turn];
                sb.Append((turn + 1).ToString().PadLeft(turnMaxWidth));
                sb.Append(turnSuffix);
                sb.Append(guess);
                sb.Append(" ");
                sb.Append(guessResult);
                sb.AppendLine();
            }

            var code = _code.ToString();
            sb.Append('-', turnMaxWidth + turnSuffix.Length + code.Length);
            sb.AppendLine();
            sb.Append(' ', turnMaxWidth + turnSuffix.Length);
            sb.Append(code);

            return sb.ToString();
        }
    }
}
