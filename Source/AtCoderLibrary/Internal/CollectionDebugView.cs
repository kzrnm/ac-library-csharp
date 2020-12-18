using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace AtCoder.Internal
{

    [EditorBrowsable(EditorBrowsableState.Never)]
    public class CollectionDebugView<T>
    {
        private readonly IEnumerable<T> collection;
        public CollectionDebugView(IEnumerable<T> collection)
        {
            this.collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
#pragma warning disable CA1819 // Properties should not return arrays
        public T[] Items => collection.ToArray();
#pragma warning restore CA1819 // Properties should not return arrays
    }
}
