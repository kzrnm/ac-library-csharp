using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AtCoder
{
    /// <summary>
    /// 最大フロー問題 を解くライブラリ(int版)です。
    /// </summary>
    public class MFGraphInt : MFGraph<int, IntOperator> { public MFGraphInt(int n) : base(n) { } }

    /// <summary>
    /// 最大フロー問題 を解くライブラリ(long版)です。
    /// </summary>
    public class MFGraphLong : MFGraph<long, LongOperator> { public MFGraphLong(int n) : base(n) { } }

    /// <summary>
    /// 最大フロー問題 を解くライブラリです。
    /// </summary>
    /// <typeparam name="TValue">容量の型</typeparam>
    /// <typeparam name="TOp"><typeparamref name="TValue"/>に対応する演算を提要する型</typeparam>
    /// <remarks>
    /// <para>制約: <typeparamref name="TValue"/> は int, long。</para>
    /// <para>
    /// 内部では各辺 e について 2 つの変数、流量 f_e と容量 c_e を管理しています。
    /// 頂点 v から出る辺の集合を out(v)、入る辺の集合を in(v)、
    /// また頂点 v について g(v, f) = (Σ_in(v) f_e) - (Σ_out(v) f_e) とします。 
    /// </para>
    /// </remarks>
    public class MFGraph<TValue, TOp>
         where TValue : struct
         where TOp : struct, INumOperator<TValue>
    {
        static readonly TOp op = default;

        /// <summary>
        /// <paramref name="n"/> 頂点 0 辺のグラフを作ります。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0 ≤ <paramref name="n"/> ≤ 10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
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

        /// <summary>
        /// <paramref name="from"/> から <paramref name="to"/> へ
        /// 最大容量 <paramref name="cap"/>、流量 0 の辺を追加し、何番目に追加された辺かを返します。
        /// </summary>
        /// <remarks>
        /// 制約: 
        /// <list type="bullet">
        /// <item>
        /// <description>0 ≤ <paramref name="from"/>, <paramref name="to"/> &lt; n</description>
        /// </item>
        /// <item>
        /// <description>0 ≤ <paramref name="cap"/></description>
        /// </item>
        /// </list>
        /// <para>計算量: ならしO(1)</para>
        /// </remarks>
        public int AddEdge(int from, int to, TValue cap)
        {
            int m = _pos.Count;
            Debug.Assert(0 <= from && from < _n);
            Debug.Assert(0 <= to && to < _n);
            Debug.Assert(op.LessThanOrEqual(default, cap));
            _pos.Add((from, _g[from].Count));
            int fromId = _g[from].Count;
            int toId = _g[to].Count;

            if (from == to) toId++;

            _g[from].Add(new EdgeInternal(to, toId, cap));
            _g[to].Add(new EdgeInternal(from, fromId, default));
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
            return new Edge(_pos[i].first, _e.To, op.Add(_e.Cap, _re.Cap), _re.Cap);
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
        /// <paramref name="i"/> 番目に追加された辺の容量、流量を
        /// <paramref name="newCap"/>, <paramref name="newFlow"/> に変更します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// 他の辺の容量、流量は変更しません。
        /// 辺 <paramref name="i"/> の流量、容量のみを
        /// <paramref name="newCap"/>, <paramref name="newFlow"/> へ変更します。
        /// </para>
        /// </remarks>
        public void ChangeEdge(int i, TValue newCap, TValue newFlow)
        {
            int m = _pos.Count;
            Debug.Assert(0 <= i && i < m);
            Debug.Assert(op.LessThanOrEqual(default, newFlow) && op.LessThanOrEqual(newFlow, newCap));
            var _e = _g[_pos[i].first][_pos[i].second];
            var _re = _g[_e.To][_e.Rev];
            _e.Cap = op.Subtract(newCap, newFlow);
            _re.Cap = newFlow;
        }

        /// <summary>
        /// 頂点 <paramref name="s"/> から <paramref name="t"/> へ流せる限り流し、
        /// 流せた量を返します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// 複数回呼ぶことも可能で、その時の挙動は
        /// 変更前と変更後の流量を f_e, f'_e として、以下の条件を満たすように変更します。
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description>0 ≤ f'_e ≤ C_e</description>
        /// </item>
        /// <item>
        /// <description>
        /// <paramref name="s"/>, <paramref name="t"/> 以外の頂天 v について、
        /// g(v, f) = g(v, f')
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// (flowLimit を指定した場合) g(t, f') - g(t, f) ≤ flowLimit
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// g(t, f') - g(t, f) が委譲の条件を満たすうち最大。この g(t, f') - g(t, f) を返す。
        /// </description>
        /// </item>
        /// </list>
        /// <para>制約: 返値が <typeparamref name="TValue"/> に収まる。</para>
        /// 計算量: m を追加された辺数として、
        /// <list type="bullet">
        /// <item>
        /// <description>O(min(n^(2/3) m, m^(3/2))) (辺の容量が全部 1 の時)</description>
        /// </item>
        /// <item>
        /// <description>O(n^2 m)</description>
        /// </item>
        /// </list>
        /// </remarks>
        public TValue Flow(int s, int t)
        {
            return Flow(s, t, op.MaxValue);
        }

        /// <summary>
        /// 頂点 <paramref name="s"/> から <paramref name="t"/> へ
        /// 流量 <paramref name="flowLimit"/> に達するまで流せる限り流し、
        /// 流せた量を返します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// 複数回呼ぶことも可能で、その時の挙動は
        /// 変更前と変更後の流量を f_e, f'_e として、以下の条件を満たすように変更します。
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description>0 ≤ f'_e ≤ C_e</description>
        /// </item>
        /// <item>
        /// <description>
        /// <paramref name="s"/>, <paramref name="t"/> 以外の頂天 v について、
        /// g(v, f) = g(v, f')
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// (<paramref name="flowLimit"/> を指定した場合) g(t, f') - g(t, f) ≤ <paramref name="flowLimit"/>
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// g(t, f') - g(t, f) が委譲の条件を満たすうち最大。この g(t, f') - g(t, f) を返す。
        /// </description>
        /// </item>
        /// </list>
        /// <para>制約: 返値が <typeparamref name="TValue"/> に収まる。</para>
        /// 計算量: m を追加された辺数として、
        /// <list type="bullet">
        /// <item>
        /// <description>O(min(n^(2/3) m, m^(3/2))) (辺の容量が全部 1 の時)</description>
        /// </item>
        /// <item>
        /// <description>O(n^2 m)</description>
        /// </item>
        /// </list>
        /// </remarks>
        public TValue Flow(int s, int t, TValue flowLimit)
        {
            Debug.Assert(0 <= s && s < _n);
            Debug.Assert(0 <= t && t < _n);
            Debug.Assert(s != t);

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
                    var d = Dfs(e.To, op.LessThan(up1, up2) ? up1 : up2);
                    if (op.Compare(d, default) <= 0) continue;
                    _g[v][iter[v]].Cap = op.Add(_g[v][iter[v]].Cap, d);
                    _g[e.To][e.Rev].Cap = op.Subtract(_g[e.To][e.Rev].Cap, d);
                    res = op.Add(res, d);
                    if (res.Equals(up)) break;
                }

                return res;
            }

            TValue flow = default;
            while (op.LessThan(flow, flowLimit))
            {
                Bfs();
                if (level[t] == -1) break;
                for (int i = 0; i < _n; i++)
                {
                    iter[i] = 0;
                }
                while (op.LessThan(flow, flowLimit))
                {
                    var f = Dfs(t, op.Subtract(flowLimit, flow));
                    if (op.Equals(f, default)) break;
                    flow = op.Add(flow, f);
                }
            }
            return flow;
        }

        /// <summary>
        /// 長さ n の配列を返します。
        /// i 番目の要素には、頂点 <paramref name="s"/> から i へ残余グラフで到達可能なとき、
        /// またその時のみ true を返します。
        /// Flow(s, t) を flowLimit なしでちょうど一回呼んだ後に呼ぶと、
        /// 返り値はs, t 間の mincut に対応します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// 各辺 e = (u, v, f_e, c_e) について、 f_e &lt; c_e ならば辺 (u, v) を張り、
        /// 0 &lt; f_e ならば辺 (u, v) を張ったと仮定したとき、
        /// 頂点 ss から到達可能な頂点の集合を返します。
        /// </para>
        /// 計算量: m を追加された辺数として、
        /// <list type="bullet">
        /// <item>
        /// <description>O(n + m)</description>
        /// </item>
        /// </list>
        /// </remarks>
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

        /// <summary>
        /// フロー流すグラフの各辺に対応した情報を持ちます。
        /// </summary>
        public struct Edge : IEquatable<Edge>
        {
            /// <summary>フローが流出する頂点。</summary>
            public int From { get; set; }
            /// <summary>フローが流入する頂点。</summary>
            public int To { get; set; }
            /// <summary>辺の容量。</summary>
            public TValue Cap { get; set; }
            /// <summary>辺の流量。</summary>
            public TValue Flow { get; set; }
            public Edge(int from, int to, TValue cap, TValue flow)
            {
                From = from;
                To = to;
                Cap = cap;
                Flow = flow;
            }

            public override bool Equals(object obj)
                => obj is Edge edge && Equals(edge);

            public bool Equals(Edge other)
                => From == other.From &&
                       To == other.To &&
                       EqualityComparer<TValue>.Default.Equals(Cap, other.Cap) &&
                       EqualityComparer<TValue>.Default.Equals(Flow, other.Flow);
            public override int GetHashCode()
                => HashCode.Combine(From, To, Cap, Flow);
            public static bool operator ==(Edge left, Edge right) => left.Equals(right);
            public static bool operator !=(Edge left, Edge right) => !left.Equals(right);
        };

        internal class EdgeInternal
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

        internal readonly int _n;
        internal readonly List<(int first, int second)> _pos;
        internal readonly List<EdgeInternal>[] _g;
    }
}
