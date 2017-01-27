namespace ThreadsTestApp
{
    public interface IPriorityQueue
    {
        int Count { get; }

        void Enqueue(ITask task, TaskPriority taskPriority);

        ITask Dequeue();
    }
}