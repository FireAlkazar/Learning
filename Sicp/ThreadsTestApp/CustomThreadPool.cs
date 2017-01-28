using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadsTestApp
{
    public class CustomThreadPool
    {
        private readonly QueueDispatcher _queueDispatcher = new QueueDispatcher(new PriorityQueue());
        private readonly List<ThreadPoolItem> _threadPoolItems = new List<ThreadPoolItem>();

        public CustomThreadPool(int numberOfThreads)
        {
            if(numberOfThreads <= 0)
            {
                throw new ArgumentException(nameof(numberOfThreads));
            }

            for (var i = 0; i < numberOfThreads; i++)
            {
                var threadPoolItem = new ThreadPoolItem(_queueDispatcher);
                threadPoolItem.Start();
                _threadPoolItems.Add(threadPoolItem);
            }
        }

        public bool Execute(ITask task, TaskPriority taskPriority)
        {
            return _queueDispatcher.Enqueue(task, taskPriority);
        }

        public void Stop()
        {
            _queueDispatcher.Stop();
            _threadPoolItems.ForEach(x => x.WaitForStop());
        }
    }

    internal class QueueDispatcher
    {
        private readonly object _dispatcherLock = new object();
        private readonly PriorityQueue _queue;
        private volatile bool _stopped;

        public QueueDispatcher(PriorityQueue queue)
        {
            _queue = queue;
        }

        public bool Enqueue(ITask task, TaskPriority priority)
        {
            lock (_dispatcherLock)
            {
                if(_stopped)
                {
                    return false;
                }

                _queue.Enqueue(task, priority);
                Monitor.PulseAll(_dispatcherLock);
                return true;
            }
        }

        public bool WaitForDequeueOrStop(out ITask task)
        {
            lock (_dispatcherLock)
            {
                while (true)
                {
                    if (_queue.Count > 0)
                    {
                        task = _queue.Dequeue();
                        return true;
                    }

                    if (_stopped)
                    {
                        task = null;
                        return false;
                    }

                    Monitor.Wait(_dispatcherLock);
                }
            }
        }

        public void Stop()
        {
            lock (_dispatcherLock)
            {
                _stopped = true;
                Monitor.PulseAll(_dispatcherLock);
            }
        }
    }

    internal class ThreadPoolItem
    {
        private readonly QueueDispatcher _taskQueueDispatcher;
        private readonly Thread _thread;

        public ThreadPoolItem(QueueDispatcher taskQueueDispatcher)
        {
            _taskQueueDispatcher = taskQueueDispatcher;
            _thread = new Thread(MainLoop);
        }

        private void MainLoop()
        {
            while (true)
            {
                try
                {
                    ITask task;
                    bool taskTaken = _taskQueueDispatcher.WaitForDequeueOrStop(out task);
                    if (taskTaken)
                    {
                        task.Execute();
                    }
                    else
                    {
                        return;
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public void Start()
        {
            _thread.Start();
        }

        public void WaitForStop()
        {
            _thread.Join();
        }
    }

    internal class PriorityQueue
    {
        private const int HighVersusNormalPriorityCoefficient = 3;
        private readonly Dictionary<TaskPriority, Queue<ITask>> _queues = new Dictionary<TaskPriority, Queue<ITask>>();
        private long _highPriorityTaskTakenCount;
        private long _normalPriorityTaskTakenCount;

        public PriorityQueue()
        {
            _queues[TaskPriority.High] = new Queue<ITask>();
            _queues[TaskPriority.Normal] = new Queue<ITask>();
            _queues[TaskPriority.Low] = new Queue<ITask>();
        }

        public int Count => _queues[TaskPriority.High].Count
            + _queues[TaskPriority.Normal].Count
            + _queues[TaskPriority.Low].Count;

        public ITask Dequeue()
        {
            Queue<ITask> queue;
            if (ShouldTakeHighPriority())
            {
                _highPriorityTaskTakenCount++;
                queue = _queues[TaskPriority.High];
            }
            else if (ShouldTakeNormalPriority())
            {
                _normalPriorityTaskTakenCount++;
                queue = _queues[TaskPriority.Normal];
            }
            else
            {
                queue = _queues[TaskPriority.Low];
            }

            return queue.Dequeue();
        }

        public void Enqueue(ITask task, TaskPriority taskPriority)
        {
            _queues[taskPriority].Enqueue(task);
        }

        private bool IsLessThanThreeHighOnOneNormaPrioritylWasTaken()
        {
            if (_normalPriorityTaskTakenCount == 0)
            {
                return _highPriorityTaskTakenCount < HighVersusNormalPriorityCoefficient;
            }

            return ((double)_highPriorityTaskTakenCount) / _normalPriorityTaskTakenCount < HighVersusNormalPriorityCoefficient;
        }

        private bool ShouldTakeHighPriority()
        {
            return _queues[TaskPriority.High].Count > 0
                   && IsLessThanThreeHighOnOneNormaPrioritylWasTaken();
        }

        private bool ShouldTakeNormalPriority()
        {
            return _queues[TaskPriority.Normal].Count > 0;
        }
    }
}