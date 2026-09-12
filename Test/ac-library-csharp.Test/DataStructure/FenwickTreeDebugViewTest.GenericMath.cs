using System.Linq;
using System.Numerics;
using System.Reflection;
using Shouldly;
using Xunit;
using Xunit.Sdk;

namespace AtCoder
{
    public class FenwickTreeDebugViewGenericMathTest
    {
        public class DebugItem : IXunitSerializable
        {
            public long Value;
            public long Sum;

            internal FenwickTree<long>.DebugItem ToObject() => new(Value, Sum);
            public void Deserialize(IXunitSerializationInfo info)
            {
                Sum = info.GetValue<long>(nameof(Sum));
                Value = info.GetValue<long>(nameof(Value));
            }
            public void Serialize(IXunitSerializationInfo info)
            {
                info.AddValue(nameof(Sum), Sum);
                info.AddValue(nameof(Value), Value);
            }
        }

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

        [Fact]
        public void Empty()
        {
            var s = new FenwickTree<long>(0);
            var view = CreateWrapper(s);
            view.GetItems().ShouldBeEmpty();
        }

        public static TheoryData<int, DebugItem[]> Simple_Data => new()
        {
            {
                1,
                new[]
                {
                    new DebugItem{Value=0b000001, Sum=0b000001 },
                }
            },
            {
                2,
                new[]
                {
                    new DebugItem{Value=0b000001, Sum=0b000001},
                    new DebugItem{Value=0b000010, Sum=0b000011},
                }
            },
            {
                3,
                new[]
                {
                    new DebugItem{Value=0b000001, Sum=0b000001},
                    new DebugItem{Value=0b000010, Sum=0b000011},
                    new DebugItem{Value=0b000100, Sum=0b000111},
                }
            },
            {
                4,
                new[]
                {
                    new DebugItem{Value=0b000001, Sum=0b000001},
                    new DebugItem{Value=0b000010, Sum=0b000011},
                    new DebugItem{Value=0b000100, Sum=0b000111},
                    new DebugItem{Value=0b001000, Sum=0b001111},
                }
            },
            {
                5,
                new[]
                {
                    new DebugItem{Value=0b000001, Sum=0b000001},
                    new DebugItem{Value=0b000010, Sum=0b000011},
                    new DebugItem{Value=0b000100, Sum=0b000111},
                    new DebugItem{Value=0b001000, Sum=0b001111},
                    new DebugItem{Value=0b010000, Sum=0b011111},
                }
            },
            {
                6,
                new[]
                {
                    new DebugItem{Value=0b000001, Sum=0b000001},
                    new DebugItem{Value=0b000010, Sum=0b000011},
                    new DebugItem{Value=0b000100, Sum=0b000111},
                    new DebugItem{Value=0b001000, Sum=0b001111},
                    new DebugItem{Value=0b010000, Sum=0b011111},
                    new DebugItem{Value=0b100000, Sum=0b111111},
                }
            },
        };

        [Theory]
        [MemberData(nameof(Simple_Data))]
        public void Simple(int size, DebugItem[] expectedObj)
        {
            var expected = expectedObj.Select(t => t.ToObject()).ToArray();
            var array = Enumerable.Range(0, size).Select(i => $"{(char)('a' + i)}").ToArray();
            var fw = new FenwickTree<long>(size);
            for (int i = 0; i < size; i++)
                fw.Add(i, 1L << i);

            var view = CreateWrapper(fw);
            var items = view.GetItems();
            items.ShouldBe(expected);
        }
    }
}
