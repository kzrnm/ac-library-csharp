using System.Linq;
using System.Reflection;
using AtCoder.DataStructure.Native;
using FluentAssertions;
using Xunit;

namespace AtCoder
{
    public class SegtreeDebugViewTest
    {
        class WrapperView<T, TOp> where TOp : struct, ISegtreeOperator<T>
        {
            readonly object debugView;
            readonly PropertyInfo itemsProperty;
            public WrapperView(Segtree<T, TOp> s)
            {
                var type = typeof(Segtree<T, TOp>).GetNestedType("DebugView", BindingFlags.NonPublic)
                    .MakeGenericType(typeof(T), typeof(TOp));
                debugView = type.GetConstructor([s.GetType()]).Invoke([s]);
                itemsProperty = debugView.GetType().GetProperty("Items");
            }
            public Segtree<T, TOp>.DebugItem[] GetItems()
            {
                return (Segtree<T, TOp>.DebugItem[])itemsProperty.GetValue(debugView);
            }
        }
        static WrapperView<T, TOp> CreateWrapper<T, TOp>(Segtree<T, TOp> s) where TOp : struct, ISegtreeOperator<T> => new(s);
        static Segtree<string, MonoidOperator>.DebugItem CreateDebugItem(int l, int r, string s) => new(l, r, s);

        [Fact]
        public void Empty()
        {
            var s = new Segtree<string, MonoidOperator>(0);
            var view = CreateWrapper(s);
            view.GetItems().Should().BeEmpty();
        }

        public static TheoryData Simple_Data => new TheoryData<int, Segtree<string, MonoidOperator>.DebugItem[]>
        {
            {
                1,
                new[]
                {
                    CreateDebugItem(0, 1, "a"),
                }
            },
            {
                2,
                new[]
                {
                    CreateDebugItem(0, 1, "a"),
                    CreateDebugItem(1, 2, "b"),
                    CreateDebugItem(0, 2, "ab"),
                }
            },
            {
                3,
                new[]
                {
                    CreateDebugItem(0, 1, "a"),
                    CreateDebugItem(1, 2, "b"),
                    CreateDebugItem(2, 3, "c"),
                    CreateDebugItem(0, 2, "ab"),
                    CreateDebugItem(2, 4, "c"),
                    CreateDebugItem(0, 4, "abc"),
                }
            },
            {
                4,
                new[]
                {
                    CreateDebugItem(0, 1, "a"),
                    CreateDebugItem(1, 2, "b"),
                    CreateDebugItem(2, 3, "c"),
                    CreateDebugItem(3, 4, "d"),
                    CreateDebugItem(0, 2, "ab"),
                    CreateDebugItem(2, 4, "cd"),
                    CreateDebugItem(0, 4, "abcd"),
                }
            },
            {
                5,
                new[]
                {
                    CreateDebugItem(0, 1, "a"),
                    CreateDebugItem(1, 2, "b"),
                    CreateDebugItem(2, 3, "c"),
                    CreateDebugItem(3, 4, "d"),
                    CreateDebugItem(4, 5, "e"),
                    CreateDebugItem(0, 2, "ab"),
                    CreateDebugItem(2, 4, "cd"),
                    CreateDebugItem(4, 6, "e"),
                    CreateDebugItem(0, 4, "abcd"),
                    CreateDebugItem(4, 8, "e"),
                    CreateDebugItem(0, 8, "abcde"),
                }
            },
            {
                6,
                new[]
                {
                    CreateDebugItem(0, 1, "a"),
                    CreateDebugItem(1, 2, "b"),
                    CreateDebugItem(2, 3, "c"),
                    CreateDebugItem(3, 4, "d"),
                    CreateDebugItem(4, 5, "e"),
                    CreateDebugItem(5, 6, "f"),
                    CreateDebugItem(0, 2, "ab"),
                    CreateDebugItem(2, 4, "cd"),
                    CreateDebugItem(4, 6, "ef"),
                    CreateDebugItem(0, 4, "abcd"),
                    CreateDebugItem(4, 8, "ef"),
                    CreateDebugItem(0, 8, "abcdef"),
                }
            },
        };

        [Theory]
        [MemberData(nameof(Simple_Data))]
        public void Simple(int size, object expectedObj)
        {
            var expected = (Segtree<string, MonoidOperator>.DebugItem[])expectedObj;
            var array = Enumerable.Range(0, size).Select(i => $"{(char)('a' + i)}").ToArray();
            var s = new Segtree<string, MonoidOperator>(array);

            var naive = new SegtreeNaive(size);
            for (int i = 0; i < size; i++) naive[i] = array[i];

            var view = CreateWrapper(s);
            var items = view.GetItems();
            items.Should().Equal(expected);
            foreach (var item in items)
                item.Value.Should().Be(naive.Prod(item.L, System.Math.Min(item.R, size)));
        }
    }
}
