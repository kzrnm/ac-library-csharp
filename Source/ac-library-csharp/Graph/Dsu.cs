using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AtCoder.Internal;

namespace AtCoder
{
    /// <summary>
    /// Disjoint set union
    /// </summary>
    public class Dsu
    {
        /// <summary>
        /// Parent or size. A negative value indicates size, a positive value indicates parent.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public readonly int[] _ps;

        /// <summary>
        /// <see cref="Dsu"/> クラスの新しいインスタンスを、<paramref name="n"/> 頂点 0 辺のグラフとして初期化します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>≤10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        public Dsu(int n)
        {
            _ps = new int[n];
            _ps.AsSpan().Fill(-1);
        }

        /// <summary>
        /// 頂点 <paramref name="a"/> と頂点 <paramref name="b"/> を結ぶ辺を追加し、それらの代表元を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>, <paramref name="b"/>&lt;n</para>
        /// <para>計算量: ならしO(a(n))</para>
        /// </remarks>
        [MethodImpl(256)]
        public int Merge(int a, int b)
        {
            Contract.Assert((uint)a < (uint)_ps.Length, reason: $"IndexOutOfRange: 0 <= {nameof(a)} && {nameof(a)} < n");
            Contract.Assert((uint)b < (uint)_ps.Length, reason: $"IndexOutOfRange: 0 <= {nameof(b)} && {nameof(b)} < n");
            int x = Leader(a), y = Leader(b);
            if (x == y) return x;
            if (-_ps[x] < -_ps[y]) (x, y) = (y, x);
            _ps[x] += _ps[y];
            _ps[y] = x;
            return x;
        }

        /// <summary>
        /// 頂点 <paramref name="a"/>, <paramref name="b"/> が連結かどうかを返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>, <paramref name="b"/>&lt;n</para>
        /// <para>計算量: ならしO(a(n))</para>
        /// </remarks>
        [MethodImpl(256)]
        public bool Same(int a, int b)
        {
            Contract.Assert((uint)a < (uint)_ps.Length, reason: $"IndexOutOfRange: 0 <= {nameof(a)} && {nameof(a)} < n");
            Contract.Assert((uint)b < (uint)_ps.Length, reason: $"IndexOutOfRange: 0 <= {nameof(b)} && {nameof(b)} < n");
            return Leader(a) == Leader(b);
        }

        /// <summary>
        /// 頂点 <paramref name="a"/> の属する連結成分の代表元を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>&lt;n</para>
        /// <para>計算量: ならしO(a(n))</para>
        /// </remarks>
        [MethodImpl(256)]
        public int Leader(int a)
        {
            Contract.Assert((uint)a < (uint)_ps.Length, reason: $"IndexOutOfRange: 0 <= {nameof(a)} && {nameof(a)} < n");
            if (_ps[a] < 0) return a;
            while (0 <= _ps[_ps[a]])
            {
                (a, _ps[a]) = (_ps[a], _ps[_ps[a]]);
            }
            return _ps[a];
        }


        /// <summary>
        /// 頂点 <paramref name="a"/> の属する連結成分のサイズを返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>&lt;n</para>
        /// <para>計算量: ならしO(a(n))</para>
        /// </remarks>
        [MethodImpl(256)]
        public int Size(int a)
        {
            Contract.Assert(0 <= a && a < _ps.Length, reason: $"IndexOutOfRange: 0 <= {nameof(a)} && {nameof(a)} < _n");
            return -_ps[Leader(a)];
        }

        /// <summary>
        /// グラフを連結成分に分け、その情報を返します。
        /// </summary>
        /// <para>計算量: O(n)</para>
        /// <returns>「一つの連結成分の頂点番号のリスト」のリスト。</returns>
        [MethodImpl(256)]
        public int[][] Groups()
        {
            int n = _ps.Length;
            int[] leaderBuf = new int[n];
            int[] id = new int[n];
            var resultList = new SimpleList<int[]>(n);
            for (int i = 0; i < leaderBuf.Length; i++)
            {
                leaderBuf[i] = Leader(i);
                if (i == leaderBuf[i])
                {
                    id[i] = resultList.Count;
                    resultList.Add(new int[-_ps[i]]);
                }
            }
            var result = resultList.ToArray();
            int[] ind = new int[result.Length];
            for (int i = 0; i < leaderBuf.Length; i++)
            {
                var leaderID = id[leaderBuf[i]];
                result[leaderID][ind[leaderID]] = i;
                ind[leaderID]++;
            }
            return result;
        }
    }
}
