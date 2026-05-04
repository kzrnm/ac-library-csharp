using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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
    public class Csr<TEdge> : IEnumerable<(int from, TEdge edge)>
    {
        /// <summary>
        /// 各頂点から伸びる有向辺数の累積和を取得します。
        /// </summary>
        public readonly int[] Start;

        /// <summary>
        /// 有向辺の終点の配列を取得します。
        /// </summary>
        public readonly TEdge[] EList;

        public Csr(int n, ReadOnlySpan<(int from, TEdge e)> edges)
        {
            // 本家 C++ 版 ACL を参考に実装。通常の隣接リストと比較して高速か否かは未検証。
            var counter = new int[n + 1];
            EList = new TEdge[edges.Length];

            foreach (var (from, _) in edges)
            {
                counter[from + 1]++;
            }

            for (int i = 1; i <= n; i++)
            {
                counter[i] += counter[i - 1];
            }

            Start = (int[])counter.Clone();
            foreach (var (from, e) in edges)
            {
                EList[counter[from]++] = e;
            }
        }
        public Enumerator GetEnumerator() => new(this);
        IEnumerator<(int from, TEdge edge)> IEnumerable<(int from, TEdge edge)>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public struct Enumerator(Csr<TEdge> g) : IEnumerator<(int from, TEdge edge)>
        {
            private int index = -1;
            private int start = 0;
            public (int from, TEdge edge) Current => (start, g.EList[index]);
            object IEnumerator.Current => Current;

            [MethodImpl(256)]
            public bool MoveNext()
            {
                if (++index < g.Start[start + 1])
                    return true;
                return MoveNextStart();
            }
            private bool MoveNextStart()
            {
                for (++start; start + 1 < g.Start.Length; ++start)
                    if (index < g.Start[start + 1])
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
