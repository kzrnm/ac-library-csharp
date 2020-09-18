using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using AtCoder.Util.Internal;

namespace AtCoder.Stl
{
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class SetDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    {
        /*
         * Original is SortedSet<T>
         *
         * Copyright (c) .NET Foundation and Contributors
         * Released under the MIT license
         * https://github.com/dotnet/runtime/blob/master/LICENSE.TXT
         */
        internal const string LISENCE = @"
Original is SortedSet<T>

Copyright (c) .NET Foundation and Contributors
Released under the MIT license
https://github.com/dotnet/runtime/blob/master/LICENSE.TXT
";

        public bool IsMulti { get; }
        public KeyValuePair<TKey, TValue> Min
        {
            get
            {
                if (root == null) return default;
                var cur = root;
                while (cur.Left != null) { cur = cur.Left; }
                return cur.Pair;
            }
        }
        public KeyValuePair<TKey, TValue> Max
        {
            get
            {
                if (root == null) return default;
                var cur = root;
                while (cur.Right != null) { cur = cur.Right; }
                return cur.Pair;
            }
        }
        public Node FindNode(TKey key)
        {
            Node current = root;
            while (current != null)
            {
                int order = comparer.Compare(key, current.Key);
                if (order == 0) return current;
                current = order < 0 ? current.Left : current.Right;
            }
            return null;
        }
        public int Index(Node node)
        {
            var ret = NodeSize(node.Left);
            Node prev = node;
            node = node.Parent;
            while (prev != root)
            {
                if (node.Left != prev)
                {
                    ret += NodeSize(node.Left) + 1;
                }
                prev = node;
                node = node.Parent;
            }
            return ret;
        }
        public Node FindByIndex(int index)
        {
            var current = root; var currentIndex = current.Size - NodeSize(current.Right) - 1;
            while (currentIndex != index)
            {
                if (currentIndex > index)
                {
                    current = current.Left;
                    if (current == null) break;
                    currentIndex -= NodeSize(current.Right) + 1;
                }
                else
                {
                    current = current.Right;
                    if (current == null) break;
                    currentIndex += NodeSize(current.Left) + 1;
                }
            }
            return current;
        }
        public (Node node, int index) BinarySearch(TKey key, bool isLowerBound)
        {
            Node right = null;
            Node current = root;
            if (current == null) return (null, -1);
            int ri = -1;
            int ci = NodeSize(current.Left);
            while (true)
            {
                var order = comparer.Compare(key, current.Key);
                if (order < 0 || (isLowerBound && order == 0))
                {
                    right = current;
                    ri = ci;
                    current = current.Left;
                    if (current != null)
                        ci -= NodeSize(current.Right) + 1;
                    else break;
                }
                else
                {
                    current = current.Right;
                    if (current != null)
                        ci += NodeSize(current.Left) + 1;
                    else break;
                }
            }
            return (right, ri);
        }
        public Node FindNodeLowerBound(TKey key) => BinarySearch(key, true).node;
        public Node FindNodeUpperBound(TKey key) => BinarySearch(key, false).node;
        public KeyValuePair<TKey, TValue> LowerBoundItem(TKey key) => BinarySearch(key, true).node.Pair;
        public KeyValuePair<TKey, TValue> UpperBoundItem(TKey key) => BinarySearch(key, false).node.Pair;
        public int LowerBoundIndex(TKey key) => BinarySearch(key, true).index;
        public int UpperBoundIndex(TKey key) => BinarySearch(key, false).index;

        public IEnumerable<KeyValuePair<TKey, TValue>> Reversed()
        {
            var e = new Enumerator(this, true, null);
            while (e.MoveNext()) yield return e.Current;
        }
        public IEnumerable<KeyValuePair<TKey, TValue>> Enumerate(Node from) => Enumerate(from, false);
        public IEnumerable<KeyValuePair<TKey, TValue>> Enumerate(Node from, bool reverse)
        {
            if (from == null) yield break;
            var e = new Enumerator(this, reverse, from);
            while (e.MoveNext()) yield return e.Current;
        }

        public SetDictionary(bool isMulti = false) : this(Comparer<TKey>.Default, isMulti) { }
        public SetDictionary(IDictionary<TKey, TValue> dict, bool isMulti = false) : this(dict, Comparer<TKey>.Default, isMulti) { }
        public SetDictionary(IComparer<TKey> comparer, bool isMulti = false)
        {
            this.comparer = comparer;
            IsMulti = isMulti;
        }
        public SetDictionary(IDictionary<TKey, TValue> dict, IComparer<TKey> comparer, bool isMulti = false)
        {
            this.comparer = comparer;
            var arr = InitArray(dict);
            root = ConstructRootFromSortedArray(arr, 0, arr.Length - 1, null);
            IsMulti = isMulti;
        }
        protected KeyValuePair<TKey, TValue>[] InitArray(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            var comparer = Comparer<KeyValuePair<TKey, TValue>>.Create((a, b) => this.comparer.Compare(a.Key, b.Key));
            KeyValuePair<TKey, TValue>[] arr;
            if (IsMulti)
            {
                arr = collection.ToArray();
                Array.Sort(arr, comparer);
            }
            else
            {
                var list = new List<KeyValuePair<TKey, TValue>>(collection);
                list.Sort(comparer);
                for (int i = list.Count - 1; i > 0; i--)
                    if (this.comparer.Compare(list[i - 1].Key, list[i].Key) == 0)
                        list.RemoveAt(i);
                arr = list.ToArray();
            }
            return arr;
        }

        public int Count => NodeSize(root);
        protected static int NodeSize(Node node) => node == null ? 0 : node.Size;
        protected readonly IComparer<TKey> comparer;
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;
        ICollection<TKey> IDictionary<TKey, TValue>.Keys => throw new NotSupportedException();
        ICollection<TValue> IDictionary<TKey, TValue>.Values => throw new NotSupportedException();
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => throw new NotSupportedException();
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => throw new NotSupportedException();
        public TValue this[TKey key] { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }


        Node root;

        static Node ConstructRootFromSortedArray(KeyValuePair<TKey, TValue>[] arr, int startIndex, int endIndex, Node redNode)
        {
            int size = endIndex - startIndex + 1;
            Node root;

            switch (size)
            {
                case 0:
                    return null;
                case 1:
                    root = new Node(arr[startIndex], false);
                    if (redNode != null)
                    {
                        root.Left = redNode;
                    }
                    break;
                case 2:
                    root = new Node(arr[startIndex], false)
                    {
                        Right = new Node(arr[endIndex], true)
                    };
                    if (redNode != null)
                    {
                        root.Left = redNode;
                    }
                    break;
                case 3:
                    root = new Node(arr[startIndex + 1], false)
                    {
                        Left = new Node(arr[startIndex], false),
                        Right = new Node(arr[endIndex], false)
                    };
                    if (redNode != null)
                    {
                        root.Left.Left = redNode;
                    }
                    break;
                default:
                    int midpt = ((startIndex + endIndex) / 2);
                    root = new Node(arr[midpt], false)
                    {
                        Left = ConstructRootFromSortedArray(arr, startIndex, midpt - 1, redNode),
                        Right = size % 2 == 0 ?
                        ConstructRootFromSortedArray(arr, midpt + 2, endIndex, new Node(arr[midpt + 1], true)) :
                        ConstructRootFromSortedArray(arr, midpt + 1, endIndex, null)
                    };
                    break;
            }
            return root;
        }

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value) => Add(key, value);
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> pair) => Add(pair.Key, pair.Value);
        public bool Add(TKey key, TValue value)
        {
            if (root == null)
            {
                root = new Node(key, value, false);
                return true;
            }
            Node current = root;
            Node parent = null;
            Node grandParent = null;
            Node greatGrandParent = null;
            int order = 0;
            while (current != null)
            {
                order = comparer.Compare(key, current.Key);
                if (order == 0 && !IsMulti)
                {
                    current.Key = key;
                    root.IsRed = false;
                    return false;
                }
                if (Is4Node(current))
                {
                    Split4Node(current);
                    if (IsNonNullRed(parent) == true)
                    {
                        InsertionBalance(current, ref parent, grandParent, greatGrandParent);
                    }
                }
                greatGrandParent = grandParent;
                grandParent = parent;
                parent = current;
                current = (order < 0) ? current.Left : current.Right;
            }
            Node node = new Node(key, value, true);
            if (order >= 0) parent.Right = node;
            else parent.Left = node;
            if (parent.IsRed) InsertionBalance(node, ref parent, grandParent, greatGrandParent);
            root.IsRed = false;
            return true;
        }
        public bool Remove(TKey key)
        {
            if (root == null) return false;
            Node current = root;
            Node parent = null;
            Node grandParent = null;
            Node match = null;
            Node parentOfMatch = null;
            bool foundMatch = false;
            while (current != null)
            {
                if (Is2Node(current))
                {
                    if (parent == null)
                    {
                        current.IsRed = true;
                    }
                    else
                    {
                        Node sibling = GetSibling(current, parent);
                        if (sibling.IsRed)
                        {
                            if (parent.Right == sibling) RotateLeft(parent);
                            else RotateRight(parent);

                            parent.IsRed = true;
                            sibling.IsRed = false;
                            ReplaceChildOrRoot(grandParent, parent, sibling);
                            grandParent = sibling;
                            if (parent == match) parentOfMatch = sibling;
                            sibling = (parent.Left == current) ? parent.Right : parent.Left;
                        }
                        if (Is2Node(sibling))
                        {
                            Merge2Nodes(parent);
                        }
                        else
                        {
                            TreeRotation rotation = GetRotation(parent, current, sibling);
                            Node newGrandParent = Rotate(parent, rotation);
                            newGrandParent.IsRed = parent.IsRed;
                            parent.IsRed = false;
                            current.IsRed = true;
                            ReplaceChildOrRoot(grandParent, parent, newGrandParent);
                            if (parent == match)
                            {
                                parentOfMatch = newGrandParent;
                            }
                        }
                    }
                }
                int order = foundMatch ? -1 : comparer.Compare(key, current.Key);
                if (order == 0)
                {
                    foundMatch = true;
                    match = current;
                    parentOfMatch = parent;
                }
                grandParent = parent;
                parent = current;
                current = order < 0 ? current.Left : current.Right;
            }
            if (match != null)
            {
                ReplaceNode(match, parentOfMatch, parent, grandParent);
            }
            if (root != null)
            {
                root.IsRed = false;
            }
            return foundMatch;
        }
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> pair)
        {
            if (root == null) return false;
            Node current = root;
            Node parent = null;
            Node grandParent = null;
            Node match = null;
            Node parentOfMatch = null;
            bool foundMatch = false;
            while (current != null)
            {
                if (Is2Node(current))
                {
                    if (parent == null)
                    {
                        current.IsRed = true;
                    }
                    else
                    {
                        Node sibling = GetSibling(current, parent);
                        if (sibling.IsRed)
                        {
                            if (parent.Right == sibling) RotateLeft(parent);
                            else RotateRight(parent);

                            parent.IsRed = true;
                            sibling.IsRed = false;
                            ReplaceChildOrRoot(grandParent, parent, sibling);
                            grandParent = sibling;
                            if (parent == match) parentOfMatch = sibling;
                            sibling = (parent.Left == current) ? parent.Right : parent.Left;
                        }
                        if (Is2Node(sibling))
                        {
                            Merge2Nodes(parent);
                        }
                        else
                        {
                            TreeRotation rotation = GetRotation(parent, current, sibling);
                            Node newGrandParent = Rotate(parent, rotation);
                            newGrandParent.IsRed = parent.IsRed;
                            parent.IsRed = false;
                            current.IsRed = true;
                            ReplaceChildOrRoot(grandParent, parent, newGrandParent);
                            if (parent == match)
                            {
                                parentOfMatch = newGrandParent;
                            }
                        }
                    }
                }
                int order = foundMatch ? -1 : comparer.Compare(pair.Key, current.Key);
                if (order == 0 && EqualityComparer<TValue>.Default.Equals(pair.Value, current.Value))
                {
                    foundMatch = true;
                    match = current;
                    parentOfMatch = parent;
                }
                grandParent = parent;
                parent = current;
                current = order < 0 ? current.Left : current.Right;
            }
            if (match != null)
            {
                ReplaceNode(match, parentOfMatch, parent, grandParent);
            }
            if (root != null)
            {
                root.IsRed = false;
            }
            return foundMatch;
        }
        public void Clear()
        {
            root = null;
        }
        public bool ContainsKey(TKey key) => FindNode(key) != null;
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> pair)
        {
            var node = FindNodeLowerBound(pair.Key);
            if (node == null) return false;
            var e = new Enumerator(this, false, node);
            while (e.MoveNext())
            {
                if (comparer.Compare(pair.Key, e.Current.Key) != 0) break;
                if (EqualityComparer<TValue>.Default.Equals(pair.Value, e.Current.Value)) return true;
            }
            return false;
        }
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            foreach (var item in this) array[arrayIndex++] = item;
        }
        public bool TryGetValue(TKey key, out TValue value)
        {
            var node = FindNode(key);
            if (node == null)
            {
                value = default;
                return false;
            }
            value = node.Value;
            return true;
        }
        public Enumerator GetEnumerator() => new Enumerator(this);
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => new Enumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        #region private
        static bool Is2Node(Node node) => IsNonNullBlack(node) && IsNullOrBlack(node.Left) && IsNullOrBlack(node.Right); static bool Is4Node(Node node) => IsNonNullRed(node.Left) && IsNonNullRed(node.Right);
        static bool IsNonNullRed(Node node) => node != null && node.IsRed;
        static bool IsNonNullBlack(Node node) => node != null && !node.IsRed;
        static bool IsNullOrBlack(Node node) => node == null || !node.IsRed;
        void ReplaceNode(Node match, Node parentOfMatch, Node succesor, Node parentOfSuccesor)
        {
            if (succesor == match)
            {
                succesor = match.Left;
            }
            else
            {
                if (succesor.Right != null)
                {
                    succesor.Right.IsRed = false;
                }
                if (parentOfSuccesor != match)
                {
                    parentOfSuccesor.Left = succesor.Right;
                    succesor.Right = match.Right;
                }
                succesor.Left = match.Left;
            }
            if (succesor != null)
            {
                succesor.IsRed = match.IsRed;
            }
            ReplaceChildOrRoot(parentOfMatch, match, succesor);
        }
        static void Merge2Nodes(Node parent)
        {
            parent.IsRed = false;
            parent.Left.IsRed = true;
            parent.Right.IsRed = true;
        }
        static void Split4Node(Node node)
        {
            node.IsRed = true;
            node.Left.IsRed = false;
            node.Right.IsRed = false;
        }
        static Node GetSibling(Node node, Node parent)
        {
            return parent.Left == node ? parent.Right : parent.Left;
        }
        void InsertionBalance(Node current, ref Node parent, Node grandParent, Node greatGrandParent)
        {
            bool parentIsOnRight = grandParent.Right == parent;
            bool currentIsOnRight = parent.Right == current;
            Node newChildOfGreatGrandParent;
            if (parentIsOnRight == currentIsOnRight)
            {
                newChildOfGreatGrandParent = currentIsOnRight ? RotateLeft(grandParent) : RotateRight(grandParent);
            }
            else
            {
                newChildOfGreatGrandParent = currentIsOnRight ? RotateLeftRight(grandParent) : RotateRightLeft(grandParent);
                parent = greatGrandParent;
            }
            grandParent.IsRed = true;
            newChildOfGreatGrandParent.IsRed = false;
            ReplaceChildOrRoot(greatGrandParent, grandParent, newChildOfGreatGrandParent);

        }
        static Node Rotate(Node node, TreeRotation rotation)
        {
            switch (rotation)
            {
                case TreeRotation.Right:
                    node.Left.Left.IsRed = false;
                    return RotateRight(node);
                case TreeRotation.Left:
                    node.Right.Right.IsRed = false;
                    return RotateLeft(node);
                case TreeRotation.RightLeft:
                    return RotateRightLeft(node);
                case TreeRotation.LeftRight:
                    return RotateLeftRight(node);
                default:
                    throw new InvalidOperationException();
            }
        }
        static Node RotateLeft(Node node)
        {
            Node child = node.Right;
            node.Right = child.Left;
            child.Left = node;
            return child;
        }
        static Node RotateLeftRight(Node node)
        {
            Node child = node.Left;
            Node grandChild = child.Right;

            node.Left = grandChild.Right;
            grandChild.Right = node;
            child.Right = grandChild.Left;
            grandChild.Left = child;
            return grandChild;
        }
        static Node RotateRight(Node node)
        {
            Node child = node.Left;
            node.Left = child.Right;
            child.Right = node;
            return child;
        }
        static Node RotateRightLeft(Node node)
        {
            Node child = node.Right;
            Node grandChild = child.Left;

            node.Right = grandChild.Left;
            grandChild.Left = node;
            child.Left = grandChild.Right;
            grandChild.Right = child;
            return grandChild;
        }
        void ReplaceChildOrRoot(Node parent, Node child, Node newChild)
        {
            if (parent != null)
            {
                if (parent.Left == child)
                {
                    parent.Left = newChild;
                }
                else
                {
                    parent.Right = newChild;
                }
            }
            else
            {
                root = newChild;
            }
        }
        static TreeRotation GetRotation(Node parent, Node current, Node sibling)
        {
            if (IsNonNullRed(sibling.Left))
            {
                if (parent.Left == current)
                {
                    return TreeRotation.RightLeft;
                }
                return TreeRotation.Right;
            }
            else
            {
                if (parent.Left == current)
                {
                    return TreeRotation.Left;
                }
                return TreeRotation.LeftRight;
            }
        }
        #endregion private

        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {

            SetDictionary<TKey, TValue> tree;

            Stack<Node> stack;
            Node current;

            bool reverse;
            internal Enumerator(SetDictionary<TKey, TValue> set) : this(set, false, null) { }
            internal Enumerator(SetDictionary<TKey, TValue> set, bool reverse, Node startNode)
            {
                tree = set;
                stack = new Stack<Node>(2 * Log2(tree.Count + 1));
                current = null;
                this.reverse = reverse;
                if (startNode == null) IntializeAll();
                else Intialize(startNode);

            }
            void IntializeAll()
            {
                var node = tree.root;
                while (node != null)
                {
                    var next = reverse ? node.Right : node.Left;
                    stack.Push(node); node = next;
                }
            }
            void Intialize(Node startNode)
            {
                if (startNode == null)
                    throw new InvalidOperationException(nameof(startNode) + "is null");
                current = null;
                var node = startNode;
                var list = new List<Node>(Log2(tree.Count + 1));
                var comparer = tree.comparer;

                if (reverse)
                {
                    while (node != null)
                    {
                        list.Add(node);
                        var parent = node.Parent;
                        if (parent == null || parent.Left == node) { node = parent; break; }
                        node = parent;
                    }
                    while (node != null)
                    {
                        var parent = node.Parent;
                        if (parent == null || parent.Right == node) { node = parent; break; }
                        node = parent;
                    }
                    while (node != null)
                    {
                        if (comparer.Compare(startNode.Key, node.Key) >= 0)
                            list.Add(node);
                        node = node.Parent;
                    }
                }
                else
                {
                    while (node != null)
                    {
                        list.Add(node);
                        var parent = node.Parent;
                        if (parent == null || parent.Right == node) { node = parent; break; }
                        node = parent;
                    }
                    while (node != null)
                    {
                        var parent = node.Parent;
                        if (parent == null || parent.Left == node) { node = parent; break; }
                        node = parent;
                    }
                    while (node != null)
                    {
                        if (comparer.Compare(startNode.Key, node.Key) <= 0)
                            list.Add(node);
                        node = node.Parent;
                    }
                }

                list.Reverse();
                foreach (var n in list) stack.Push(n);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static int Log2(int num) => BitOperations.Log2((uint)num) + 1;
            public KeyValuePair<TKey, TValue> Current => current == null ? default : current.Pair;

            public bool MoveNext()
            {
                if (stack.Count == 0)
                {
                    current = null; return false;
                }
                current = stack.Pop();
                var node = reverse ? current.Left : current.Right;
                while (node != null)
                {
                    var next = reverse ? node.Right : node.Left;
                    stack.Push(node);
                    node = next;
                }
                return true;
            }

            object IEnumerator.Current => Current;
            public void Dispose() { }
            public void Reset() => throw new NotSupportedException();

        }
        public class Node
        {
            public bool IsRed;
            public TKey Key;
            public TValue Value;
            public KeyValuePair<TKey, TValue> Pair => KeyValuePair.Create(Key, Value);
            public Node Parent
            {
                get; private set;
            }
            Node _left;
            public Node Left
            {
                get
                {
                    return _left;
                }
                set
                {
                    _left = value; if (value != null) value.Parent = this;
                    for (var cur = this; cur != null; cur = cur.Parent)
                    {
                        if (!cur.UpdateSize()) break;
                        if (cur.Parent != null && cur.Parent.Left != cur && cur.Parent.Right != cur)
                        {
                            cur.Parent = null;
                            break;
                        }
                    }
                }
            }
            Node _right;
            public Node Right
            {
                get
                {
                    return _right;
                }
                set
                {
                    _right = value;
                    if (value != null) value.Parent = this;
                    for (var cur = this; cur != null; cur = cur.Parent)
                    {
                        if (!cur.UpdateSize()) break;
                        if (cur.Parent != null && cur.Parent.Left != cur && cur.Parent.Right != cur)
                        {
                            cur.Parent = null; break;
                        }
                    }
                }
            }

            public int Size
            {
                get; private set;
            } = 1;
            public Node(KeyValuePair<TKey, TValue> pair, bool isRed)
            {
                Key = pair.Key;
                Value = pair.Value;
                IsRed = isRed;
            }
            public Node(TKey key, TValue value, bool isRed)
            {
                Key = key;
                Value = value;
                IsRed = isRed;
            }
            public bool UpdateSize()
            {
                var oldsize = Size;
                var size = 1;
                if (Left != null) size += Left.Size;
                if (Right != null) size += Right.Size;
                Size = size;
                return oldsize != size;
            }
            public override string ToString() => $"Size = {Size}, Item = {Key}";
        }
        enum TreeRotation : byte
        {
            Left = 1,
            Right = 2,
            RightLeft = 3,
            LeftRight = 4,
        }
    }
}
