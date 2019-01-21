using System;

namespace Mastermind.Players
{
    public class ComputerCodemaker : Codemaker
    {
        private readonly PegPattern _code;

        public ComputerCodemaker(IGameOptions options) : base(options)
        {
            _code = Options.Palette.GetRandomPattern(Options);
        }

        public ComputerCodemaker(IGameOptions options, PegPattern code) : base(options)
        {
            _code = code ?? throw new ArgumentNullException(nameof(code));
        }

        public sealed override PlayerType Type => PlayerType.Computer;

        public override GuessResult ProcessGuess(PegPattern guess)
        {
            return guess.CompareTo(_code);
        }
    }
}
