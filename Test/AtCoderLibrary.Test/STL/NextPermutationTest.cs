using System;
using FluentAssertions;
using Xunit;

namespace AtCoder
{
    public class NextPermutationTest
    {
        public static TheoryData IntTestData = new TheoryData<int[], int[][]>
        {
            {
                new int[]{ 1,2,3,4 },
                new int[][] {
                    new int[]{ 1,2,3,4 },
                    new int[]{ 1,2,4,3 },
                    new int[]{ 1,3,2,4 },
                    new int[]{ 1,3,4,2 },
                    new int[]{ 1,4,2,3 },
                    new int[]{ 1,4,3,2 },

                    new int[]{ 2,1,3,4 },
                    new int[]{ 2,1,4,3 },
                    new int[]{ 2,3,1,4 },
                    new int[]{ 2,3,4,1 },
                    new int[]{ 2,4,1,3 },
                    new int[]{ 2,4,3,1 },

                    new int[]{ 3,1,2,4 },
                    new int[]{ 3,1,4,2 },
                    new int[]{ 3,2,1,4 },
                    new int[]{ 3,2,4,1 },
                    new int[]{ 3,4,1,2 },
                    new int[]{ 3,4,2,1 },

                    new int[]{ 4,1,2,3 },
                    new int[]{ 4,1,3,2 },
                    new int[]{ 4,2,1,3 },
                    new int[]{ 4,2,3,1 },
                    new int[]{ 4,3,1,2 },
                    new int[]{ 4,3,2,1 },
                }
            },
            {
                new int[]{ 3,1,4,2 },
                new int[][] {
                    new int[]{ 3,1,4,2 },
                    new int[]{ 3,2,1,4 },
                    new int[]{ 3,2,4,1 },
                    new int[]{ 3,4,1,2 },
                    new int[]{ 3,4,2,1 },

                    new int[]{ 4,1,2,3 },
                    new int[]{ 4,1,3,2 },
                    new int[]{ 4,2,1,3 },
                    new int[]{ 4,2,3,1 },
                    new int[]{ 4,3,1,2 },
                    new int[]{ 4,3,2,1 },
                }
            },
            {
                new int[]{ 1,2,2 },
                new int[][] {
                    new int[]{ 1,2,2 },
                    new int[]{ 2,1,2 },
                    new int[]{ 2,2,1 },
                }
            },
            {
                new int[]{ 1,1,2,2 },
                new int[][] {
                    new int[]{ 1,1,2,2 },
                    new int[]{ 1,2,1,2 },
                    new int[]{ 1,2,2,1 },
                    new int[]{ 2,1,1,2 },
                    new int[]{ 2,1,2,1 },
                    new int[]{ 2,2,1,1 },
                }
            },
        };
        [Theory]
        [MemberData(nameof(IntTestData))]
        public void Int(int[] input, int[][] expected) => Generic(input, expected);

        public static TheoryData LongTestData = new TheoryData<long[], long[][]>
        {
            {
                new long[]{ 1,2,3,4 },
                new long[][] {
                    new long[]{ 1,2,3,4 },
                    new long[]{ 1,2,4,3 },
                    new long[]{ 1,3,2,4 },
                    new long[]{ 1,3,4,2 },
                    new long[]{ 1,4,2,3 },
                    new long[]{ 1,4,3,2 },

                    new long[]{ 2,1,3,4 },
                    new long[]{ 2,1,4,3 },
                    new long[]{ 2,3,1,4 },
                    new long[]{ 2,3,4,1 },
                    new long[]{ 2,4,1,3 },
                    new long[]{ 2,4,3,1 },

                    new long[]{ 3,1,2,4 },
                    new long[]{ 3,1,4,2 },
                    new long[]{ 3,2,1,4 },
                    new long[]{ 3,2,4,1 },
                    new long[]{ 3,4,1,2 },
                    new long[]{ 3,4,2,1 },

                    new long[]{ 4,1,2,3 },
                    new long[]{ 4,1,3,2 },
                    new long[]{ 4,2,1,3 },
                    new long[]{ 4,2,3,1 },
                    new long[]{ 4,3,1,2 },
                    new long[]{ 4,3,2,1 },
                }
            },
            {
                new long[]{ 3,1,4,2 },
                new long[][] {
                    new long[]{ 3,1,4,2 },
                    new long[]{ 3,2,1,4 },
                    new long[]{ 3,2,4,1 },
                    new long[]{ 3,4,1,2 },
                    new long[]{ 3,4,2,1 },

                    new long[]{ 4,1,2,3 },
                    new long[]{ 4,1,3,2 },
                    new long[]{ 4,2,1,3 },
                    new long[]{ 4,2,3,1 },
                    new long[]{ 4,3,1,2 },
                    new long[]{ 4,3,2,1 },
                }
            },
        };
        [Theory]
        [MemberData(nameof(LongTestData))]
        public void Long(long[] input, long[][] expected) => Generic(input, expected);

        public static TheoryData CharTestData = new TheoryData<string[], string[][]>
        {
            {
                new string[]{ "ax","b","ca" },
                new string[][] {
                    new string[]{ "ax","b","ca" },
                    new string[]{ "ax","ca","b" },
                    new string[]{ "b","ax","ca" },
                    new string[]{ "b","ca","ax" },
                    new string[]{ "ca","ax","b" },
                    new string[]{ "ca","b","ax" },
                }
            },
            {
                new string[]{ "b","ax","ca" },
                new string[][] {
                    new string[]{ "b","ax","ca" },
                    new string[]{ "b","ca","ax" },
                    new string[]{ "ca","ax","b" },
                    new string[]{ "ca","b","ax" },
                }
            },
        };
        [Theory]
        [MemberData(nameof(CharTestData))]
        public void Char(string[] input, string[][] expected) => Generic(input, expected);

        private void Generic<T>(T[] input, T[][] expected) where T : IComparable<T>
        {
            var e = StlFunction.NextPermutation(input).GetEnumerator();
            for (int i = 0; i < expected.Length; i++)
            {
                e.MoveNext().Should().BeTrue();
                e.Current.Should().Equal(expected[i]);
            }
            e.MoveNext().Should().BeFalse();
        }
    }
}
