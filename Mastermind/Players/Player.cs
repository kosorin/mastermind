namespace Mastermind.Players
{
    public abstract class Player : IPlayer
    {
        protected Player(IGameOptions options)
        {
            Options = options;
        }

        public abstract PlayerType Type { get; }

        public abstract PlayerRole Role { get; }

        protected IGameOptions Options { get; }
    }
}
