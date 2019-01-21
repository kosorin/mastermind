using System;
using System.Collections.Generic;
using System.Linq;

namespace Mastermind.Players
{
    public class ComputerCodebreaker : Codebreaker
    {
        private readonly PegPattern _initialGuess;

        private readonly Random _random = new Random();
        private LinkedList<PegPattern> _availableGuesses;

        public ComputerCodebreaker(IGameOptions options, PegPattern initialGuess) : base(options)
        {
            _initialGuess = initialGuess;
        }

        public sealed override PlayerType Type => PlayerType.Computer;


        public override PegPattern BuildInitialGuess()
        {
            GenerateAvailableGuesses();

            if (_initialGuess != null)
            {
                LastGuess = _initialGuess;
            }
            else if (Options.AllowDuplicates)
            {
                var left = Options.Size / 2;
                var right = Options.Size - left;
                var pegs = Options.Palette.GetPegs();
                LastGuess = new PegPattern(Options.Palette, Enumerable.Repeat(pegs[0], left).Concat(Enumerable.Repeat(pegs[1], right)).ToList());
            }
            else
            {
                LastGuess = new PegPattern(Options.Palette, Options.Palette.GetPegs().Take(Options.Size).ToList());
            }

            return LastGuess;
        }

        public override PegPattern BuildNextGuess(GuessResult previousGuessResult)
        {
            ProcessGuessResult(previousGuessResult);

            if (_availableGuesses.Count > 0)
            {
                LastGuess = _availableGuesses.First.Value; //_availableGuesses[_random.Next(_availableGuesses.Count)];
            }
            else
            {
                LastGuess = null;
            }

            return LastGuess;
        }


        private void GenerateAvailableGuesses()
        {
            _availableGuesses = Options.Palette.GetAllPatterns(Options);
        }

        private void ProcessGuessResult(GuessResult guessResult)
        {
            var currentNode = _availableGuesses.First;
            while (currentNode != null)
            {
                var nextNode = currentNode.Next;
                var pattern = currentNode.Value;
                if (LastGuess.CompareTo(pattern) != guessResult)
                {
                    _availableGuesses.Remove(currentNode);
                }
                currentNode = nextNode;
            }
        }
    }
}
