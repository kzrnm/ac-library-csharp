using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace AtCoder
{
    /// <summary>
    /// 有向グラフを強連結成分分解します。
    /// </summary>
    [DebuggerDisplay("Vertices = {_n}, Edges = {edges.Count}")]
    public class SCCGraph
    {
        private readonly int _n;
        private readonly List<Edge> edges;

        /// <summary>
        /// <see cref="SCCGraph"/> クラスの新しいインスタンスを、<paramref name="n"/> 頂点 0 辺の有向グラフとして初期化します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>≤10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        public SCCGraph(int n) 
        {
            Debug.Assert(unchecked((uint)n <= 100_000_000));
            _n = n;
            edges = new List<Edge>();
        }

        /// <summary>
        /// 頂点 <paramref name="from"/> から頂点 <paramref name="to"/> へ有向辺を追加します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="from"/>, <paramref name="to"/>&lt;n</para>
        /// <para>計算量: ならしO(1)</para>
        /// </remarks>
        public void AddEdge(int from, int to)
        {
            Debug.Assert(unchecked((uint)from < _n));
            Debug.Assert(unchecked((uint)to < _n));
            edges.Add(new Edge(from, to));
        }

        /// <summary>
        /// 強連結成分分解の結果である「頂点のリスト」のリストを取得します。
        /// </summary>
        /// <remarks>
        /// <para>- 全ての頂点がちょうど1つずつ、どれかのリストに含まれます。</para>
        /// <para>- 内側のリストと強連結成分が一対一に対応します。リスト内での頂点の順序は未定義です。</para>
        /// <para>- リストはトポロジカルソートされています。異なる強連結成分の頂点 u, v について、u から v に到達できる時、u の属するリストは v の属するリストよりも前です。</para>
        /// <para>計算量: 追加された辺の本数を m として O(n+m)</para>
        /// </remarks>
        public List<List<int>> SCC() 
        {
            // R. Tarjan のアルゴリズム
            var g = new CSR(_n, edges);
            int nowOrd = 0;
            var visited = new Stack<int>(_n);
            var low = new int[_n];
            var ord = Enumerable.Repeat(-1, _n).ToArray();

            // 強連結成分分解結果
            var sccs = new Stack<List<int>>();

            for (int i = 0; i < ord.Length; i++)
            {
                if (ord[i] == -1)
                {
                    DFS(i);
                }
            }

            // トポロジカル順序にするには逆順にする必要がある。
            return sccs.ToList();

            void DFS(int v)
            {
                low[v] = nowOrd;
                ord[v] = nowOrd++;
                visited.Push(v);
                
                // 頂点 v から伸びる有向辺を探索する。
                for (int i = g.Start[v]; i < g.Start[v + 1]; i++)
                {
                    int to = g.EList[i];
                    if (ord[to] == -1)
                    {
                        DFS(to);
                        low[v] = System.Math.Min(low[v], low[to]);
                    }
                    else
                    {
                        low[v] = System.Math.Min(low[v], ord[to]);
                    }
                }

                // v がSCCの根である場合、強連結成分を List に詰める。
                if (low[v] == ord[v])
                {
                    var scc = new List<int>();

                    while (true)
                    {
                        int u = visited.Pop();
                        ord[u] = _n;
                        scc.Add(u);

                        if (u == v)
                        {
                            break;
                        }
                    }

                    sccs.Push(scc);
                }
            }
        }

        /// <summary>
        /// 有向グラフの辺集合を表します。
        /// </summary>
        /// <example>
        /// <code>
        /// for (int i = graph.Starts[v]; i < graph.Starts[v + 1]; i++)
        /// {
        ///     int to = graph.Edges[i];
        /// }
        /// </code>
        /// </example>
        private class CSR
        {
            /// <summary>
            /// 各頂点から伸びる有向辺数の累積和を取得します。
            /// </summary>
            public int[] Start { get; }

            /// <summary>
            /// 有向辺の終点の配列を取得します。
            /// </summary>
            public int[] EList { get; }

            public CSR(int n, List<Edge> edges)
            {
                // 本家 C++ 版 ACL を参考に実装。通常の隣接リストと比較して高速か否かは未検証。
                Start = new int[n + 1];
                EList = new int[edges.Count];

                foreach (var e in edges)
                {
                    Start[e.From + 1]++;
                }

                for (int i = 1; i <= n; i++)
                {
                    Start[i] += Start[i - 1];
                }

                var counter = new int[Start.Length];
                Start.CopyTo(counter, 0);
                foreach (var e in edges)
                {
                    EList[counter[e.From]++] = e.To;
                }
            }
        }

        [DebuggerDisplay("From:{From}, To:{To}")]
        private readonly struct Edge
        {
            public int From { get; }
            public int To { get; }

            public Edge(int from, int to)
            {
                From = from;
                To = to;
            }
        }
    }
}
