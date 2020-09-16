using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AtCoder.Test.Utils;
using Xunit;

namespace AtCoder.Test.Example
{
    public class MaxFlowTest
    {
        [ProblemTestCase(@"https://atcoder.jp/contests/practice2/tasks/practice2_d")]
        public void Practice(string input, string answer)
        {
            var reader = new StringReader(input);
            var writer = new StringWriter();

            Solver(reader, writer);

            Assert.True(new AlwaysFailValidator().IsValid(input, answer, writer.ToString()));

            static void Solver(TextReader reader, TextWriter writer)
            {
                int[] nm = reader.ReadLine().Split().Select(int.Parse).ToArray();
                (int n, int m) = (nm[0], nm[1]);

                char[][] grid = new char[n][];
                for (int i = 0; i < n; i++)
                {
                    grid[i] = reader.ReadLine().ToCharArray();
                }

                var g = new MFGraphInt(n * m + 2);
                int s = n * m, t = n * m + 1;

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        if (grid[i][j] == '#') continue;
                        int v = i * m + j;
                        if ((i + j) % 2 == 0)
                        {
                            g.AddEdge(s, v, 1);
                        }
                        else
                        {
                            g.AddEdge(v, t, 1);
                        }
                    }
                }

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        if ((i + j) % 2 != 0 || grid[i][j] == '#') continue;
                        int v0 = i * m + j;
                        if (i != 0 && grid[i - 1][j] == '.')
                        {
                            int v1 = (i - 1) * m + j;
                            g.AddEdge(v0, v1, 1);
                        }
                        if (j != 0 && grid[i][j - 1] == '.')
                        {
                            int v1 = i * m + (j - 1);
                            g.AddEdge(v0, v1, 1);
                        }
                        if (i + 1 < n && grid[i + 1][j] == '.')
                        {
                            int v1 = (i + 1) * m + j;
                            g.AddEdge(v0, v1, 1);
                        }
                        if (j + 1 < m && grid[i][j + 1] == '.')
                        {
                            int v1 = i * m + (j + 1);
                            g.AddEdge(v0, v1, 1);
                        }
                    }
                }

                writer.WriteLine(g.Flow(s, t));

                List<MFGraph<int, IntOperator>.Edge> edges = g.Edges();
                foreach (MFGraph<int, IntOperator>.Edge e in edges)
                {
                    if (e.From == s || e.To == t || e.Flow == 0) continue;
                    int i0 = e.From / m, j0 = e.From % m;
                    int i1 = e.To / m, j1 = e.To % m;

                    if (i0 == i1 + 1)
                    {
                        grid[i1][j1] = 'v';
                        grid[i0][j0] = '^';
                    }
                    else if (j0 == j1 + 1)
                    {
                        grid[i1][j1] = '>'; grid[i0][j0] = '<';
                    }
                    else if (i0 == i1 - 1)
                    {
                        grid[i0][j0] = 'v';
                        grid[i1][j1] = '^';
                    }
                    else
                    {
                        grid[i0][j0] = '>'; grid[i1][j1] = '<';
                    }
                }

                for (int i = 0; i < n; i++)
                {
                    writer.WriteLine(grid[i]);
                }
            }
        }
    }
}
