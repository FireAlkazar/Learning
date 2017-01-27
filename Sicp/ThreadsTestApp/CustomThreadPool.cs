using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadsTestApp.Threads
{
    public class CustomThreadPool
    {
        private readonly List<ThreadPoolItem> _threadPoolItems = new List<ThreadPoolItem>();
        private readonly QueueDispatcher _queueDispatcher = new QueueDispatcher();

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
        private object _dispatcherLock = new object();
        private readonly IPriorityQueue _queue = new PriorityQueue();
        private volatile bool _stopped;

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

        public bool TryDequeue(out ITask task)
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
                //Monitor.PulseAll(_dispatcherLock);
            }
        }
    }

    internal class ThreadPoolItem
    {
        private readonly Thread _thread;
        private QueueDispatcher _taskQueueDispatcher;

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
                    if (_taskQueueDispatcher.TryDequeue(out task))
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
}