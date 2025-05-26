using System.Linq;
using System.Reflection;
using AtCoder.Operators;
using Shouldly;
using Xunit;

namespace AtCoder
{
    public class FenwickTreeDebugViewTest
    {
#pragma warning disable CS0618
        class WrapperView<T, TOp> where TOp : struct, IAdditionOperator<T>, ISubtractOperator<T>
        {
            readonly object debugView;
            readonly PropertyInfo itemsProperty;
            public WrapperView(FenwickTree<T, TOp> fw)
            {
                var type = typeof(FenwickTree<T, TOp>).GetNestedType("DebugView", BindingFlags.NonPublic)
                    .MakeGenericType(typeof(T), typeof(TOp));
                debugView = type.GetConstructor([fw.GetType()]).Invoke([fw]);
                itemsProperty = debugView.GetType().GetProperty("Items");
            }
            public FenwickTree<T, TOp>.DebugItem[] GetItems()
            {
                return (FenwickTree<T, TOp>.DebugItem[])itemsProperty.GetValue(debugView);
            }
        }
        static WrapperView<T, TOp> CreateWrapper<T, TOp>(FenwickTree<T, TOp> f) where TOp : struct, IAdditionOperator<T>, ISubtractOperator<T> => new(f);
        static LongFenwickTree.DebugItem CreateDebugItem(long val, long sum) => new(val, sum);

        [Fact]
        public void Empty()
        {
            var s = new LongFenwickTree(0);
            var view = CreateWrapper(s);
            view.GetItems().ShouldBeEmpty();
        }

        public static TheoryData Simple_Data => new TheoryData<int, LongFenwickTree.DebugItem[]>
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
            var expected = (LongFenwickTree.DebugItem[])expectedObj;
            var array = Enumerable.Range(0, size).Select(i => $"{(char)('a' + i)}").ToArray();
            var fw = new LongFenwickTree(size);
            for (int i = 0; i < size; i++)
                fw.Add(i, 1L << i);

            var view = CreateWrapper(fw);
            var items = view.GetItems();
            items.ShouldBe(expected);
        }
    }
}
