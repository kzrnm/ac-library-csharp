﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using AtCoder.Internal;

namespace AtCoder
{
    /// <summary>
    /// Minimum-cost flow problem を扱うライブラリです。
    /// </summary>
    /// <typeparam name="TValue">容量とコストの型</typeparam>
    public class McfGraph<TValue> : McfGraph<TValue, TValue>
            where TValue : INumber<TValue>, ISignedNumber<TValue>, IMinMaxValue<TValue>
    {
        /// <summary>
        /// <paramref name="n"/> 頂点 0 辺のグラフを作ります。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0 ≤ <paramref name="n"/> ≤ 10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        public McfGraph(int n) : base(n) { }
    }
    /// <summary>
    /// Minimum-cost flow problem を扱うライブラリです。
    /// </summary>
    /// <typeparam name="TCap">容量の型</typeparam>
    /// <typeparam name="TCost">コストの型</typeparam>
    public class McfGraph<TCap, TCost>
            where TCap : INumber<TCap>, IMinMaxValue<TCap>
            where TCost : INumber<TCost>, ISignedNumber<TCost>, IMinMaxValue<TCost>
    {
        /// <summary>
        /// <paramref name="n"/> 頂点 0 辺のグラフを作ります。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0 ≤ <paramref name="n"/> ≤ 10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        public McfGraph(int n)
        {
            _n = n;
        }

        /// <summary>
        /// <paramref name="from"/> から <paramref name="to"/> へ
        /// 最大容量 <paramref name="cap"/>、コスト <paramref name="cost"/> の辺を追加し、
        /// 何番目に追加された辺かを返します。
        /// </summary>
        /// <remarks>
        /// 制約:
        /// <list type="bullet">
        /// <item>
        /// <description>0 ≤ <paramref name="from"/>, <paramref name="to"/> &lt; n</description>
        /// </item>
        /// <item>
        /// <description>0 ≤ <paramref name="cap"/>, <paramref name="cost"/></description>
        /// </item>
        /// </list>
        /// <para>計算量: ならしO(1)</para>
        /// </remarks>
        [MethodImpl(256)]
        public int AddEdge(int from, int to, TCap cap, TCost cost)
        {
            Contract.Assert((uint)from < (uint)_n, reason: $"IndexOutOfRange: 0 <= {nameof(from)} && {nameof(from)} < _n");
            Contract.Assert((uint)to < (uint)_n, reason: $"IndexOutOfRange: 0 <= {nameof(to)} && {nameof(to)} < _n");
            Contract.Assert(TCap.IsPositive(cap), reason: $"IndexOutOfRange: 0 <= {nameof(cap)}");
            Contract.Assert(TCost.IsPositive(cost), reason: $"IndexOutOfRange: 0 <= {nameof(cost)}");

            int m = _edges.Count;
            _edges.Add(new Edge(from, to, cap, default, cost));
            return m;
        }

        /// <summary>
        /// 今の内部の辺の状態を返します。
        /// </summary>
        /// <remarks>
        /// <para>AddEdge で <paramref name="i"/> 番目 (0-indexed) に追加された辺を返す。</para>
        /// <para>制約: m を追加された辺数として 0 ≤ i &lt; m</para>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        [MethodImpl(256)]
        public Edge GetEdge(int i)
        {
            Contract.Assert((uint)i < (uint)_edges.Count, reason: $"IndexOutOfRange: 0 <= {nameof(i)} && {nameof(i)} < edgeCount");
            return _edges[i];
        }

        /// <summary>
        /// 今の内部の辺の状態を返します。
        /// </summary>
        /// <remarks>
        /// <para>辺の順番はadd_edgeで追加された順番と同一。</para>
        /// <para>計算量: m を追加された辺数として O(m)</para>
        /// </remarks>
        [MethodImpl(256)] public Span<Edge> Edges() => _edges.AsSpan();

        /// <summary>
        /// 頂点 <paramref name="s"/> から <paramref name="t"/> へ流せる限り流し、
        /// その流量とコストを返します。
        /// </summary>
        /// <remarks>
        /// 制約: 辺のコストの最大を x として
        /// <list type="bullet">
        /// <item>
        /// <description><paramref name="s"/> ≠ <paramref name="t"/></description>
        /// </item>
        /// <item>
        /// <description>Flow や Slope 関数を合わせて複数回呼んだときの挙動は未定義。</description>
        /// </item>
        /// <item>
        /// <description>
        /// <paramref name="s"/> から <paramref name="t"/> へ流したフローの
        /// 流量が cap に収まる。
        /// </description>
        /// </item>
        /// <item>
        /// <description>流したコストの総和が cost に収まる。</description>
        /// </item>
        /// <item>
        /// <description>(Cost: int) 0 ≤ nx ≤ 2 * 10^9 + 1000 </description>
        /// </item>
        /// <item>
        /// <description>(Cost: long) 0 ≤ nx ≤ 8 * 10^18 + 1000 </description>
        /// </item>
        /// </list>
        /// 計算量: F を流量、m を追加した辺の本数として
        /// O(F(n + m) log (n + m))
        /// </remarks>
        [MethodImpl(256)]
        public (TCap cap, TCost cost) Flow(int s, int t) => Flow(s, t, TCap.MaxValue);

        /// <summary>
        /// 頂点 <paramref name="s"/> から <paramref name="t"/> へ
        /// 流量 <paramref name="flowLimit"/> に達するまで流せる限り流し、
        /// その流量とコストを返します。
        /// </summary>
        /// <remarks>
        /// 制約: 辺のコストの最大を x として
        /// <list type="bullet">
        /// <item>
        /// <description><paramref name="s"/> ≠ <paramref name="t"/></description>
        /// </item>
        /// <item>
        /// <description>Flow や Slope 関数を合わせて複数回呼んだときの挙動は未定義。</description>
        /// </item>
        /// <item>
        /// <description>
        /// <paramref name="s"/> から <paramref name="t"/> へ流したフローの
        /// 流量が cap に収まる。
        /// </description>
        /// </item>
        /// <item>
        /// <description>流したコストの総和が cost に収まる。</description>
        /// </item>
        /// <item>
        /// <description>(Cost: int) 0 ≤ nx ≤ 2 * 10^9 + 1000 </description>
        /// </item>
        /// <item>
        /// <description>(Cost: long) 0 ≤ nx ≤ 8 * 10^18 + 1000 </description>
        /// </item>
        /// </list>
        /// 計算量: F を流量、m を追加した辺の本数として
        /// O(F(n + m) log (n + m))
        /// </remarks>
        [MethodImpl(256)]
        public (TCap cap, TCost cost) Flow(int s, int t, TCap flowLimit)
        {
            var slope = SlopeImpl(s, t, flowLimit, false);
            return slope[slope.Count - 1];
        }

        /// <summary>
        /// 返り値に流量とコストの関係の折れ線が入ります。
        /// 全ての x について、流量 x の時の最小コストを g(x) とすると、
        /// (x, g(x)) は返り値を折れ線として見たものに含まれます。
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// <description>返り値の最初の要素は (0, 0)。</description>
        /// </item>
        /// <item>
        /// <description>返り値の .first、.second は共に狭義単調増加。</description>
        /// </item>
        /// <item>
        /// <description>3点が同一線上にあることは無い。</description>
        /// </item>
        /// <item>
        /// <description>返り値の最後の要素は最大流量 x として (x, g(x))。</description>
        /// </item>
        /// </list>
        /// 制約: 辺のコストの最大を x として
        /// <list type="bullet">
        /// <item>
        /// <description><paramref name="s"/> ≠ <paramref name="t"/></description>
        /// </item>
        /// <item>
        /// <description>Flow や Slope 関数を合わせて複数回呼んだときの挙動は未定義。</description>
        /// </item>
        /// <item>
        /// <description>
        /// <paramref name="s"/> から <paramref name="t"/> へ流したフローの
        /// 流量が cap に収まる。
        /// </description>
        /// </item>
        /// <item>
        /// <description>流したコストの総和が cost に収まる。</description>
        /// </item>
        /// <item>
        /// <description>(Cost: int) 0 ≤ nx ≤ 2 * 10^9 + 1000 </description>
        /// </item>
        /// <item>
        /// <description>(Cost: long) 0 ≤ nx ≤ 8 * 10^18 + 1000 </description>
        /// </item>
        /// </list>
        /// 計算量: F を流量、m を追加した辺の本数として
        /// O(F(n + m) log (n + m))
        /// </remarks>
        [MethodImpl(256)]
        public List<(TCap cap, TCost cost)> Slope(int s, int t) => Slope(s, t, TCap.MaxValue);


        /// <summary>
        /// 返り値に流量とコストの関係の折れ線が入ります。
        /// 全ての x について、流量 x の時の最小コストを g(x) とすると、
        /// (x, g(x)) は返り値を折れ線として見たものに含まれます。
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// <description>返り値の最初の要素は (0, 0)。</description>
        /// </item>
        /// <item>
        /// <description>返り値の .first、.second は共に狭義単調増加。</description>
        /// </item>
        /// <item>
        /// <description>3点が同一線上にあることは無い。</description>
        /// </item>
        /// <item>
        /// <description>
        /// 返り値の最後の要素は
        /// y = min(x, <paramref name="flowLimit"/>) として (y, g(y))。</description>
        /// </item>
        /// </list>
        /// 制約: 辺のコストの最大を x として
        /// <list type="bullet">
        /// <item>
        /// <description><paramref name="s"/> ≠ <paramref name="t"/></description>
        /// </item>
        /// <item>
        /// <description>Flow や Slope 関数を合わせて複数回呼んだときの挙動は未定義。</description>
        /// </item>
        /// <item>
        /// <description>
        /// <paramref name="s"/> から <paramref name="t"/> へ流したフローの
        /// 流量が cap に収まる。
        /// </description>
        /// </item>
        /// <item>
        /// <description>流したコストの総和が cost に収まる。</description>
        /// </item>
        /// <item>
        /// <description>(Cost: int) 0 ≤ nx ≤ 2 * 10^9 + 1000 </description>
        /// </item>
        /// <item>
        /// <description>(Cost: long) 0 ≤ nx ≤ 8 * 10^18 + 1000 </description>
        /// </item>
        /// </list>
        /// 計算量: F を流量、m を追加した辺の本数として
        /// O(F(n + m) log (n + m))
        /// </remarks>
        public List<(TCap cap, TCost cost)> Slope(int s, int t, TCap flowLimit) => SlopeImpl(s, t, flowLimit, true);

        /// <summary>
        /// 返り値に流量とコストの関係の折れ線が入ります。
        /// 全ての x について、流量 x の時の最小コストを g(x) とすると、
        /// (x, g(x)) は返り値を折れ線として見たものに含まれます。
        /// Slope と異なり 3 点が同一線上にある場合も削除しません。
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// <description>返り値の最初の要素は (0, 0)。</description>
        /// </item>
        /// <item>
        /// <description>返り値の .first、.second は共に狭義単調増加。</description>
        /// </item>
        /// <item>
        /// <description>3点が同一線上にある場合もある。</description>
        /// </item>
        /// <item>
        /// <description>返り値の最後の要素は最大流量 x として (x, g(x))。</description>
        /// </item>
        /// </list>
        /// 制約: 辺のコストの最大を x として
        /// <list type="bullet">
        /// <item>
        /// <description><paramref name="s"/> ≠ <paramref name="t"/></description>
        /// </item>
        /// <item>
        /// <description>Flow や Slope 関数を合わせて複数回呼んだときの挙動は未定義。</description>
        /// </item>
        /// <item>
        /// <description>
        /// <paramref name="s"/> から <paramref name="t"/> へ流したフローの
        /// 流量が cap に収まる。
        /// </description>
        /// </item>
        /// <item>
        /// <description>流したコストの総和が cost に収まる。</description>
        /// </item>
        /// <item>
        /// <description>(Cost: int) 0 ≤ nx ≤ 2 * 10^9 + 1000 </description>
        /// </item>
        /// <item>
        /// <description>(Cost: long) 0 ≤ nx ≤ 8 * 10^18 + 1000 </description>
        /// </item>
        /// </list>
        /// 計算量: F を流量、m を追加した辺の本数として
        /// O(F(n + m) log (n + m))
        /// </remarks>
        [MethodImpl(256)]
        public List<(TCap cap, TCost cost)> Slope2(int s, int t) => Slope2(s, t, TCap.MaxValue);


        /// <summary>
        /// 返り値に流量とコストの関係の折れ線が入ります。
        /// 全ての x について、流量 x の時の最小コストを g(x) とすると、
        /// (x, g(x)) は返り値を折れ線として見たものに含まれます。
        /// Slope と異なり 3 点が同一線上にある場合も削除しません。
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// <description>返り値の最初の要素は (0, 0)。</description>
        /// </item>
        /// <item>
        /// <description>返り値の .first、.second は共に狭義単調増加。</description>
        /// </item>
        /// <item>
        /// <description>3点が同一線上にある場合もある。</description>
        /// </item>
        /// <item>
        /// <description>
        /// 返り値の最後の要素は
        /// y = min(x, <paramref name="flowLimit"/>) として (y, g(y))。</description>
        /// </item>
        /// </list>
        /// 制約: 辺のコストの最大を x として
        /// <list type="bullet">
        /// <item>
        /// <description><paramref name="s"/> ≠ <paramref name="t"/></description>
        /// </item>
        /// <item>
        /// <description>Flow や Slope 関数を合わせて複数回呼んだときの挙動は未定義。</description>
        /// </item>
        /// <item>
        /// <description>
        /// <paramref name="s"/> から <paramref name="t"/> へ流したフローの
        /// 流量が cap に収まる。
        /// </description>
        /// </item>
        /// <item>
        /// <description>流したコストの総和が cost に収まる。</description>
        /// </item>
        /// <item>
        /// <description>(Cost: int) 0 ≤ nx ≤ 2 * 10^9 + 1000 </description>
        /// </item>
        /// <item>
        /// <description>(Cost: long) 0 ≤ nx ≤ 8 * 10^18 + 1000 </description>
        /// </item>
        /// </list>
        /// 計算量: F を流量、m を追加した辺の本数として
        /// O(F(n + m) log (n + m))
        /// </remarks>
        public List<(TCap cap, TCost cost)> Slope2(int s, int t, TCap flowLimit) => SlopeImpl(s, t, flowLimit, false);

        struct SlopeDualRef
        {
            public TCost[] dual, dist;
            public CSR<EdgeInternal> g;
            public int[] prevE;
            public int s, t, _n;

            [MethodImpl(256)]
            public bool DualRef()
            {
                dist.AsSpan().Fill(TCost.MaxValue);
                var vis = new bool[_n];

                var queMin = new Stack<int>();
                var que = new PriorityQueueDictionary<TCost, int>();

                dist[s] = default;
                queMin.Push(s);
                while (queMin.Count > 0 || que.Count > 0)
                {
                    int v;
                    if (queMin.Count > 0)
                        v = queMin.Pop();
                    else
                        v = que.Dequeue().Value;
                    if (vis[v]) continue;
                    vis[v] = true;
                    if (v == t) break;
                    // dist[v] = shortest(s, v) + dual[s] - dual[v]
                    // dist[v] >= 0 (all reduced cost are positive)
                    // dist[v] <= (n-1)C
                    var dualV = dual[v];
                    var distV = dist[v];

                    var gStartCur = g.Start[v];
                    var gStartNext = g.Start[v + 1];
                    for (int i = gStartCur; i < gStartNext; i++)
                    {
                        var e = g.EList[i];
                        if (EqualityComparer<TCap>.Default.Equals(e.Cap, default)) continue;
                        // |-dual[e.To] + dual[v]| <= (n-1)C
                        // cost <= C - -(n-1)C + 0 = nC
                        var cost = e.Cost - dual[e.To] + dualV;
                        if (dist[e.To] - distV > cost)
                        {
                            var distTo = distV + cost;
                            dist[e.To] = distTo;
                            prevE[e.To] = e.Rev;
                            if (EqualityComparer<TCost>.Default.Equals(distTo, distV))
                                queMin.Push(e.To);
                            else
                                que.Enqueue(distTo, e.To);
                        }
                    }
                }
                if (!vis[t])
                {
                    return false;
                }

                for (int v = 0; v < _n; v++)
                {
                    if (!vis[v]) continue;
                    // dual[v] = dual[v] - dist[t] + dist[v]
                    //         = dual[v] - (shortest(s, t) + dual[s] - dual[t]) +
                    //         (shortest(s, v) + dual[s] - dual[v]) = - shortest(s,
                    //         t) + dual[t] + shortest(s, v) = shortest(s, v) -
                    //         shortest(s, t) >= 0 - (n-1)C
                    dual[v] -= (dist[t] - dist[v]);
                }
                return true;
            }
        }


        private List<(TCap cap, TCost cost)> SlopeImpl(int s, int t, TCap flowLimit, bool removeLine)
        {
            Contract.Assert((uint)s < (uint)_n, reason: $"IndexOutOfRange: 0 <= {nameof(s)} && {nameof(s)} < _n");
            Contract.Assert((uint)t < (uint)_n, reason: $"IndexOutOfRange: 0 <= {nameof(t)} && {nameof(t)} < _n");
            Contract.Assert(s != t, reason: $"{nameof(s)} and {nameof(t)} must be different.");

            int m = _edges.Count;
            var edgeIdx = new int[m];

            CSR<EdgeInternal> g;
            {
                var degree = new int[_n];
                var redgeIdx = new int[m];
                var elist = new SimpleList<(int from, EdgeInternal edge)>(2 * m);

                for (int i = 0; i < m; i++)
                {
                    var e = _edges[i];
                    edgeIdx[i] = degree[e.From]++;
                    redgeIdx[i] = degree[e.To]++;
                    elist.Add((e.From, new EdgeInternal(e.To, -1, e.Cap - e.Flow, e.Cost)));
                    elist.Add((e.To, new EdgeInternal(e.From, -1, e.Flow, -e.Cost)));
                }
                g = new CSR<EdgeInternal>(_n, elist);
                for (int i = 0; i < m; i++)
                {
                    var e = _edges[i];
                    edgeIdx[i] += g.Start[e.From];
                    redgeIdx[i] += g.Start[e.To];
                    g.EList[edgeIdx[i]].Rev = redgeIdx[i];
                    g.EList[redgeIdx[i]].Rev = edgeIdx[i];
                }
            }

            var result = SlopeImpl(g, s, t, flowLimit, removeLine);

            for (int i = 0; i < m; i++)
            {
                var e = g.EList[edgeIdx[i]];
                _edges[i].Flow = _edges[i].Cap - e.Cap;
            }

            return result;
        }

        /// <summary>
        /// Slope の内部実装
        /// </summary>
        /// <param name="g">辺のリスト</param>
        /// <param name="s">開始ノード</param>
        /// <param name="t">終了ノード</param>
        /// <param name="flowLimit">最大のフロー</param>
        /// <param name="removeLine">直線になるときには間を削除する</param>
        /// <returns></returns>
        private List<(TCap cap, TCost cost)> SlopeImpl(CSR<EdgeInternal> g, int s, int t, TCap flowLimit, bool removeLine)
        {
            // variants (C = maxcost):
            // -(n-1)C <= dual[s] <= dual[i] <= dual[t] = 0
            // reduced cost (= e.cost + dual[e.from] - dual[e.To]) >= 0 for all edge
            var dual = new TCost[_n];
            var dist = new TCost[_n];
            var prevE = new int[_n];

            var slopeDR = new SlopeDualRef
            {
                dual = dual,
                dist = dist,
                g = g,
                prevE = prevE,
                s = s,
                t = t,
                _n = _n,
            };

            TCap flow = default;
            TCost cost = default;
            TCost prevCostPerFlow = TCost.NegativeOne;
            var result = new List<(TCap cap, TCost cost)> { (flow, cost) };
            while (flow < flowLimit)
            {
                if (!slopeDR.DualRef()) break;
                var c = flowLimit - flow;
                for (int v = t; v != s; v = g.EList[prevE[v]].To)
                {
                    c = TCap.Min(c, g.EList[g.EList[prevE[v]].Rev].Cap);
                }
                for (int v = t; v != s; v = g.EList[prevE[v]].To)
                {
                    var e = g.EList[prevE[v]];
                    g.EList[prevE[v]].Cap = e.Cap + c;
                    g.EList[e.Rev].Cap -= c;
                }
                var d = -dual[s];
                flow += c;
                cost += TCost.CreateChecked(c) * d;
                if (removeLine && EqualityComparer<TCost>.Default.Equals(prevCostPerFlow, d))
                {
                    result.RemoveAt(result.Count - 1);
                }
                result.Add((flow, cost));
                prevCostPerFlow = d;
            }
            return result;
        }

        /// <summary>
        /// フローを流すグラフの各辺に対応した情報を持ちます。
        /// </summary>
        [DebuggerDisplay("From={" + nameof(From) + "} To={" + nameof(To) + "} Cap={" + nameof(Cap) + "} Flow={" + nameof(Flow) + "} Cost={" + nameof(Cost) + "}")]
        public struct Edge : IEquatable<Edge>
        {
            /// <summary>フローが流出する頂点。</summary>
            public int From { get; set; }
            /// <summary>フローが流入する頂点。</summary>
            public int To { get; set; }
            /// <summary>辺の容量。</summary>
            public TCap Cap { get; set; }
            /// <summary>辺の流量。</summary>
            public TCap Flow { get; set; }
            /// <summary>辺の費用</summary>
            public TCost Cost { get; set; }
            public Edge(int from, int to, TCap cap, TCap flow, TCost cost)
            {
                From = from;
                To = to;
                Cap = cap;
                Flow = flow;
                Cost = cost;
            }

            public override bool Equals(object obj) => obj is Edge edge && Equals(edge);
            [MethodImpl(256)]
            public bool Equals(Edge other) => From == other.From &&
                       To == other.To &&
                       EqualityComparer<TCap>.Default.Equals(Cap, other.Cap) &&
                       EqualityComparer<TCap>.Default.Equals(Flow, other.Flow) &&
                       EqualityComparer<TCost>.Default.Equals(Cost, other.Cost);
            public override int GetHashCode() => HashCode.Combine(From, To, Cap, Flow, Cost);
            public static bool operator ==(Edge left, Edge right) => left.Equals(right);
            public static bool operator !=(Edge left, Edge right) => !left.Equals(right);
        }

        [DebuggerDisplay("To={" + nameof(To) + "} Rev={" + nameof(Rev) + "} Cap={" + nameof(Cap) + "} Cost={" + nameof(Cost) + "}")]
        private struct EdgeInternal
        {
            public int To;
            public int Rev;
            public TCap Cap;
            public TCost Cost;
            public EdgeInternal(int to, int rev, TCap cap, TCost cost)
            {
                To = to;
                Rev = rev;
                Cap = cap;
                Cost = cost;
            }
        }

        private readonly int _n;
        private readonly SimpleList<Edge> _edges = new SimpleList<Edge>();
    }
}
