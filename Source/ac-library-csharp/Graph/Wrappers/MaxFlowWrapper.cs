namespace AtCoder
{
    /// <summary>
    /// 最大フロー問題 を解くライブラリ(int版)です。
    /// </summary>
    [System.Obsolete("Use generic math")]
    public class MfGraphInt(int n) : MfGraph<int, IntOperator>(n) { }

    /// <summary>
    /// 最大フロー問題 を解くライブラリ(long版)です。
    /// </summary>
    [System.Obsolete("Use generic math")]
    public class MfGraphLong(int n) : MfGraph<long, LongOperator>(n) { }
}
