using System;
using Shouldly;

namespace AtCoder.DataStructure.Native
{
    internal readonly struct MonoidOperator : ISegtreeOperator<string>
    {
        public string Identity => "$";
        public string Operate(string a, string b)
        {
            if (!(a == "$" || b == "$" || StringComparer.Ordinal.Compare(a, b) <= 0)) throw new Exception();
            if (a == "$") return b;
            if (b == "$") return a;
            return a + b;
        }
    }
    internal class SegtreeNaive
    {
        private static readonly MonoidOperator op = default;
        int n;
        string[] d;

        public SegtreeNaive(int _n)
        {
            n = _n;
            d = new string[n];
            Array.Fill(d, op.Identity);
        }
        public string this[int p]
        {
            set => d[p] = value;
            get => d[p];
        }

        public string Prod(int l, int r)
        {
            var sum = op.Identity;
            for (int i = l; i < r; i++)
            {
                sum = op.Operate(sum, d[i]);
            }
            return sum;
        }
        public string AllProd => Prod(0, n);
        public int MaxRight(int l, Predicate<string> f)
        {
            var sum = op.Identity;
            f(sum).ShouldBeTrue();
            for (int i = l; i < n; i++)
            {
                sum = op.Operate(sum, d[i]);
                if (!f(sum)) return i;
            }
            return n;
        }
        public int MinLeft(int r, Predicate<string> f)
        {
            var sum = op.Identity;
            f(sum).ShouldBeTrue();
            for (int i = r - 1; i >= 0; i--)
            {
                sum = op.Operate(d[i], sum);
                if (!f(sum)) return i + 1;
            }
            return 0;
        }
    }
}
