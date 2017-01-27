using System.Collections.Generic;

namespace ThreadsTestApp
{
    public class PriorityQueue : IPriorityQueue
    {
        private readonly Queue<ITask> _queue = new Queue<ITask>();

        public int Count => _queue.Count;

        public void Enqueue(ITask task, TaskPriority taskPriority)
        {
            _queue.Enqueue(task);
        }

        public ITask Dequeue()
        {
            return _queue.Dequeue();
        }
    }
}