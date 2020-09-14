using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;

namespace AtCoder
{
    public class McfGraph<TValue, TOp>
        where TValue : struct
        where TOp : struct, INumOperator<TValue>
    {
        static readonly TOp op = default;

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

        public int AddEdge(int from, int to, TValue cap, TValue cost)
        {
            Debug.Assert(0 <= from && from < _n);
            Debug.Assert(0 <= to && to < _n);
            int m = _pos.Count;
            _pos.Add((from, _g[from].Count));
            _g[from].Add(new EdgeInternal(to, _g[to].Count, cap, cost));
            _g[to].Add(new EdgeInternal(from, _g[from].Count - 1, default, op.Minus(cost)));
            return m;
        }

        public Edge GetEdge(int i)
        {
            int m = _pos.Count;
            Debug.Assert(0 <= i && i < m);
            var _e = _g[_pos[i].first][_pos[i].second];
            var _re = _g[_e.To][_e.Rev];
            return new Edge(_pos[i].first, _e.To, op.Add(_e.Cap, _re.Cap), _re.Cap, _e.Cost);
        }

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

        public (TValue cap, TValue cost) Flow(int s, int t)
        {
            return flow(s, t, op.MaxValue);
        }
        public (TValue cap, TValue cost) flow(int s, int t, TValue flow_limit)
        {
            return Slope(s, t, flow_limit).Last();
        }
        public List<(TValue cap, TValue cost)> Slope(int s, int t)
        {
            return Slope(s, t, op.MaxValue);
        }

        public List<(TValue cap, TValue cost)> Slope(int s, int t, TValue flow_limit)
        {
            Debug.Assert(0 <= s && s < _n);
            Debug.Assert(0 <= t && t < _n);
            Debug.Assert(s != t);
            // variants (C = maxcost):
            // -(n-1)C <= dual[s] <= dual[i] <= dual[t] = 0
            // reduced cost (= e.cost + dual[e.from] - dual[e.to]) >= 0 for all edge
            var dual = new TValue[_n];
            var dist = new TValue[_n];
            var pv = new int[_n];
            var pe = new int[_n];
            var vis = new bool[_n];

            bool DualRef()
            {
                dist.AsSpan().Fill(op.MaxValue);
                pv.AsSpan().Fill(-1);
                pe.AsSpan().Fill(-1);
                vis.AsSpan().Fill(false);

                var que = new PriorityQueueForMcf();
                dist[s] = default;
                que.Enqueue(default, s);
                while (que.Count > 0)
                {
                    int v = que.Dequeue().to;
                    if(vis[v]) continue;
                    vis[v] = true;
                    if(v == t) break;
                    // dist[v] = shortest(s, v) + dual[s] - dual[v]
                    // dist[v] >= 0 (all reduced cost are positive)
                    // dist[v] <= (n-1)C
                    for (int i = 0; i < _g[v].Count; i++)
                    {
                        var e = _g[v][i];
                        if(vis[e.To] || op.Equals(e.Cap, default)) continue;
                        // |-dual[e.to] + dual[v]| <= (n-1)C
                        // cost <= C - -(n-1)C + 0 = nC
                        TValue cost = op.Add(op.Subtract(e.Cost, dual[e.To]), dual[v]);
                        if (op.GreaterThan(op.Subtract(dist[e.To], dist[v]), cost)) {
                            dist[e.To] = op.Add(dist[v], cost);
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
                    dual[v] = op.Subtract(dual[v], op.Subtract(dist[t], dist[v]));
                }

                return true;
            }

            TValue flow = default;
            TValue cost = default;
            TValue prev_cost = op.Decrement(default); //-1
            var result = new List<(TValue cap, TValue cost)>();
            result.Add((flow, cost));
            while (op.LessThan(flow, flow_limit))
            {
                if (!DualRef()) break;
                TValue c = op.Subtract(flow_limit, flow);
                for (int v = t; v != s; v = pv[v])
                {
                    if (op.LessThan(_g[pv[v]][pe[v]].Cap, c))
                    {
                        c = _g[pv[v]][pe[v]].Cap;
                    }
                }
                for (int v = t; v != s; v = pv[v])
                {
                    _g[pv[v]][pe[v]].Cap = op.Subtract(_g[pv[v]][pe[v]].Cap, c);
                    _g[v][_g[pv[v]][pe[v]].Rev].Cap = op.Add(_g[v][_g[pv[v]][pe[v]].Rev].Cap, c);
                }
                TValue d = op.Minus(dual[s]);
                flow = op.Add(flow, c);
                cost = op.Add(cost, op.Multiply(c, d));
                if (op.Equals(prev_cost, d))
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
            public TValue Cap { get; set; }
            /// <summary>辺の流量。</summary>
            public TValue Flow { get; set; }
            /// <summary>辺の費用</summary>
            public TValue Cost { get; set; }
            public Edge(int from, int to, TValue cap, TValue flow, TValue cost)
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
            public TValue Cap { get; set; }
            public TValue Cost { get; set; }
            public EdgeInternal(int to, int rev, TValue cap, TValue cost)
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
            private (TValue cost, int to)[] _heap;

            public int Count { get; private set; } = 0;
            public PriorityQueueForMcf()
            {
                _heap = new (TValue cost, int to)[1024];
            }

            public void Enqueue(TValue cost, int to)
            {
                var pair = (cost, to);
                if (_heap.Length == Count)
                {
                    var newHeap = new (TValue cost, int to)[_heap.Length * 2];
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

            public (TValue cost, int to) Dequeue()
            {
                (TValue cost, int to) ret = _heap[0];
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
            private int Compare(TValue x, TValue y) => op.Compare(y, x);
        }
    }
}
