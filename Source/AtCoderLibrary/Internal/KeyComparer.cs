using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AtCoder.Internal
{

    [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct KeyComparer<TKey, TValue, TKOp>
        : IComparer<KeyValuePair<TKey, TValue>>
        where TKOp : IComparer<TKey>
    {
        public KeyComparer(TKOp comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            this.comparer = comparer;
        }

        public readonly TKOp comparer;
        public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y) => comparer.Compare(x.Key, y.Key);
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
