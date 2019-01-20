namespace Mastermind.Players
{
    public interface ICodemaker
    {
        GuessResult ProcessGuess(PegPattern guess);
    }
}
