using System.Collections.Generic;

namespace AtCoder
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public interface INumOperator<T> : IEqualityComparer<T>, IComparer<T> where T : struct
    {
        public T MinValue { get; }
        public T MaxValue { get; }
        T Add(T x, T y);
        T Subtract(T x, T y);
        T Multiply(T x, T y);
        T Divide(T x, T y);
        T Modulo(T x, T y);
        bool GreaterThan(T x, T y);
        bool GreaterThanOrEqual(T x, T y);
        bool LessThan(T x, T y);
        bool LessThanOrEqual(T x, T y);
    }
    public readonly struct IntOperator : INumOperator<int>
    {
        public int MinValue => int.MinValue;
        public int MaxValue => int.MaxValue;
        public int Add(int x, int y) => x + y;
        public int Subtract(int x, int y) => x - y;
        public int Multiply(int x, int y) => x * y;
        public int Divide(int x, int y) => x / y;
        public int Modulo(int x, int y) => x % y;
        public bool GreaterThan(int x, int y) => x > y;
        public bool GreaterThanOrEqual(int x, int y) => x >= y;
        public bool LessThan(int x, int y) => x < y;
        public bool LessThanOrEqual(int x, int y) => x <= y;
        public int Compare(int x, int y) => x.CompareTo(y);
        public bool Equals(int x, int y) => x == y;
        public int GetHashCode(int obj) => obj.GetHashCode();
    }
    public readonly struct LongOperator : INumOperator<long>
    {
        public long MinValue => long.MinValue;
        public long MaxValue => long.MaxValue;
        public long Add(long x, long y) => x + y;
        public long Subtract(long x, long y) => x - y;
        public long Multiply(long x, long y) => x * y;
        public long Divide(long x, long y) => x / y;
        public long Modulo(long x, long y) => x % y;
        public bool GreaterThan(long x, long y) => x > y;
        public bool GreaterThanOrEqual(long x, long y) => x >= y;
        public bool LessThan(long x, long y) => x < y;
        public bool LessThanOrEqual(long x, long y) => x <= y;
        public int Compare(long x, long y) => x.CompareTo(y);
        public bool Equals(long x, long y) => x == y;
        public int GetHashCode(long obj) => obj.GetHashCode();
    }
    public readonly struct UIntOperator : INumOperator<uint>
    {
        public uint MinValue => uint.MinValue;
        public uint MaxValue => uint.MaxValue;
        public uint Add(uint x, uint y) => x + y;
        public uint Subtract(uint x, uint y) => x - y;
        public uint Multiply(uint x, uint y) => x * y;
        public uint Divide(uint x, uint y) => x / y;
        public uint Modulo(uint x, uint y) => x % y;
        public bool GreaterThan(uint x, uint y) => x > y;
        public bool GreaterThanOrEqual(uint x, uint y) => x >= y;
        public bool LessThan(uint x, uint y) => x < y;
        public bool LessThanOrEqual(uint x, uint y) => x <= y;
        public int Compare(uint x, uint y) => x.CompareTo(y);
        public bool Equals(uint x, uint y) => x == y;
        public int GetHashCode(uint obj) => obj.GetHashCode();
    }
    public readonly struct ULongOperator : INumOperator<ulong>
    {
        public ulong MinValue => ulong.MinValue;
        public ulong MaxValue => ulong.MaxValue;
        public ulong Add(ulong x, ulong y) => x + y;
        public ulong Subtract(ulong x, ulong y) => x - y;
        public ulong Multiply(ulong x, ulong y) => x * y;
        public ulong Divide(ulong x, ulong y) => x / y;
        public ulong Modulo(ulong x, ulong y) => x % y;
        public bool GreaterThan(ulong x, ulong y) => x > y;
        public bool GreaterThanOrEqual(ulong x, ulong y) => x >= y;
        public bool LessThan(ulong x, ulong y) => x < y;
        public bool LessThanOrEqual(ulong x, ulong y) => x <= y;
        public int Compare(ulong x, ulong y) => x.CompareTo(y);
        public bool Equals(ulong x, ulong y) => x == y;
        public int GetHashCode(ulong obj) => obj.GetHashCode();
    }
    public readonly struct DoubleOperator : INumOperator<double>
    {
        public double MinValue => double.MinValue;
        public double MaxValue => double.MaxValue;
        public double Add(double x, double y) => x + y;
        public double Subtract(double x, double y) => x - y;
        public double Multiply(double x, double y) => x * y;
        public double Divide(double x, double y) => x / y;
        public double Modulo(double x, double y) => x % y;
        public bool GreaterThan(double x, double y) => x > y;
        public bool GreaterThanOrEqual(double x, double y) => x >= y;
        public bool LessThan(double x, double y) => x < y;
        public bool LessThanOrEqual(double x, double y) => x <= y;
        public int Compare(double x, double y) => x.CompareTo(y);
        public bool Equals(double x, double y) => x == y;
        public int GetHashCode(double obj) => obj.GetHashCode();
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
