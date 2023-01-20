﻿using System;
using System.Linq;
using FluentAssertions;
using MersenneTwister;
using Xunit;

namespace AtCoder
{
    public class LazySegtree_StressTest
    {
        private class TimeManager
        {
            int[] v;
            public TimeManager(int n) { v = Enumerable.Repeat(-1, n).ToArray(); }
            public void Action(int l, int r, int time)
            {
                for (int i = l; i < r; i++)
                {
                    v[i] = time;
                }
            }
            public int Prod(int l, int r)
            {
                int res = -1;
                for (int i = l; i < r; i++)
                {
                    res = Math.Max(res, v[i]);
                }
                return res;
            }
        }

        private readonly struct Op : ILazySegtreeOperator<(int l, int r, int time), int>
        {
            public (int l, int r, int time) Identity => (-1, -1, -1);

            public int FIdentity => -1;

            public int Composition(int l, int r)
            {
                if (l == -1) return r;
                if (r == -1) return l;
                l.Should().BeGreaterThan(r);
                return l;
            }

            public (int l, int r, int time) Mapping(int l, (int l, int r, int time) r)
            {
                if (l == -1) return r;
                r.time.Should().BeLessThan(l);
                return (r.l, r.r, l);
            }

            public (int l, int r, int time) Operate((int l, int r, int time) l, (int l, int r, int time) r)
            {
                if (l.l == -1) return r;
                if (r.l == -1) return l;
                (l.r == r.l).Should().BeTrue();
                return (l.l, r.r, Math.Max(l.time, r.time));
            }
        }

        [Fact]
        public void NaiveTest()
        {
            var mt = MTRandom.Create();
            for (int n = 1; n <= 30; n++)
            {
                for (int ph = 0; ph < 10; ph++)
                {
                    var seg0 = new LazySegtree<(int l, int r, int time), int, Op>(n);
                    var tm = new TimeManager(n);
                    for (int i = 0; i < n; i++)
                    {
                        seg0[i] = (i, i + 1, -1);
                    }
                    int now = 0;
                    for (int q = 0; q < 3000; q++)
                    {
                        int ty = mt.Next(0, 4);
                        var (l, r) = mt.NextPair(0, n + 1);
                        if (ty == 0)
                        {
                            seg0.Prod(l, r).Should().Be((l, r, tm.Prod(l, r)));
                        }
                        else if (ty == 1)
                        {
                            seg0[l].Should().Be((l, l + 1, tm.Prod(l, l + 1)));
                        }
                        else if (ty == 2)
                        {
                            now++;
                            seg0.Apply(l, r, now);
                            tm.Action(l, r, now);
                        }
                        else if (ty == 3)
                        {
                            now++;
                            seg0.Apply(l, now);
                            tm.Action(l, l + 1, now);
                        }
                        else
                            Assert.True(false);
                    }
                }
            }
        }

        [Fact]
        public void MaxRightTest()
        {
            var mt = MTRandom.Create();
            for (int n = 1; n <= 30; n++)
            {
                for (int ph = 0; ph < 10; ph++)
                {
                    var seg0 = new LazySegtree<(int l, int r, int time), int, Op>(n);
                    var tm = new TimeManager(n);
                    for (int i = 0; i < n; i++)
                    {
                        seg0[i] = (i, i + 1, -1);
                    }
                    int now = 0;
                    for (int q = 0; q < 1000; q++)
                    {
                        int ty = mt.Next(0, 3);
                        var (l, r) = mt.NextPair(0, n + 1);
                        if (ty == 0)
                        {
                            seg0.MaxRight(l, s =>
                            {
                                if (s.l == -1) return true;
                                (s.l == l).Should().BeTrue();
                                (s.time == tm.Prod(l, s.r)).Should().BeTrue();
                                return s.r <= r;
                            }).Should().Be(r);
                        }
                        else
                        {
                            now++;
                            seg0.Apply(l, r, now);
                            tm.Action(l, r, now);
                        }
                    }
                }
            }
        }

        [Fact]
        public void MinLeftTest()
        {
            var mt = MTRandom.Create();
            for (int n = 1; n <= 30; n++)
            {
                for (int ph = 0; ph < 10; ph++)
                {
                    var seg0 = new LazySegtree<(int l, int r, int time), int, Op>(n);
                    var tm = new TimeManager(n);
                    for (int i = 0; i < n; i++)
                    {
                        seg0[i] = (i, i + 1, -1);
                    }
                    int now = 0; int ty = mt.Next(0, 3);
                    var (l, r) = mt.NextPair(0, n + 1);
                    if (ty == 0)
                    {
                        seg0.MinLeft(r, s =>
                        {
                            if (s.l == -1) return true;
                            (s.r == r).Should().BeTrue();
                            (s.time == tm.Prod(s.l, r)).Should().BeTrue();
                            return l <= s.l;
                        }).Should().Be(l);
                    }
                    else
                    {
                        now++;
                        seg0.Apply(l, r, now);
                        tm.Action(l, r, now);
                    }
                }
            }
        }
    }
}
