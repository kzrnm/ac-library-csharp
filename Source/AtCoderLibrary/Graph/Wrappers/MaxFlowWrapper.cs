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
}
