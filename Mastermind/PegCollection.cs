using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mastermind
{
    public abstract class PegCollection : IPegCollection
    {
        protected readonly Dictionary<int, int> _pegCounts;
        protected readonly int _hashCode;

        protected PegCollection(Palette palette, int[] pegs)
        {
            if (palette == null)
            {
                throw new ArgumentNullException(nameof(palette));
            }

            if (pegs == null)
            {
                throw new ArgumentNullException(nameof(pegs));
            }

            if (!palette.ContainsAll(pegs))
            {
                throw new ArgumentException("Peg collection contains invalid peg.", nameof(pegs));
            }

            Pegs = pegs;

            Palette = palette;
            Size = Pegs.Length;

            _pegCounts = Palette.CountPegs(Pegs);
            _hashCode = HashCode.Combine(Palette, Size, Pegs.GetListHashCode());
        }


        public Palette Palette { get; }

        public int Size { get; }

        protected int[] Pegs { get; }

        int ICollection<int>.Count => Size;

        bool ICollection<int>.IsReadOnly => true;


        public bool Contains(int peg)
        {
            return Pegs.Contains(peg);
        }

        public void CopyTo(int[] array, int index)
        {
            Pegs.CopyTo(array, index);
        }

        void ICollection<int>.Add(int item)
        {
            throw new NotSupportedException();
        }

        bool ICollection<int>.Remove(int item)
        {
            throw new NotSupportedException();
        }

        void ICollection<int>.Clear()
        {
            throw new NotSupportedException();
        }


        public override string ToString()
        {
            var pegMaxWidth = Palette.PegCount.GetWidth();
            var collection = string.Join(' ', Pegs.Select(x => x.ToString().PadLeft(pegMaxWidth)));
            return $"[{collection}]";
        }


        public IEnumerator<int> GetEnumerator()
        {
            return ((IEnumerable<int>)Pegs).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
