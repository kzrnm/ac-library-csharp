namespace AtCoder
{
    /// <summary>
    /// 最大フロー問題 を解くライブラリ(int版)です。
    /// </summary>
#if GENERIC_MATH
    [System.Obsolete("Use generic math")]
#endif
    public class MfGraphInt : MfGraph<int, IntOperator> { public MfGraphInt(int n) : base(n) { } }

    /// <summary>
    /// 最大フロー問題 を解くライブラリ(long版)です。
    /// </summary>
#if GENERIC_MATH
    [System.Obsolete("Use generic math")]
#endif
    public class MfGraphLong : MfGraph<long, LongOperator> { public MfGraphLong(int n) : base(n) { } }
}
