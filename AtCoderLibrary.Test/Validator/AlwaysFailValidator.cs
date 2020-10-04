using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace AtCoder.Test
{
    class AlwaysFailValidator : IValidator
    {
        public bool IsValid(string input, string answer, string output) => false;
    }
}
