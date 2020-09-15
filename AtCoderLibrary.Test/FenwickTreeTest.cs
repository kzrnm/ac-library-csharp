using AtCoder;
using NUnit.Framework;

namespace AtCoderLibrary.Test
{
    class FenwickTreeTest
    {

        [Test]
        // https://judge.yosupo.jp/problem/point_add_range_sum
        public void LibraryChecker()
        {
            var ft = new IntFenwickTree(5);
            ft.Add(0, 1);
            ft.Add(1, 2);
            ft.Add(2, 3);
            ft.Add(3, 4);
            ft.Add(4, 5);
            Assert.AreEqual(15, ft.Sum(0, 5));
            Assert.AreEqual(7, ft.Sum(2, 4));
            ft.Add(3, 10);
            Assert.AreEqual(25, ft.Sum(0, 5));
            Assert.AreEqual(6, ft.Sum(0, 3));
        }

        [Test]
        public void UIntFenwickTreeTest()
        {
            var ft = new UIntFenwickTree(5);
            var debugView = new UIntFenwickTree.DebugView(ft);
            Assert.AreEqual(0, ft.Sum(0, 0));
            Assert.AreEqual(0, ft.Sum(0, 1));
            Assert.AreEqual(0, ft.Sum(0, 2));
            Assert.AreEqual(0, ft.Sum(0, 3));
            Assert.AreEqual(0, ft.Sum(0, 4));
            Assert.AreEqual(0, ft.Sum(0, 5));
            CollectionAssert.AreEqual(new[] {
                new UIntFenwickTree.DebugItem(0, 0),
                new UIntFenwickTree.DebugItem(0, 0),
                new UIntFenwickTree.DebugItem(0, 0),
                new UIntFenwickTree.DebugItem(0, 0),
                new UIntFenwickTree.DebugItem(0, 0),
            }, debugView.Items);
            for (var i = 0; i < 5; ++i)
                ft.Add(i, (uint)i + 1);
            Assert.AreEqual(0, ft.Sum(0, 0));
            Assert.AreEqual(1, ft.Sum(0, 1));
            Assert.AreEqual(3, ft.Sum(0, 2));
            Assert.AreEqual(6, ft.Sum(0, 3));
            Assert.AreEqual(10, ft.Sum(0, 4));
            Assert.AreEqual(15, ft.Sum(0, 5));
            Assert.AreEqual(14, ft.Sum(1, 5));
            Assert.AreEqual(12, ft.Sum(2, 5));
            Assert.AreEqual(9, ft.Sum(3, 5));
            Assert.AreEqual(5, ft.Sum(4, 5));
            Assert.AreEqual(0, ft.Sum(5, 5));
            CollectionAssert.AreEqual(new[] {
                new UIntFenwickTree.DebugItem(1, 1),
                new UIntFenwickTree.DebugItem(2, 3),
                new UIntFenwickTree.DebugItem(3, 6),
                new UIntFenwickTree.DebugItem(4, 10),
                new UIntFenwickTree.DebugItem(5, 15),
            }, debugView.Items);
        }
    }
}
