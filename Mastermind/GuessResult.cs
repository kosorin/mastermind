using System;
using System.Linq;

namespace Mastermind
{
    public class GuessResult : IEquatable<GuessResult>
    {
        public GuessResult(int colorMatch, int positionMatch)
        {
            ColorMatch = colorMatch;
            PositionMatch = positionMatch;
        }


        public int ColorMatch { get; }

        public int PositionMatch { get; }


        public bool Check(int size)
        {
            return PositionMatch == size;
        }


        public override string ToString()
        {
            var positions = Enumerable.Repeat('\u25CF', PositionMatch);
            var colors = Enumerable.Repeat('\u25CB', ColorMatch);
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
            return ColorMatch == other.ColorMatch
                && PositionMatch == other.PositionMatch;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ColorMatch, PositionMatch);
        }
    }
}
