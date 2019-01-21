namespace Mastermind.Players
{
    public abstract class Codemaker : Player, ICodemaker
    {
        protected Codemaker(IGameOptions options) : base(options)
        {
        }

        public sealed override PlayerRole Role => PlayerRole.Codemaker;

        public abstract GuessResult ProcessGuess(PegPattern guess);
    }
}
