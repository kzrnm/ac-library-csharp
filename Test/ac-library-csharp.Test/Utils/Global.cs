namespace AtCoder
{
    internal class Global
    {
        public const bool IsCi
#if CI
            = true;
#else
            = false;
#endif
    }
}
