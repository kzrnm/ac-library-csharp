using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace AtCoder.Test.Utils
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ProblemTestCaseAttribute : Attribute
    {
        public ProblemTestCaseAttribute(string url)
        {
            throw new NotImplementedException();
        }
    }
}
