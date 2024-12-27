using System.Linq;
using System.Numerics;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace AtCoder
{
    public class FenwickTreeDebugViewGenericMathTest
    {
#pragma warning disable CS0618
        class WrapperView<T> where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>, IAdditiveIdentity<T, T>
        {
            readonly object debugView;
            readonly PropertyInfo itemsProperty;
            public WrapperView(FenwickTree<T> fw)
            {
                var type = typeof(FenwickTree<T>).GetNestedType("DebugView", BindingFlags.NonPublic)
                    .MakeGenericType(typeof(T));
                debugView = type.GetConstructor([fw.GetType()]).Invoke([fw]);
                itemsProperty = debugView.GetType().GetProperty("Items");
            }
            public FenwickTree<T>.DebugItem[] GetItems()
            {
                return (FenwickTree<T>.DebugItem[])itemsProperty.GetValue(debugView);
            }
        }
        static WrapperView<T> CreateWrapper<T>(FenwickTree<T> f) where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>, IAdditiveIdentity<T, T> => new(f);
        static FenwickTree<long>.DebugItem CreateDebugItem(long val, long sum) => new(val, sum);

        [Fact]
        public void Empty()
        {
            var s = new FenwickTree<long>(0);
            var view = CreateWrapper(s);
            view.GetItems().Should().BeEmpty();
        }

        public static TheoryData Simple_Data => new TheoryData<int, FenwickTree<long>.DebugItem[]>
        {
            {
                1,
                new[]
                {
                    CreateDebugItem(0b000001, 0b000001),
                }
            },
            {
                2,
                new[]
                {
                    CreateDebugItem(0b000001, 0b000001),
                    CreateDebugItem(0b000010, 0b000011),
                }
            },
            {
                3,
                new[]
                {
                    CreateDebugItem(0b000001, 0b000001),
                    CreateDebugItem(0b000010, 0b000011),
                    CreateDebugItem(0b000100, 0b000111),
                }
            },
            {
                4,
                new[]
                {
                    CreateDebugItem(0b000001, 0b000001),
                    CreateDebugItem(0b000010, 0b000011),
                    CreateDebugItem(0b000100, 0b000111),
                    CreateDebugItem(0b001000, 0b001111),
                }
            },
            {
                5,
                new[]
                {
                    CreateDebugItem(0b000001, 0b000001),
                    CreateDebugItem(0b000010, 0b000011),
                    CreateDebugItem(0b000100, 0b000111),
                    CreateDebugItem(0b001000, 0b001111),
                    CreateDebugItem(0b010000, 0b011111),
                }
            },
            {
                6,
                new[]
                {
                    CreateDebugItem(0b000001, 0b000001),
                    CreateDebugItem(0b000010, 0b000011),
                    CreateDebugItem(0b000100, 0b000111),
                    CreateDebugItem(0b001000, 0b001111),
                    CreateDebugItem(0b010000, 0b011111),
                    CreateDebugItem(0b100000, 0b111111),
                }
            },
        };

        [Theory]
        [MemberData(nameof(Simple_Data))]
        public void Simple(int size, object expectedObj)
        {
            var expected = (FenwickTree<long>.DebugItem[])expectedObj;
            var array = Enumerable.Range(0, size).Select(i => $"{(char)('a' + i)}").ToArray();
            var fw = new FenwickTree<long>(size);
            for (int i = 0; i < size; i++)
                fw.Add(i, 1L << i);

            var view = CreateWrapper(fw);
            var items = view.GetItems();
            items.Should().Equal(expected);
        }
    }
}
