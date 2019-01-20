namespace Mastermind
{
    public class GameOptions : IGameOptions
    {
        public bool AllowDuplicates { get; set; } = true;

        public Palette Palette { get; set; } = new Palette(6);

        public int Size { get; set; } = 4;
    }
}
