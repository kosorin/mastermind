namespace Mastermind.Players
{
    public class HumanCodemaker : Codemaker
    {
        public HumanCodemaker(IGameOptions options) : base(options)
        {
        }

        public sealed override PlayerType Type => PlayerType.Human;

        public override GuessResult ProcessGuess(PegPattern guess)
        {
            var positionCount = Shell.PromptInt("Number of pegs in correct position", 0, (0, guess.Size));
            var colorCount = (guess.Size - positionCount > 1)
                ? Shell.PromptInt("Number of pegs in wrong position", 0, (0, guess.Size - positionCount))
                : 0;
            return new GuessResult(positionCount, colorCount);
        }
    }
}
