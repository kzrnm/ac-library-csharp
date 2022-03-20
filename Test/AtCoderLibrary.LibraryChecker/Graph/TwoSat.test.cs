using System;
using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers
{
    public class TwoSatTest
    {
        static void Main() { using var cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/two_sat
        static void Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            _ = cr.Ascii();
            _ = cr.Ascii();
            int n = cr;
            int m = cr;
            var twoSat = new TwoSat(n);
            for (int i = 0; i < m; i++)
            {
                int a = cr;
                int b = cr;
                _ = cr.Int();

                int a1 = Math.Abs(a) - 1;
                bool a2 = a >= 0;
                int b1 = Math.Abs(b) - 1;
                bool b2 = b >= 0;
                twoSat.AddClause(a1, a2, b1, b2);
            }
            if (twoSat.Satisfiable())
            {
                cw.WriteLine("s SATISFIABLE");
                cw.Write("v ");
                var res = new int[n + 1];
                var answer = twoSat.Answer();
                for (int i = 0; i < n; i++)
                {
                    if (answer[i])
                        res[i] = i + 1;
                    else
                        res[i] = -(i + 1);
                }
                cw.WriteLineJoin(res);
            }
            else
                cw.WriteLine("s UNSATISFIABLE");
        }
    }
}
