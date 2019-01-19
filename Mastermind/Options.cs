namespace Mastermind
{
    public class Options : IGameOptions
    {
        public static Options Default { get; } = new Options();

        public bool Quiet { get; set; } = false;

        public bool AllowDuplicates { get; set; } = true;
    }
}
