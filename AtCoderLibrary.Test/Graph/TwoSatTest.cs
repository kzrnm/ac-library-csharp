using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using MersenneTwister;
using Xunit;

namespace AtCoder
{
    public class TwoSatTest
    {
        [Fact]
        public void Empty()
        {
            var ts1 = new TwoSat(0);
            ts1.Satisfiable().Should().BeTrue();
            ts1.Answer().Should().Equal(Array.Empty<bool>());
        }
        [Fact]
        public void One()
        {
            {
                var ts = new TwoSat(1);
                ts.AddClause(0, true, 0, true);
                ts.AddClause(0, false, 0, false);
                ts.Satisfiable().Should().BeFalse();
            }
            {
                var ts = new TwoSat(1);
                ts.AddClause(0, true, 0, true);
                ts.Satisfiable().Should().BeTrue();
                ts.Answer().Should().Equal(true);
            }
            {
                var ts = new TwoSat(1);
                ts.AddClause(0, false, 0, false);
                ts.Satisfiable().Should().BeTrue();
                ts.Answer().Should().Equal(false);
            }
        }
        [Fact]
        public void StressOK()
        {
            var mt = MTRandom.Create();
            for (int phase = 0; phase < 10000; phase++)
            {
                int n = mt.Next(1, 21);
                int m = mt.Next(1, 101);
                var expect = new bool[n];
                for (int i = 0; i < n; i++)
                {
                    expect[i] = mt.NextBool();
                }
                var ts = new TwoSat(n);
                var xs = new int[m];
                var ys = new int[m];
                var types = new int[m];
                for (int i = 0; i < m; i++)
                {
                    int x = mt.Next(0, n);
                    int y = mt.Next(0, n);
                    int type = mt.Next(0, 3);
                    xs[i] = x;
                    ys[i] = y;
                    types[i] = type;
                    if (type == 0)
                    {
                        ts.AddClause(x, expect[x], y, expect[y]);
                    }
                    else if (type == 1)
                    {
                        ts.AddClause(x, !expect[x], y, expect[y]);
                    }
                    else
                    {
                        ts.AddClause(x, expect[x], y, !expect[y]);
                    }
                }
                ts.Satisfiable().Should().BeTrue();
                var actual = ts.Answer();
                for (int i = 0; i < m; i++)
                {
                    int x = xs[i], y = ys[i], type = types[i];
                    if (type == 0)
                    {
                        (actual[x] == expect[x] || actual[y] == expect[y]).Should().BeTrue();
                    }
                    else if (type == 1)
                    {
                        (actual[x] != expect[x] || actual[y] == expect[y]).Should().BeTrue();
                    }
                    else
                    {
                        (actual[x] == expect[x] || actual[y] != expect[y]).Should().BeTrue();
                    }
                }
            }
        }
    }
}
