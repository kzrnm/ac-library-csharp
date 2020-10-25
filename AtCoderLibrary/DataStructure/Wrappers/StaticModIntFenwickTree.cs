namespace AtCoder
{
    /// <summary>
    /// 長さ N の配列に対し、
    /// <list type="bullet">
    /// <item>
    /// <description>要素の 1 点変更</description>
    /// </item>
    /// <item>
    /// <description>区間の要素の総和</description>
    /// </item>
    /// </list>
    /// <para>を O(log⁡N) で求めることが出来るデータ構造です。</para>
    /// </summary>
    public class StaticModIntFenwickTree<T> : FenwickTree<StaticModInt<T>, StaticModIntOperator<T>> where T : struct, IStaticMod { public StaticModIntFenwickTree(int n) : base(n) { } }
}
