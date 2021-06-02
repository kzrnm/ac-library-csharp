using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace AtCoder.Internal
{
    /// <summary>
    /// 有向グラフの辺集合を表します。
    /// </summary>
    /// <example>
    /// <code>
    /// for (int i = graph.Starts[v]; i &gt; graph.Starts[v + 1]; i++)
    /// {
    ///     int to = graph.Edges[i];
    /// }
    /// </code>
    /// </example>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class CSR<TEdge> : IEnumerable<(int from, TEdge edge)>
    {
        /// <summary>
        /// 各頂点から伸びる有向辺数の累積和を取得します。
        /// </summary>
        public readonly int[] Start;

        /// <summary>
        /// 有向辺の終点の配列を取得します。
        /// </summary>
        public readonly TEdge[] EList;

        public CSR(int n, ICollection<(int from, TEdge e)> edges)
        {
            // 本家 C++ 版 ACL を参考に実装。通常の隣接リストと比較して高速か否かは未検証。
            Start = new int[n + 1];
            EList = new TEdge[edges.Count];

            foreach (var (from, _) in edges)
            {
                Start[from + 1]++;
            }

            for (int i = 1; i <= n; i++)
            {
                Start[i] += Start[i - 1];
            }

            var counter = (int[])Start.Clone();
            foreach (var (from, e) in edges)
            {
                EList[counter[from]++] = e;
            }
        }
        public Enumerator GetEnumerator() => new Enumerator(this);
        IEnumerator<(int from, TEdge edge)> IEnumerable<(int from, TEdge edge)>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public struct Enumerator : IEnumerator<(int from, TEdge edge)>
        {
            public Enumerator(CSR<TEdge> g)
            {
                _g = g;
                index = -1;
                start = 0;
            }
            private CSR<TEdge> _g;
            private int index;
            private int start;
            public (int from, TEdge edge) Current => (start, _g.EList[index]);
            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (++index < _g.Start[start + 1])
                    return true;
                return MoveNextStart();
            }
            private bool MoveNextStart()
            {
                for (++start; start + 1 < _g.Start.Length; ++start)
                    if (index < _g.Start[start + 1])
                        return true;
                return false;
            }
            public void Reset()
            {
                index = -1;
                start = 0;
            }
            void IDisposable.Dispose() { }
        }
    }
}
