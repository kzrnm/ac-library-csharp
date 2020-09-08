using System;
using System.Collections.Generic;
using System.Text;

namespace AtCoder.Internal
{
    public static partial class InternalMath
    {
        public static long SafeMod(long x, long m)
        {
            x %= m;
            if (x < 0) x += m;
            return x;
        }
    }
}
