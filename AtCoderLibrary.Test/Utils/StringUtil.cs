using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using FluentAssertions.Primitives;

namespace AtCoder.Test.Utils
{
    static class StringUtil
    {
        public static IEnumerable<string> ToLines(this string str)
        {
            using var sr = new StringReader(str);
            var line = sr.ReadLine();
            while (line != null)
            {
                yield return line;
                line = sr.ReadLine();
            }
        }

        public static void EqualLines(this StringAssertions assertions, string expected)
        {
            assertions.Subject.ToLines().Should().Equal(expected.ToLines());
        }
    }
}
