namespace Mastermind.Players
{
    public abstract class Codebreaker : ICodebreaker
    {
        protected Codebreaker(IGameOptions options)
        {
            Options = options;
        }

        protected IGameOptions Options { get; }

        protected PegPattern LastGuess { get; set; }

        public abstract PegPattern BuildNextGuess(GuessResult previousGuessResult);

        public abstract PegPattern BuildInitialGuess();
    }
}
