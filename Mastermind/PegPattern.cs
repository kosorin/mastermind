using System;
using System.Collections.Generic;
using System.Linq;

namespace Mastermind
{
    public class PegPattern : PegCollection, IEquatable<PegPattern>
    {
        public PegPattern(Palette palette, IList<int> pegs) : base(palette, pegs?.ToArray())
        {
            if (Size <= 0)
            {
                throw new ArgumentException("Pattern size must be greater than 0.");
            }
        }


        public GuessResult CompareTo(PegPattern other)
        {
            if (Palette != other.Palette || Size != other.Size)
            {
                throw new InvalidOperationException("Patterns must have same palette and size.");
            }

            var positionMatch = Pegs
                .Zip(other.Pegs, (a, b) => a == b)
                .Count(x => x);

            var colorMatch = Pegs
                .Intersect(other.Pegs)
                .Sum(x => Math.Min(_pegCounts[x], other._pegCounts[x]));

            colorMatch -= positionMatch;

            return new GuessResult(colorMatch, positionMatch);
        }


        public PegSet ToSet()
        {
            return new PegSet(Palette, Pegs);
        }


        public bool Equals(PegPattern other)
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
            return obj is PegPattern other && EqualsCore(other);
        }

        public static bool operator ==(PegPattern left, PegPattern right)
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

        public static bool operator !=(PegPattern left, PegPattern right)
        {
            return !(left == right);
        }

        protected virtual bool EqualsCore(PegPattern other)
        {
            return Palette == other.Palette
                && Size == other.Size
                && Pegs.SequenceEqual(other.Pegs);
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }
    }
}
