namespace Mastermind.Players
{
    public abstract class Codemaker : ICodemaker
    {
        protected Codemaker(IGameOptions options)
        {
            Options = options;
        }

        protected IGameOptions Options { get; }

        public abstract GuessResult ProcessGuess(PegPattern guess);
    }
}
