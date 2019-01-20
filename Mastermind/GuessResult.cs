using System;
using System.Linq;

namespace Mastermind
{
    public class GuessResult : IEquatable<GuessResult>
    {
        public static char PositionChar { get; } = '\u25CF';

        public static char ColorChar { get; } = '\u25CB';


        public GuessResult(int positionCount, int colorCount)
        {
            PositionCount = positionCount;
            ColorCount = colorCount;
        }


        public int PositionCount { get; }

        public int ColorCount { get; }


        public bool IsVictory(int size)
        {
            return PositionCount == size;
        }

        // TODO: Validate result
        public bool IsValid(int size)
        {
            return PositionCount + ColorCount < size
                || (PositionCount + ColorCount == size && ColorCount > 1);
        }


        public override string ToString()
        {
            var positions = Enumerable.Repeat(PositionChar, PositionCount);
            var colors = Enumerable.Repeat(ColorChar, ColorCount);
            return string.Join(' ', positions.Concat(colors));
        }


        public bool Equals(GuessResult other)
        {
            if (ReferenceEquals(other, this))
            {
                return true;
            }
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            return EqualsCore(other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, this))
            {
                return true;
            }
            if (ReferenceEquals(obj, null))
            {
                return false;
            }
            return obj is GuessResult other && EqualsCore(other);
        }

        public static bool operator ==(GuessResult left, GuessResult right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            if (ReferenceEquals(left, null))
            {
                return false;
            }
            if (ReferenceEquals(right, null))
            {
                return false;
            }
            return left.EqualsCore(right);
        }

        public static bool operator !=(GuessResult left, GuessResult right)
        {
            return !(left == right);
        }

        protected virtual bool EqualsCore(GuessResult other)
        {
            return PositionCount == other.PositionCount
                && ColorCount == other.ColorCount;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PositionCount, ColorCount);
        }
    }
}
