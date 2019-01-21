namespace Mastermind.Players
{
    public class HumanCodebreaker : Codebreaker
    {
        public HumanCodebreaker(IGameOptions options) : base(options)
        {
        }

        public sealed override PlayerType Type => PlayerType.Human;


        public override PegPattern BuildInitialGuess()
        {
            return BuildGuess();
        }

        public override PegPattern BuildNextGuess(GuessResult previousGuessResult)
        {
            return BuildGuess();
        }


        private PegPattern BuildGuess()
        {
            return ShellEx.PromptPegPattern("Guess the secret code", Options);
        }
    }
}
