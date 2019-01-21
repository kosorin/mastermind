namespace Mastermind.Players
{
    public abstract class Codebreaker : Player, ICodebreaker
    {
        protected Codebreaker(IGameOptions options) : base(options)
        {
        }

        public sealed override PlayerRole Role => PlayerRole.Codebreaker;

        protected PegPattern LastGuess { get; set; }

        public abstract PegPattern BuildNextGuess(GuessResult previousGuessResult);

        public abstract PegPattern BuildInitialGuess();
    }
}
