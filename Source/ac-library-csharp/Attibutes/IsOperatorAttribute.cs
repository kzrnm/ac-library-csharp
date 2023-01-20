using System;

namespace AtCoder
{
    /// <summary>
    /// Attribute of operator interface
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
    public sealed class IsOperatorAttribute : Attribute { }
}
