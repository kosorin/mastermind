namespace Mastermind.Players
{
    public interface ICodebreaker : IPlayer
    {
        PegPattern BuildInitialGuess();

        PegPattern BuildNextGuess(GuessResult previousGuessResult);
    }
}
