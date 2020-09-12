using System.Collections.Generic;
using System.Diagnostics;

namespace AtCoder
{
    public class MFGraphInt : MFGraph<int, IntOperator> { public MFGraphInt(int n) : base(n) { } }
    public class MFGraphUInt : MFGraph<uint, UIntOperator> { public MFGraphUInt(int n) : base(n) { } }
    public class MFGraphLong : MFGraph<long, LongOperator> { public MFGraphLong(int n) : base(n) { } }
    public class MFGraphULong : MFGraph<ulong, ULongOperator> { public MFGraphULong(int n) : base(n) { } }
    public class MFGraphDouble : MFGraph<double, DoubleOperator> { public MFGraphDouble(int n) : base(n) { } }
    public class MFGraph<TValue, TOp>
         where TValue : struct
         where TOp : struct, INumOperator<TValue>
    {
        static readonly TOp op = default;
        public MFGraph(int n)
        {
            _n = n;
            _g = new List<EdgeInternal>[n];
            for (int i = 0; i < n; i++)
            {
                _g[i] = new List<EdgeInternal>();
            }
            _pos = new List<(int first, int second)>();
        }

        public int AddEdge(int from, int to, TValue cap)
        {
            int m = _pos.Count;
            Debug.Assert(0 <= from && from < _n);
            Debug.Assert(0 <= to && to < _n);
            Debug.Assert(op.Compare(default, cap) <= 0);
            _pos.Add((from, _g[from].Count));
            _g[from].Add(new EdgeInternal(to, _g[to].Count, cap));
            _g[to].Add(new EdgeInternal(from, _g[from].Count - 1, default));
            return m;
        }

        public Edge GetEdge(int i)
        {
            int m = _pos.Count;
            Debug.Assert(0 <= i && i < m);
            var _e = _g[_pos[i].first][_pos[i].second];
            var _re = _g[_e.To][_e.Rev];
            return new Edge(_pos[i].first, _e.To, op.Add(_e.Cap, _re.Cap), _re.Cap);
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

        public void ChangeEdge(int i, TValue newCap, TValue newFlow)
        {
            int m = _pos.Count;
            Debug.Assert(0 <= i && i < m);
            Debug.Assert(op.Compare(default, newFlow) <= 0 && op.Compare(newFlow, newCap) <= 0);
            var _e = _g[_pos[i].first][_pos[i].second];
            var _re = _g[_e.To][_e.Rev];
            _e.Cap = op.Subtract(newCap, newFlow);
            _re.Cap = newFlow;
        }

        public TValue Flow(int s, int t)
        {
            return Flow(s, t, op.MaxValue);
        }

        public TValue Flow(int s, int t, TValue flowLimit)
        {
            Debug.Assert(0 <= s && s < _n);
            Debug.Assert(0 <= t && t < _n);

            var level = new int[_n];
            var iter = new int[_n];
            var que = new Queue<int>();

            void Bfs()
            {
                for (int i = 0; i < _n; i++)
                {
                    level[i] = -1;
                }

                level[s] = 0;
                que.Clear();
                que.Enqueue(s);
                while (que.Count > 0)
                {
                    int v = que.Dequeue();
                    foreach (var e in _g[v])
                    {
                        if (op.Equals(e.Cap, default) || level[e.To] >= 0) continue;
                        level[e.To] = level[v] + 1;
                        if (e.To == t) return;
                        que.Enqueue(e.To);
                    }
                }
            }

            TValue Dfs(int v, TValue up)
            {
                if (v == s) return up;
                var res = default(TValue);
                int level_v = level[v];
                for (; iter[v] < _g[v].Count; iter[v]++)
                {
                    EdgeInternal e = _g[v][iter[v]];
                    if (level_v <= level[e.To] || op.Equals(_g[e.To][e.Rev].Cap, default)) continue;
                    var up1 = op.Subtract(up, res);
                    var up2 = _g[e.To][e.Rev].Cap;
                    var d = Dfs(e.To, op.Compare(up1, up2) < 0 ? up1 : up2);
                    if (op.Compare(d, default) <= 0) continue;
                    _g[v][iter[v]].Cap = op.Add(_g[v][iter[v]].Cap, d);
                    _g[e.To][e.Rev].Cap = op.Subtract(_g[e.To][e.Rev].Cap, d);
                    res = op.Add(res, d);
                    if (res.Equals(up)) break;
                }

                return res;
            }

            TValue flow = default;
            while (op.Compare(flow, flowLimit) < 0)
            {
                Bfs();
                if (level[t] == -1) break;
                for (int i = 0; i < _n; i++)
                {
                    iter[i] = 0;
                }
                while (op.Compare(flow, flowLimit) < 0)
                {
                    var f = Dfs(t, op.Subtract(flowLimit, flow));
                    if (op.Equals(f, default)) break;
                    flow = op.Add(flow, f);
                }
            }
            return flow;
        }

        public bool[] MinCut(int s)
        {
            var visited = new bool[_n];
            var que = new Queue<int>();
            que.Enqueue(s);
            while (que.Count > 0)
            {
                int p = que.Dequeue();
                visited[p] = true;
                foreach (var e in _g[p])
                {
                    if (!op.Equals(e.Cap, default) && !visited[e.To])
                    {
                        visited[e.To] = true;
                        que.Enqueue(e.To);
                    }
                }
            }

            return visited;
        }

        public struct Edge
        {
            public int From { get; set; }
            public int To { get; set; }
            public TValue Cap { get; set; }
            public TValue Flow { get; set; }
            public Edge(int from, int to, TValue cap, TValue flow)
            {
                From = from;
                To = to;
                Cap = cap;
                Flow = flow;
            }
        };

        private class EdgeInternal
        {
            public int To { get; set; }
            public int Rev { get; set; }
            public TValue Cap { get; set; }
            public EdgeInternal(int to, int rev, TValue cap)
            {
                To = to;
                Rev = rev;
                Cap = cap;
            }
        };

        private readonly int _n;
        private readonly List<(int first, int second)> _pos;
        private readonly List<EdgeInternal>[] _g;
    }
}
