using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xunit;

namespace AtCoder
{
    static class DebugUtil
    {
        public static void SkipIfNotDebug()
        {
#if !DEBUG
            Skip.If(true, "not defined DEBUG symbol");
#endif
        }
    }
}
