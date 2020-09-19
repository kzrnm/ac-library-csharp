using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace AtCoder.Test
{
    class TokenEqualityValidator : IValidator
    {
        public bool IsValid(string input, string answer, string output)
            => Tokenize(answer).SequenceEqual(Tokenize(output));

        private static IEnumerable<string> Tokenize(string s)
           => s is null ? Enumerable.Empty<string>() : s.Split().Where(x => x.Length != 0);
    }
}
