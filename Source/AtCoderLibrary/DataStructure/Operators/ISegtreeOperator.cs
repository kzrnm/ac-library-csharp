namespace AtCoder
{
    /// <summary>
    /// モノイドを定義するインターフェイスです。
    /// </summary>
    /// <typeparam name="T">操作を行う型。</typeparam>
    [IsOperator]
    public interface ISegtreeOperator<T>
    {
        /// <summary>
        /// Operate(x, <paramref name="Identity"/>) = xを満たす単位元。
        /// </summary>
        T Identity { get; }
        /// <summary>
        /// 結合律 Operate(Operate(a, b), c) = Operate(a, Operate(b, c)) を満たす写像。
        /// </summary>
        T Operate(T x, T y);
    }
}
