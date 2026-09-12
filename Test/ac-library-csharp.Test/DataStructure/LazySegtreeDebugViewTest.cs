using System.Linq;
using System.Reflection;
using Shouldly;
using Xunit;
using Xunit.Sdk;

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
                debugView = type.GetConstructor([s.GetType()]).Invoke([s]);
                itemsProperty = debugView.GetType().GetProperty("Items");
            }
            public LazySegtree<T, F, TOp>.DebugItem[] GetItems()
            {
                return (LazySegtree<T, F, TOp>.DebugItem[])itemsProperty.GetValue(debugView);
            }
        }
        static WrapperView<T, F, TOp> CreateWrapper<T, F, TOp>(LazySegtree<T, F, TOp> s) where TOp : struct, ILazySegtreeOperator<T, F> => new(s);


        public class DebugItem : IXunitSerializable
        {
            public int L;
            public int R;
            public int Value;
            public int Lazy;

            public void Deserialize(IXunitSerializationInfo info)
            {
                L = info.GetValue<int>(nameof(L));
                R = info.GetValue<int>(nameof(R));
                Value = info.GetValue<int>(nameof(Value));
                Lazy = info.GetValue<int>(nameof(Lazy));
            }
            public void Serialize(IXunitSerializationInfo info)
            {
                info.AddValue(nameof(L), L);
                info.AddValue(nameof(R), R);
                info.AddValue(nameof(Value), Value);
                info.AddValue(nameof(Lazy), Lazy);
            }
        }
        static DebugItem CreateDebugItem(int l, int r, int value, int lazy = 0) => new() { L = l, R = r, Value = value, Lazy = lazy };


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
            view.GetItems().ShouldBeEmpty();
        }

        public static TheoryData<int, DebugItem[]> Simple_Data() => new()
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
        public void Simple(int size, DebugItem[] expectedObj)
        {
            var expected = expectedObj.Select(t => new LazySegtree<int, int, MaxOp>.DebugItem(t.L, t.R, t.Value, t.Lazy)).ToArray();
            var array = Enumerable.Range(0, size).ToArray();
            var s = new LazySegtree<int, int, MaxOp>(array);

            var view = CreateWrapper(s);
            var items = view.GetItems();
            items.ShouldBe(expected);
            foreach (var item in items)
                item.Value.ShouldBe(System.Math.Min(item.R, size) - 1);
        }

        [Fact]
        public void Lazy()
        {
            static LazySegtree<int, int, MaxOp>.DebugItem CreateDebugItem(int l, int r, int val, int lazy = 0)
                => new(l, r, val, lazy);
            var array = Enumerable.Range(0, 5).ToArray();
            var s = new LazySegtree<int, int, MaxOp>(array);
            var view = CreateWrapper(s);

            s.Apply(1, 4);
            view.GetItems().ShouldBe([
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
            ]);

            s.Apply(2, 5, -2);
            view.GetItems().ShouldBe([
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
            ]);

            s.Apply(3, 5, 3);
            view.GetItems().ShouldBe([
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
            ]);

            s.Apply(0, 5, 1);
            view.GetItems().ShouldBe([
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
            ]);

            s[2] = 5;
            view.GetItems().ShouldBe([
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
            ]);
        }
    }
}
