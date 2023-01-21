﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using AtCoder.Internal;

namespace AtCoder
{
    /// <summary>
    /// 有向グラフを強連結成分分解します。
    /// </summary>
    [DebuggerDisplay("Vertices = {_n}, Edges = {edges.Count}")]
    public class SccGraph
    {
        internal readonly int _n;
        private readonly SimpleList<(int from, Edge e)> edges;

        internal int VerticesNumbers => _n;

        /// <summary>
        /// <see cref="SccGraph"/> クラスの新しいインスタンスを、<paramref name="n"/> 頂点 0 辺の有向グラフとして初期化します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>≤10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        public SccGraph(int n)
        {
            _n = n;
            edges = new SimpleList<(int from, Edge e)>();
        }

        /// <summary>
        /// 頂点 <paramref name="from"/> から頂点 <paramref name="to"/> へ有向辺を追加します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="from"/>, <paramref name="to"/>&lt;n</para>
        /// <para>計算量: ならしO(1)</para>
        /// </remarks>
        [MethodImpl(256)]
        public void AddEdge(int from, int to)
        {
            Contract.Assert((uint)from < (uint)VerticesNumbers, reason: $"IndexOutOfRange: 0 <= {nameof(from)} && {nameof(from)} < _n");
            Contract.Assert((uint)to < (uint)VerticesNumbers, reason: $"IndexOutOfRange: 0 <= {nameof(to)} && {nameof(to)} < _n");
            edges.Add((from, new Edge(to)));
        }


        /// <summary>
        /// 強連結成分ごとに ID を割り振り、各頂点の所属する強連結成分の ID が記録された配列を取得します。
        /// </summary>
        /// <remarks>
        /// <para>強連結成分の ID はトポロジカルソートされています。異なる強連結成分の頂点 u, v について、u から v に到達できる時、u の ID は v の ID よりも小さくなります。</para>
        /// <para>計算量: 追加された辺の本数を m として O(n+m)</para>
        /// </remarks>
        [MethodImpl(256)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public (int groupNum, int[] ids) SccIDs()
        {
            // R. Tarjan のアルゴリズム
            var g = new Csr<Edge>(_n, edges);
            int nowOrd = 0;
            int groupNum = 0;
            var visited = new Stack<int>(_n);
            var low = new int[_n];
            var ord = Enumerable.Repeat(-1, _n).ToArray();
            var ids = new int[_n];

            for (int i = 0; i < ord.Length; i++)
            {
                if (ord[i] == -1)
                {
                    DFS(i);
                }
            }

            foreach (ref var x in ids.AsSpan())
            {
                // トポロジカル順序にするには逆順にする必要がある。
                x = groupNum - 1 - x;
            }

            return (groupNum, ids);

            //void DFS(int v)
            //{
            //    low[v] = nowOrd;
            //    ord[v] = nowOrd++;
            //    visited.Push(v);
            //    // 頂点 v から伸びる有向辺を探索する。
            //    for (int i = g.Start[v]; i < g.Start[v + 1]; i++)
            //    {
            //        int to = g.EList[i].To;
            //        if (ord[to] == -1)
            //        {
            //            DFS(to);
            //            low[v] = Math.Min(low[v], low[to]);
            //        }
            //        else
            //        {
            //            low[v] = Math.Min(low[v], ord[to]);
            //        }
            //    }
            //    // v がSCCの根である場合、強連結成分に ID を割り振る。
            //    if (low[v] == ord[v])
            //    {
            //        while (true)
            //        {
            //            int u = visited.Pop();
            //            ord[u] = _n;
            //            ids[u] = groupNum;
            //            if (u == v)
            //            {
            //                break;
            //            }
            //        }
            //        groupNum++;
            //    }
            //}
            void DFS(int v)
            {
                var stack = new Stack<(int v, int childIndex)>();
                stack.Push((v, g.Start[v]));
            DFS: while (stack.Count > 0)
                {
                    int ci;
                    (v, ci) = stack.Pop();

                    if (ci == g.Start[v])
                    {
                        low[v] = nowOrd;
                        ord[v] = nowOrd++;
                        visited.Push(v);
                    }
                    else if (ci < 0)
                    {
                        ci = -ci;
                        int to = g.EList[ci - 1].To;
                        low[v] = Math.Min(low[v], low[to]);
                    }

                    // 頂点 v から伸びる有向辺を探索する。
                    for (; ci < g.Start[v + 1]; ci++)
                    {
                        int to = g.EList[ci].To;
                        if (ord[to] == -1)
                        {
                            stack.Push((v, -(ci + 1)));
                            stack.Push((to, g.Start[to]));
                            goto DFS;
                        }
                        else
                        {
                            low[v] = Math.Min(low[v], ord[to]);
                        }
                    }

                    // v がSCCの根である場合、強連結成分に ID を割り振る。
                    if (low[v] == ord[v])
                    {
                        while (true)
                        {
                            int u = visited.Pop();
                            ord[u] = _n;
                            ids[u] = groupNum;

                            if (u == v)
                            {
                                break;
                            }
                        }

                        groupNum++;
                    }
                }
            }
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
        [MethodImpl(256)]
        public int[][] Scc()
        {
            var (groupNum, ids) = SccIDs();
            var groups = new int[groupNum][];
            var counts = new int[groupNum];
            var seen = new int[groupNum];

            foreach (var x in ids)
                counts[x]++;

            for (int i = 0; i < groupNum; i++)
                groups[i] = new int[counts[i]];

            for (int i = 0; i < ids.Length; i++)
                groups[ids[i]][seen[ids[i]]++] = i;

            return groups;
        }

        [DebuggerDisplay("To={" + nameof(To) + "}")]
        private readonly struct Edge
        {
            public int To { get; }

            public Edge(int to)
            {
                To = to;
            }
        }
    }
}

