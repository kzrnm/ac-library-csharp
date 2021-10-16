using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AtCoder.Internal;
using AtCoder.Operators;

namespace AtCoder
{
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
    public class MfGraph<TValue, TOp>
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
        public MfGraph(int n)
        {
            _n = n;
            _g = new SimpleList<EdgeInternal>[n];
            for (int i = 0; i < _g.Length; i++)
            {
                _g[i] = new SimpleList<EdgeInternal>();
            }
            _pos = new SimpleList<(int first, int second)>();
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
            Contract.Assert((uint)from < (uint)_n, reason: $"IndexOutOfRange: 0 <= {nameof(from)} && {nameof(from)} < _n");
            Contract.Assert((uint)to < (uint)_n, reason: $"IndexOutOfRange: 0 <= {nameof(to)} && {nameof(to)} < _n");
            Contract.Assert(op.LessThanOrEqual(default, cap), reason: $"IndexOutOfRange: 0 <= {nameof(cap)}");
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
            Contract.Assert((uint)i < (uint)_pos.Count, reason: $"IndexOutOfRange: 0 <= {nameof(i)} && {nameof(i)} < edgeCount");
            var (first, second) = _pos[i];
            var (to, rev, cap) = _g[first][second];
            var _reCap = _g[to][rev].Cap;
            return new Edge(first, to, op.Add(cap, _reCap), _reCap);
        }

        /// <summary>
        /// 今の内部の辺の状態を返します。
        /// </summary>
        /// <remarks>
        /// <para>辺の順番はadd_edgeで追加された順番と同一。</para>
        /// <para>計算量: m を追加された辺数として O(m)</para>
        /// </remarks>
        public Edge[] Edges()
        {
            var result = new Edge[_pos.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = GetEdge(i);
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
            Contract.Assert((uint)i < (uint)_pos.Count, reason: $"IndexOutOfRange: 0 <= {nameof(i)} && {nameof(i)} < edgeCount");
            Contract.Assert(op.LessThanOrEqual(default, newFlow) && op.LessThanOrEqual(newFlow, newCap), reason: $"IndexOutOfRange: 0 <= {nameof(newFlow)} && {nameof(newFlow)} <= {nameof(newCap)}");
            var (first, second) = _pos[i];
            var (to, rev, _) = _g[first][second];
            //var _re = _g[_e.To][_e.Rev];
            _g[first][second].Cap = op.Subtract(newCap, newFlow);
            _g[to][rev].Cap = newFlow;
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
            Contract.Assert((uint)s < (uint)_n, reason: $"IndexOutOfRange: 0 <= {nameof(s)} && {nameof(s)} < _n");
            Contract.Assert((uint)t < (uint)_n, reason: $"IndexOutOfRange: 0 <= {nameof(t)} && {nameof(t)} < _n");
            Contract.Assert(s != t, reason: $"{nameof(s)} and {nameof(t)} must be different.");

            var level = new int[_n];
            int[] iter;
            var que = new Queue<int>();

            void Bfs()
            {
                level.AsSpan().Fill(-1);
                level[s] = 0;
                que.Clear();
                que.Enqueue(s);
                while (que.Count > 0)
                {
                    int v = que.Dequeue();
                    foreach (var (to, _, cap) in _g[v].AsSpan())
                    {
                        if (EqualityComparer<TValue>.Default.Equals(cap, default) || level[to] >= 0) continue;
                        level[to] = level[v] + 1;
                        if (to == t) return;
                        que.Enqueue(to);
                    }
                }
            }

            //TValue Dfs(int v, TValue up)
            //{
            //    if (v == s) return up;
            //    var res = default(TValue);
            //    for (; iter[v] < _g[v].Count; iter[v]++)
            //    {
            //        EdgeInternal e = _g[v][iter[v]];
            //        if (level[v] <= level[e.To] || EqualityComparer<TValue>.Default.Equals(_g[e.To][e.Rev].Cap, default)) continue;
            //        var up1 = op.Subtract(up, res);
            //        var up2 = _g[e.To][e.Rev].Cap;
            //        var d = Dfs(e.To, op.LessThan(up1, up2) ? up1 : up2);
            //        if (op.LessThanOrEqual(d, default)) continue;
            //        _g[v][iter[v]].Cap = op.Add(_g[v][iter[v]].Cap, d);
            //        _g[e.To][e.Rev].Cap = op.Subtract(_g[e.To][e.Rev].Cap, d);
            //        res = op.Add(res, d);
            //        if (EqualityComparer<TValue>.Default.Equals(res, up)) return res;
            //    }
            //    level[v] = _n;
            //    return res;
            //}

            TValue Dfs(int v, TValue up)
            {
                var lastRes = default(TValue);
                var stack = new Stack<(int v, TValue up, TValue res, bool childOk)>();
                stack.Push((v, up, default, true));

            DFS: while (stack.Count > 0)
                {
                    TValue res;
                    bool childOk;
                    (v, up, res, childOk) = stack.Pop();
                    if (v == s)
                    {
                        lastRes = up;
                        continue;
                    }
                    for (ref var itrv = ref iter[v]; itrv < _g[v].Count; itrv++)
                    {
                        var (to, rev, cap) = _g[v][itrv];
                        if (childOk)
                        {
                            if (level[v] <= level[to] || EqualityComparer<TValue>.Default.Equals(_g[to][rev].Cap, default))
                                continue;

                            var up1 = op.Subtract(up, res);
                            var up2 = _g[to][rev].Cap;
                            stack.Push((v, up, res, false));
                            stack.Push((to, op.LessThan(up1, up2) ? up1 : up2, default, true));
                            goto DFS;
                        }
                        else
                        {
                            var d = lastRes;
                            if (op.GreaterThan(d, default))
                            {
                                _g[v][itrv].Cap = op.Add(cap, d);
                                _g[to][rev].Cap = op.Subtract(_g[to][rev].Cap, d);
                                res = op.Add(res, d);

                                if (EqualityComparer<TValue>.Default.Equals(res, up))
                                {
                                    lastRes = res;
                                    goto DFS;
                                }
                            }
                            childOk = true;
                        }
                    }
                    level[v] = _n;
                    lastRes = res;
                }
                return lastRes;
            }

            TValue flow = default;
            while (op.LessThan(flow, flowLimit))
            {
                Bfs();
                if (level[t] == -1) break;
                iter = new int[_n];
                var f = Dfs(t, op.Subtract(flowLimit, flow));
                if (EqualityComparer<TValue>.Default.Equals(f, default)) break;
                flow = op.Add(flow, f);
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
                foreach (var (to, _, cap) in _g[p])
                {
                    if (!EqualityComparer<TValue>.Default.Equals(cap, default) && !visited[to])
                    {
                        visited[to] = true;
                        que.Enqueue(to);
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
        }

        internal struct EdgeInternal
        {
            int To { get; }
            int Rev { get; }
            public TValue Cap { get; set; }
            public EdgeInternal(int to, int rev, TValue cap)
            {
                To = to;
                Rev = rev;
                Cap = cap;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Deconstruct(out int to, out int rev, out TValue cap)
            {
                to = To;
                rev = Rev;
                cap = Cap;
            }
        }

        internal readonly int _n;
        internal readonly SimpleList<(int first, int second)> _pos;
        internal readonly SimpleList<EdgeInternal>[] _g;
    }
}
