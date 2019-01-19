using System;
using System.Collections.Generic;
using System.Linq;

namespace Mastermind
{
    public class PegSet : PegCollection, IEquatable<PegSet>
    {
        public PegSet(Palette palette, IEnumerable<int> pegs) : base(palette, pegs?.OrderBy(x => x).ToArray())
        {
        }


        public bool IsProperSubsetOf(PegSet other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (other == this)
            {
                return false;
            }

            if (Palette != other.Palette)
            {
                return false;
            }

            if (Size == 0)
            {
                return other.Size > 0;
            }

            if (Size >= other.Size)
            {
                return false;
            }

            return Pegs.All(peg => _pegCounts[peg] <= other._pegCounts[peg]);
        }

        public bool IsProperSupersetOf(PegSet other)
        {
            return other.IsProperSubsetOf(this);
        }


        public PegSet Except(PegSet other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (Size == 0 || other == this)
            {
                return new PegSet(Palette, Enumerable.Empty<int>());
            }

            if (other.Size == 0)
            {
                return new PegSet(Palette, Enumerable.Empty<int>());
            }

            var pegs = new List<int>();
            foreach (var (peg, count) in _pegCounts)
            {
                for (int c = count - other._pegCounts[peg]; c > 0; c--)
                {
                    pegs.Add(peg);
                }
            }
            return new PegSet(Palette, pegs);
        }


        public bool Equals(PegSet other)
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
            return obj is PegSet other && EqualsCore(other);
        }

        public static bool operator ==(PegSet left, PegSet right)
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

        public static bool operator !=(PegSet left, PegSet right)
        {
            return !(left == right);
        }

        protected virtual bool EqualsCore(PegSet other)
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
