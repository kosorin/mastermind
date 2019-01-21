namespace Mastermind
{
    public interface IPegCollectionOptions
    {
        bool AllowDuplicates { get; }

        Palette Palette { get; }

        int Size { get; }
    }
}
