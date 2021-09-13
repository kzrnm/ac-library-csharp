using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AtCoder.Internal
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct ComparableComparer
        <T> : IComparer<T>, IEquatable<ComparableComparer<T>>
        where T : IComparable<T>
    {
        private IComparer<T> Comparer { get; }
        public ComparableComparer(IComparer<T> comparer)
        {
            Comparer = comparer;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Compare(T x, T y) => Comparer?.Compare(x, y) ?? x.CompareTo(y);

        #region Equatable
        public override bool Equals(object obj) => obj is ComparableComparer<T> && Equals((ComparableComparer<T>)obj);
        public bool Equals(ComparableComparer<T> other)
        {
            if (Comparer == null)
                return other.Comparer == null;
            return Comparer.Equals(other.Comparer);
        }
        public override int GetHashCode() => Comparer?.GetHashCode() ?? 0;
        public static bool operator ==(ComparableComparer<T> left, ComparableComparer<T> right) => left.Equals(right);
        public static bool operator !=(ComparableComparer<T> left, ComparableComparer<T> right) => !left.Equals(right);
        #endregion Equatable
    }
}
