namespace AtCoder
{
    /// <summary>
    /// 最大フロー問題 を解くライブラリ(int版)です。
    /// </summary>
    [System.Obsolete("Use generic math")]
    public class MfGraphInt : MfGraph<int, IntOperator> { public MfGraphInt(int n) : base(n) { } }

    /// <summary>
    /// 最大フロー問題 を解くライブラリ(long版)です。
    /// </summary>
    [System.Obsolete("Use generic math")]
    public class MfGraphLong : MfGraph<long, LongOperator> { public MfGraphLong(int n) : base(n) { } }
}
