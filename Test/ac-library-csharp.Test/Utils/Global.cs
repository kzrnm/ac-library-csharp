using Xunit;

namespace AtCoder
{
    public class Global
    {
#if CI
        public const bool IsCi = true;
        [Fact]
        public void CI() { }
#else
        public const bool IsCi = false;
        [Fact(Skip = "This is not in CI.")]
        public void CI() { }
#endif
    }
}

namespace System.Runtime.CompilerServices
{
    class IsExternalInit { }
}
