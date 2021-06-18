namespace AtCoder.Internal
{
    public interface IPriorityQueueOp<T>
    {
        int Count { get; }
        T Peek { get; }
        void Enqueue(T value);
        T Dequeue();
        bool TryDequeue(out T result);
        void Clear();
    }
}
