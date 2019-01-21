using System;

namespace Mastermind.Players
{
    public class ComputerCodemaker : Codemaker
    {
        private readonly PegPattern _code;

        public ComputerCodemaker(IGameOptions options, PegPattern code = null) : base(options)
        {
            _code = code ?? Options.Palette.GetRandomPattern(Options);
        }

        public sealed override PlayerType Type => PlayerType.Computer;

        public override GuessResult ProcessGuess(PegPattern guess)
        {
            return guess.CompareTo(_code);
        }
    }
}
