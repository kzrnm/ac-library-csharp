using System;
using System.Collections.Generic;
using System.Text;

namespace AtCoder.Test
{
    interface IValidator
    {
        bool IsValid(string input, string answer, string output);
    }
}
