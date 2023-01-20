﻿using AtCoder.Operators;

namespace AtCoder
{

    /// <summary>
    /// Minimum-cost flow problem を扱うライブラリ(int版)です。
    /// </summary>
#if GENERIC_MATH
    [System.Obsolete("Use generic math")]
#endif
    public class McfGraphInt
        : McfGraph<int, IntOperator, int, IntOperator, SameTypeCastOperator<int>>
    {
        public McfGraphInt(int n) : base(n) { }
    }

    /// <summary>
    /// Minimum-cost flow problem を扱うライブラリ(long版)です。
    /// </summary>
#if GENERIC_MATH
    [System.Obsolete("Use generic math")]
#endif
    public class McfGraphLong
       : McfGraph<long, LongOperator, long, LongOperator, SameTypeCastOperator<long>>
    {
        public McfGraphLong(int n) : base(n) { }
    }
}
