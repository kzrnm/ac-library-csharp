using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace AtCoder
{
    public class LazySegtreeDebugViewTest
    {
        class WrapperView<T, F, TOp> where TOp : struct, ILazySegtreeOperator<T, F>
        {
            readonly object debugView;
            readonly PropertyInfo itemsProperty;
            public WrapperView(LazySegtree<T, F, TOp> s)
            {
                var type = typeof(LazySegtree<T, F, TOp>).GetNestedType("DebugView", BindingFlags.NonPublic)
                    .MakeGenericType(typeof(T), typeof(F), typeof(TOp));
                debugView = type.GetConstructor(new[] { s.GetType() }).Invoke(new object[] { s });
                itemsProperty = debugView.GetType().GetProperty("Items");
            }
            public LazySegtree<T, F, TOp>.DebugItem[] GetItems()
            {
                return (LazySegtree<T, F, TOp>.DebugItem[])itemsProperty.GetValue(debugView);
            }
        }
        static WrapperView<T, F, TOp> CreateWrapper<T, F, TOp>(LazySegtree<T, F, TOp> s) where TOp : struct, ILazySegtreeOperator<T, F> => new(s);
        static LazySegtree<int, int, MaxOp>.DebugItem CreateDebugItem(int l, int r, int value, int lazy = 0)
            => new(l, r, value, lazy);
        readonly struct MaxOp : ILazySegtreeOperator<int, int>
        {
            public int Identity => int.MinValue;
            public int FIdentity => 0;

            public int Composition(int nf, int cf) => nf + cf;
            public int Mapping(int f, int x) => x + f;
            public int Operate(int x, int y) => System.Math.Max(x, y);
        }

        [Fact]
        public void Empty()
        {
            var s = new LazySegtree<int, int, MaxOp>(0);
            var view = CreateWrapper(s);
            view.GetItems().Should().BeEmpty();
        }

        public static TheoryData Simple_Data = new TheoryData<int, LazySegtree<int, int, MaxOp>.DebugItem[]>
        {
            {
                1,
                new[]
                {
                    CreateDebugItem(0, 1, 0),
                }
            },
            {
                2,
                new[]
                {
                    CreateDebugItem(0, 1, 0),
                    CreateDebugItem(1, 2, 1),
                    CreateDebugItem(0, 2, 1),
                }
            },
            {
                3,
                new[]
                {
                    CreateDebugItem(0, 1, 0),
                    CreateDebugItem(1, 2, 1),
                    CreateDebugItem(2, 3, 2),
                    CreateDebugItem(0, 2, 1),
                    CreateDebugItem(2, 4, 2),
                    CreateDebugItem(0, 4, 2),
                }
            },
            {
                4,
                new[]
                {
                    CreateDebugItem(0, 1, 0),
                    CreateDebugItem(1, 2, 1),
                    CreateDebugItem(2, 3, 2),
                    CreateDebugItem(3, 4, 3),
                    CreateDebugItem(0, 2, 1),
                    CreateDebugItem(2, 4, 3),
                    CreateDebugItem(0, 4, 3),
                }
            },
            {
                5,
                new[]
                {
                    CreateDebugItem(0, 1, 0),
                    CreateDebugItem(1, 2, 1),
                    CreateDebugItem(2, 3, 2),
                    CreateDebugItem(3, 4, 3),
                    CreateDebugItem(4, 5, 4),
                    CreateDebugItem(0, 2, 1),
                    CreateDebugItem(2, 4, 3),
                    CreateDebugItem(4, 6, 4),
                    CreateDebugItem(0, 4, 3),
                    CreateDebugItem(4, 8, 4),
                    CreateDebugItem(0, 8, 4),
                }
            },
            {
                6,
                new[]
                {
                    CreateDebugItem(0, 1, 0),
                    CreateDebugItem(1, 2, 1),
                    CreateDebugItem(2, 3, 2),
                    CreateDebugItem(3, 4, 3),
                    CreateDebugItem(4, 5, 4),
                    CreateDebugItem(5, 6, 5),
                    CreateDebugItem(0, 2, 1),
                    CreateDebugItem(2, 4, 3),
                    CreateDebugItem(4, 6, 5),
                    CreateDebugItem(0, 4, 3),
                    CreateDebugItem(4, 8, 5),
                    CreateDebugItem(0, 8, 5),
                }
            },
        };

        [Theory]
        [MemberData(nameof(Simple_Data))]
        public void Simple(int size, object expectedObj)
        {
            var expected = (LazySegtree<int, int, MaxOp>.DebugItem[])expectedObj;
            var array = Enumerable.Range(0, size).ToArray();
            var s = new LazySegtree<int, int, MaxOp>(array);

            var view = CreateWrapper(s);
            var items = view.GetItems();
            items.Should().Equal(expected);
            foreach (var item in items)
                item.Value.Should().Be(System.Math.Min(item.R, size) - 1);
        }

        [Fact]
        public void Lazy()
        {
            var array = Enumerable.Range(0, 5).ToArray();
            var s = new LazySegtree<int, int, MaxOp>(array);
            var view = CreateWrapper(s);

            s.Apply(1, 4);
            view.GetItems().Should().Equal(new LazySegtree<int, int, MaxOp>.DebugItem[]
            {
                    CreateDebugItem(0, 1, 0),
                    CreateDebugItem(1, 2, 5),
                    CreateDebugItem(2, 3, 2),
                    CreateDebugItem(3, 4, 3),
                    CreateDebugItem(4, 5, 4),
                    CreateDebugItem(0, 2, 5),
                    CreateDebugItem(2, 4, 3),
                    CreateDebugItem(4, 6, 4),
                    CreateDebugItem(0, 4, 5),
                    CreateDebugItem(4, 8, 4),
                    CreateDebugItem(0, 8, 5),
            });

            s.Apply(2, 5, -2);
            view.GetItems().Should().Equal(new LazySegtree<int, int, MaxOp>.DebugItem[]
            {
                    CreateDebugItem(0, 1, 0),
                    CreateDebugItem(1, 2, 5),
                    CreateDebugItem(2, 3, 2),
                    CreateDebugItem(3, 4, 3),
                    CreateDebugItem(4, 5, 2),
                    CreateDebugItem(0, 2, 5),
                    CreateDebugItem(2, 4, 1, -2),
                    CreateDebugItem(4, 6, 2),
                    CreateDebugItem(0, 4, 5),
                    CreateDebugItem(4, 8, 2),
                    CreateDebugItem(0, 8, 5),
            });

            s.Apply(3, 5, 3);
            view.GetItems().Should().Equal(new LazySegtree<int, int, MaxOp>.DebugItem[]
            {
                    CreateDebugItem(0, 1, 0),
                    CreateDebugItem(1, 2, 5),
                    CreateDebugItem(2, 3, 0),
                    CreateDebugItem(3, 4, 4),
                    CreateDebugItem(4, 5, 5),
                    CreateDebugItem(0, 2, 5),
                    CreateDebugItem(2, 4, 4),
                    CreateDebugItem(4, 6, 5),
                    CreateDebugItem(0, 4, 5),
                    CreateDebugItem(4, 8, 5),
                    CreateDebugItem(0, 8, 5),
            });

            s.Apply(0, 5, 1);
            view.GetItems().Should().Equal(new LazySegtree<int, int, MaxOp>.DebugItem[]
            {
                    CreateDebugItem(0, 1, 0),
                    CreateDebugItem(1, 2, 5),
                    CreateDebugItem(2, 3, 0),
                    CreateDebugItem(3, 4, 4),
                    CreateDebugItem(4, 5, 6),
                    CreateDebugItem(0, 2, 5),
                    CreateDebugItem(2, 4, 4),
                    CreateDebugItem(4, 6, 6),
                    CreateDebugItem(0, 4, 6, 1),
                    CreateDebugItem(4, 8, 6),
                    CreateDebugItem(0, 8, 6),
            });

            s[2] = 5;
            view.GetItems().Should().Equal(new LazySegtree<int, int, MaxOp>.DebugItem[]
            {
                    CreateDebugItem(0, 1, 0),
                    CreateDebugItem(1, 2, 5),
                    CreateDebugItem(2, 3, 5),
                    CreateDebugItem(3, 4, 5),
                    CreateDebugItem(4, 5, 6),
                    CreateDebugItem(0, 2, 6, 1),
                    CreateDebugItem(2, 4, 5),
                    CreateDebugItem(4, 6, 6),
                    CreateDebugItem(0, 4, 6),
                    CreateDebugItem(4, 8, 6),
                    CreateDebugItem(0, 8, 6),
            });
        }
    }
}
