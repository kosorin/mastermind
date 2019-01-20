namespace Mastermind
{
    public interface IGameOptions
    {
        bool AllowDuplicates { get; }

        Palette Palette { get; }

        int Size { get; }
    }
}
