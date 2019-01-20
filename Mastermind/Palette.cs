using System;
using System.Collections.Generic;
using System.Linq;

namespace Mastermind
{
    public struct Palette : IEquatable<Palette>
    {
        public Palette(int pegCount)
        {
            if (pegCount <= Peg.Min)
            {
                throw new ArgumentException("Invalid palette.", nameof(pegCount));
            }
            PegCount = pegCount;
        }

        public int PegCount { get; }


        public bool Contains(int peg)
        {
            return Peg.Min <= peg && peg < PegCount;
        }

        public bool ContainsAll(IEnumerable<int> pegs)
        {
            return pegs.All(Contains);
        }


        public Dictionary<int, int> CountPegs(IEnumerable<int> pegs)
        {
            var pegCounts = GetPegs().ToDictionary(x => x, _ => 0);
            foreach (var peg in pegs)
            {
                pegCounts[peg]++;
            }
            return pegCounts;
        }

        public IList<int> GetPegs()
        {
            return Enumerable.Range(Peg.Min, PegCount).ToList();
        }

        public LinkedList<PegPattern> GetAllPatterns(int size, bool allowDuplicates)
        {
            if (size < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            var palette = new Palette(PegCount);
            var paletteSet = ToSet();
            var patternQuery = paletteSet.Select(x => new[] { x });
            for (int i = 1; i < size; i++)
            {
                patternQuery = patternQuery.SelectMany(_ => paletteSet, (pq, p) => pq.Append(p).ToArray());
            }
            return new LinkedList<PegPattern>(patternQuery
                .Where(x => allowDuplicates || x.Length == x.Distinct().Count())
                .Select(x => new PegPattern(palette, x))
                .ToList());
        }

        public PegPattern GetRandomPattern(int size, bool allowDuplicates)
        {
            var sourcePegs = GetPegs();
            var patternPegs = new List<int>();
            var random = new Random();
            for (int i = 0; i < size; i++)
            {
                var index = random.Next(sourcePegs.Count);
                var peg = sourcePegs[index];
                if (!allowDuplicates)
                {
                    sourcePegs.RemoveAt(index);
                }
                patternPegs.Add(peg);
            }
            return new PegPattern(this, patternPegs);
        }


        public PegSet ToSet()
        {
            return new PegSet(this, GetPegs());
        }

        public override string ToString()
        {
            return PegCount.ToString();
        }


        public bool Equals(Palette other)
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
            return obj is Palette other && EqualsCore(other);
        }

        public static bool operator ==(Palette left, Palette right)
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

        public static bool operator !=(Palette left, Palette right)
        {
            return !(left == right);
        }

        private bool EqualsCore(Palette other)
        {
            return PegCount == other.PegCount;
        }

        public override int GetHashCode()
        {
            return PegCount.GetHashCode();
        }
    }
}
