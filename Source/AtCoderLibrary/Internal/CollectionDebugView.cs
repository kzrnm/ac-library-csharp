using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AtCoder.Internal
{
    internal class CollectionDebugView<T>
    {
        private readonly IEnumerable<T> collection;
        public CollectionDebugView(IEnumerable<T> collection)
        {
            this.collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items => collection.ToArray();
    }
}
