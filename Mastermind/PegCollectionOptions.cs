namespace Mastermind
{
    public class PegCollectionOptions : IPegCollectionOptions
    {
        public bool AllowDuplicates { get; set; }

        public Palette Palette { get; set; }

        public int Size { get; set; }
    }
}
