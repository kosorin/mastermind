namespace Mastermind.Players
{
    public interface ICodebreaker
    {
        PegPattern BuildInitialGuess();

        PegPattern BuildNextGuess(GuessResult previousGuessResult);
    }
}
