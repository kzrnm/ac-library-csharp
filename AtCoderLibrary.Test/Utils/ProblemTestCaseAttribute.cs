using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using NUnit.Compatibility;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace AtCoder.Test.Utils
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ProblemTestCaseAttribute : NUnitAttribute, ITestBuilder, IImplyFixture
    {
        private readonly NUnitTestCaseBuilder _builder = new NUnitTestCaseBuilder();

        public ProblemTestCaseAttribute(string url)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, NUnit.Framework.Internal.Test suite)
        {
            throw new NotImplementedException();
        }
    }
}
