using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AtCoder
{
    public interface ICastOperator<TFrom, TTo>
        where TFrom : struct
        where TTo : struct
    {
        TTo Cast(TFrom y);
    }

    public struct SameTypeCastOperator<T> : ICastOperator<T, T>
        where T : struct
    {
        public T Cast(T y) => y;
    }

    public struct IntToLongCastOperator : ICastOperator<int, long>
    {
        public long Cast(int y) => y;
    }

    /// <summary>
    /// Minimum-cost flow problem を扱うライブラリ(int版)です。
    /// </summary>
    public class McfGraphInt
        : McfGraph<int, IntOperator, int, IntOperator, SameTypeCastOperator<int>>
    {
        public McfGraphInt(int n) : base(n) { }
    }

    /// <summary>
    /// Minimum-cost flow problem を扱うライブラリ(long版)です。
    /// </summary>
    public class McfGraphLong
       : McfGraph<long, LongOperator, long, LongOperator, SameTypeCastOperator<long>>
    {
        public McfGraphLong(int n) : base(n) { }
    }

    /// <summary>
    /// Minimum-cost flow problem を扱うライブラリです。
    /// </summary>
    /// <typeparam name="TCap">容量の型</typeparam>
    /// <typeparam name="TCapOp"><typeparamref name="TCap"/> に対応する演算を提供する型</typeparam>
    /// <typeparam name="TCost">コストの型</typeparam>
    /// <typeparam name="TCostOp"><typeparamref name="TCost"/> に対応する演算を提供する型</typeparam>
    /// <typeparam name="TCast">
    /// <typeparamref name="TCap"/> から <typeparamref name="TCost"/> への型変換を提供する型
    /// </typeparam>
    /// <remarks>
    /// <para>制約: <typeparamref name="TValue"/> は int, long。</para>
    /// </remarks>
    public class McfGraph<TCap, TCapOp, TCost, TCostOp, TCast>
        where TCap : struct
        where TCapOp : struct, INumOperator<TCap>
        where TCost : struct
        where TCostOp : struct, INumOperator<TCost>
        where TCast : ICastOperator<TCap, TCost>
    {
        static readonly TCapOp capOp = default;
        static readonly TCostOp costOp = default;
        static readonly TCast cast = default;

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
            _g = new List<EdgeInternal>[n];
            for (int i = 0; i < n; i++)
            {
                _g[i] = new List<EdgeInternal>();
            }
            _pos = new List<(int first, int second)>();
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
        public int AddEdge(int from, int to, TCap cap, TCost cost)
        {
            Debug.Assert(0 <= from && from < _n);
            Debug.Assert(0 <= to && to < _n);
            int m = _pos.Count;
            _pos.Add((from, _g[from].Count));
            _g[from].Add(new EdgeInternal(to, _g[to].Count, cap, cost));
            _g[to].Add(new EdgeInternal(from, _g[from].Count - 1, default, costOp.Minus(cost)));
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
        public Edge GetEdge(int i)
        {
            int m = _pos.Count;
            Debug.Assert(0 <= i && i < m);
            var _e = _g[_pos[i].first][_pos[i].second];
            var _re = _g[_e.To][_e.Rev];
            return new Edge(_pos[i].first, _e.To, capOp.Add(_e.Cap, _re.Cap), _re.Cap, _e.Cost);
        }

        /// <summary>
        /// 今の内部の辺の状態を返します。
        /// </summary>
        /// <remarks>
        /// <para>辺の順番はadd_edgeで追加された順番と同一。</para>
        /// <para>計算量: m を追加された辺数として O(m)</para>
        /// </remarks>
        public List<Edge> Edges()
        {
            int m = _pos.Count;
            var result = new List<Edge>();
            for (int i = 0; i < m; i++)
            {
                result.Add(GetEdge(i));
            }
            return result;
        }

        /// <summary>
        /// 頂点 <paramref name="s"/> から <paramref name="t"/> へ流せる限り流し、
        /// その流量とコストを返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: Slope関数と同じ</para>
        /// <para>計算量: Slope関数と同じ</para>
        /// </remarks>
        public (TCap cap, TCost cost) Flow(int s, int t)
        {
            return Flow(s, t, capOp.MaxValue);
        }

        /// <summary>
        /// 頂点 <paramref name="s"/> から <paramref name="t"/> へ
        /// 流量 <paramref name="flowLimit"/> に達するまで流せる限り流し、
        /// その流量とコストを返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: Slope関数と同じ</para>
        /// <para>計算量: Slope関数と同じ</para>
        /// </remarks>
        public (TCap cap, TCost cost) Flow(int s, int t, TCap flowLimit)
        {
            return Slope(s, t, flowLimit).Last();
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
        /// O(F(n + m) log n)
        /// </remarks>
        public List<(TCap cap, TCost cost)> Slope(int s, int t)
        {
            return Slope(s, t, capOp.MaxValue);
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
        /// O(F(n + m) log n)
        /// </remarks>
        public List<(TCap cap, TCost cost)> Slope(int s, int t, TCap flowLimit)
        {
            Debug.Assert(0 <= s && s < _n);
            Debug.Assert(0 <= t && t < _n);
            Debug.Assert(s != t);
            // variants (C = maxcost):
            // -(n-1)C <= dual[s] <= dual[i] <= dual[t] = 0
            // reduced cost (= e.cost + dual[e.from] - dual[e.to]) >= 0 for all edge
            var dual = new TCost[_n];
            var dist = new TCost[_n];
            var pv = new int[_n];
            var pe = new int[_n];
            var vis = new bool[_n];

            bool DualRef()
            {
                dist.AsSpan().Fill(costOp.MaxValue);
                pv.AsSpan().Fill(-1);
                pe.AsSpan().Fill(-1);
                vis.AsSpan().Fill(false);

                var que = new PriorityQueueForMcf();
                dist[s] = default;
                que.Enqueue(default, s);
                while (que.Count > 0)
                {
                    int v = que.Dequeue().to;
                    if (vis[v]) continue;
                    vis[v] = true;
                    if (v == t) break;
                    // dist[v] = shortest(s, v) + dual[s] - dual[v]
                    // dist[v] >= 0 (all reduced cost are positive)
                    // dist[v] <= (n-1)C
                    for (int i = 0; i < _g[v].Count; i++)
                    {
                        var e = _g[v][i];
                        if (vis[e.To] || capOp.Equals(e.Cap, default)) continue;
                        // |-dual[e.to] + dual[v]| <= (n-1)C
                        // cost <= C - -(n-1)C + 0 = nC
                        TCost cost = costOp.Add(costOp.Subtract(e.Cost, dual[e.To]), dual[v]);
                        if (costOp.GreaterThan(costOp.Subtract(dist[e.To], dist[v]), cost))
                        {
                            dist[e.To] = costOp.Add(dist[v], cost);
                            pv[e.To] = v;
                            pe[e.To] = i;
                            que.Enqueue(dist[e.To], e.To);
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
                    //         = dual[v] - (shortest(s, t) + dual[s] - dual[t]) + (shortest(s, v) + dual[s] - dual[v])
                    //         = - shortest(s, t) + dual[t] + shortest(s, v)
                    //         = shortest(s, v) - shortest(s, t) >= 0 - (n-1)C
                    dual[v] = costOp.Subtract(dual[v], costOp.Subtract(dist[t], dist[v]));
                }

                return true;
            }

            TCap flow = default;
            TCost cost = default;
            TCost prev_cost = costOp.Decrement(default); //-1
            var result = new List<(TCap cap, TCost cost)>();
            result.Add((flow, cost));
            while (capOp.LessThan(flow, flowLimit))
            {
                if (!DualRef()) break;
                TCap c = capOp.Subtract(flowLimit, flow);
                for (int v = t; v != s; v = pv[v])
                {
                    if (capOp.LessThan(_g[pv[v]][pe[v]].Cap, c))
                    {
                        c = _g[pv[v]][pe[v]].Cap;
                    }
                }
                for (int v = t; v != s; v = pv[v])
                {
                    _g[pv[v]][pe[v]].Cap = capOp.Subtract(_g[pv[v]][pe[v]].Cap, c);
                    _g[v][_g[pv[v]][pe[v]].Rev].Cap = capOp.Add(_g[v][_g[pv[v]][pe[v]].Rev].Cap, c);
                }
                TCost d = costOp.Minus(dual[s]);
                flow = capOp.Add(flow, c);
                cost = costOp.Add(cost, costOp.Multiply(cast.Cast(c), d));
                if (costOp.Equals(prev_cost, d))
                {
                    result.RemoveAt(result.Count - 1);
                }
                result.Add((flow, cost));
                prev_cost = cost;
            }
            return result;
        }

        /// <summary>
        /// フローを流すグラフの各辺に対応した情報を持ちます。
        /// </summary>
        public struct Edge
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
        };

        private class EdgeInternal
        {
            public int To { get; set; }
            public int Rev { get; set; }
            public TCap Cap { get; set; }
            public TCost Cost { get; set; }
            public EdgeInternal(int to, int rev, TCap cap, TCost cost)
            {
                To = to;
                Rev = rev;
                Cap = cap;
                Cost = cost;
            }
        };

        private readonly int _n;
        private readonly List<(int first, int second)> _pos;
        private readonly List<EdgeInternal>[] _g;

        private class PriorityQueueForMcf
        {
            private (TCost cost, int to)[] _heap;

            public int Count { get; private set; } = 0;
            public PriorityQueueForMcf()
            {
                _heap = new (TCost cost, int to)[1024];
            }

            public void Enqueue(TCost cost, int to)
            {
                var pair = (cost, to);
                if (_heap.Length == Count)
                {
                    var newHeap = new (TCost cost, int to)[_heap.Length * 2];
                    _heap.CopyTo(newHeap, 0);
                    _heap = newHeap;
                }

                _heap[Count] = pair;
                ++Count;

                int c = Count - 1;
                while (c > 0)
                {
                    int p = (c - 1) >> 1;
                    if (Compare(_heap[p].cost, cost) < 0)
                    {
                        _heap[c] = _heap[p];
                        c = p;
                    }
                    else
                    {
                        break;
                    }
                }

                _heap[c] = pair;
            }

            public (TCost cost, int to) Dequeue()
            {
                (TCost cost, int to) ret = _heap[0];
                int n = Count - 1;

                var item = _heap[n];
                int p = 0;
                int c = (p << 1) + 1;
                while (c < n)
                {
                    if (c != n - 1 && Compare(_heap[c + 1].cost, _heap[c].cost) > 0)
                    {
                        ++c;
                    }

                    if (Compare(item.cost, _heap[c].cost) < 0)
                    {
                        _heap[p] = _heap[c];
                        p = c;
                        c = (p << 1) + 1;
                    }
                    else
                    {
                        break;
                    }
                }

                _heap[p] = item;
                Count--;

                return ret;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private int Compare(TCost x, TCost y) => costOp.Compare(y, x);
        }
    }
}
