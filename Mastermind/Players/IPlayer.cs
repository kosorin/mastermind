namespace Mastermind.Players
{
    public interface IPlayer
    {
        PlayerType Type { get; }

        PlayerRole Role { get; }
    }
}
