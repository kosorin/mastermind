namespace Mastermind.Players
{
    public interface ICodemaker : IPlayer
    {
        GuessResult ProcessGuess(PegPattern guess);
    }
}
