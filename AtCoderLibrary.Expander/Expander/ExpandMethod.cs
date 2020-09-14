namespace AtCoder
{
    public enum ExpandMethod
    {
        /// <summary>
        /// Write all embeded type(fast)
        /// </summary>
        All,
        /// <summary>
        /// Found type name by SyntaxTree(slow)
        /// </summary>
        NameSyntax,
        /// <summary>
        /// Found type name by Compilation(too slow)
        /// </summary>
        Strict,
    }
}
