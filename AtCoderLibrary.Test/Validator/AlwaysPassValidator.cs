using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace AtCoder.Test
{
    class AlwaysPassValidator : IValidator
    {
        public bool IsValid(string input, string answer, string output) => true;
    }
}
