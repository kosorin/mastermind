using System.Collections.Generic;

namespace Mastermind
{
    public interface IPegCollection : ICollection<int>
    {
        Palette Palette { get; }

        int Size { get; }
    }
}
