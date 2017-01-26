using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ThreadsTestApp.Threads
{
    public class CustomThreadPool
    {
        private readonly TaskDispatcher _dispatcher;
        private readonly object _lockObject = new object();
        private volatile bool _isStopped;

        public CustomThreadPool(int numberOfThreads)
        {
            _dispatcher = new TaskDispatcher(numberOfThreads);
        }

        public bool Execute(ITask task)
        {
            lock (_lockObject)
            {
                if (_isStopped)
                {
                    return false;
                }
            }

            _dispatcher.Enqueue(task, TaskPriority.Normal);
            return true;
        }

        public void Stop()
        {
            lock (_lockObject)
            {
                _isStopped = true;
            }
            _dispatcher.ExecuteAllCurrentTasksAndWait();
        }
    }

    internal class PriorityQueue
    {
        private readonly ConcurrentQueue<ITask> _queue = new ConcurrentQueue<ITask>();

        public void Enqueue(ITask task, TaskPriority priority)
        {
            _queue.Enqueue(task);
        }

        public bool TryDequeue(out ITask task)
        {
            return _queue.TryDequeue(out task);
        }
    }

    internal class TaskDispatcher
    {
        private readonly ConcurrentStack<ThreadPoolItem> _idleThreads = new ConcurrentStack<ThreadPoolItem>();
        private readonly List<ThreadPoolItem> _threads = new List<ThreadPoolItem>();
        private readonly PriorityQueue _taskQueue = new PriorityQueue();
        private bool _stopped;

        public TaskDispatcher(int numberOfThreads)
        {
            for (var i = 0; i < numberOfThreads; i++)
            {
                var thread = new ThreadPoolItem();
                thread.TaskFinished += x =>
                {
                    _idleThreads.Push(x);
                    TryDispatchTaskToThread();
                };
                _threads.Add(thread);
                _idleThreads.Push(thread);
            }
        }

        public void ExecuteAllCurrentTasksAndWait()
        {
            _stopped = true;
            _idleThreads.ToList().ForEach(x => x.Stop());//???
            Wait();
        }

        private void Wait()
        {
            _threads.ForEach(x => x.Join());
        }

        public void Enqueue(ITask task, TaskPriority taskPriority)
        {
            _taskQueue.Enqueue(task, taskPriority);
            TryDispatchTaskToThread();
        }

        private void TryDispatchTaskToThread()
        {
            ThreadPoolItem thread = GetThread();

            if (thread == null)
            {
                return;
            }

            ITask task;
            if (_taskQueue.TryDequeue(out task))
            {
                thread.ExecuteAsync(task);
                return;
            }

            if (_stopped)
            {
                thread.Stop();
            }
            else
            {
                _idleThreads.Push(thread);
            }
        }

        private ThreadPoolItem GetThread()
        {
            ThreadPoolItem thread;
            _idleThreads.TryPop(out thread);
            return thread;
        }
    }

    internal class ThreadPoolItem
    {
        private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(false);
        private readonly Thread _thread;
        private volatile bool _isStopped;
        private volatile ITask _task;

        public ThreadPoolItem()
        {
            _thread = new Thread(MainLoop);
            _thread.Start();
        }

        private void MainLoop()
        {
            while (true)
            {
                _autoResetEvent.WaitOne();

                if (_isStopped)
                {
                    return;
                }

                try
                {
                    _task.Execute();
                }
                finally
                {
                    RaiseTaskFinishedEvent();
                }
            }
        }

        public void ExecuteAsync(ITask task)
        {
            _task = task;
            _autoResetEvent.Set();
        }

        public void Stop()
        {
            _isStopped = true;
            _autoResetEvent.Set();
        }

        public void Join()
        {
            _thread.Join();
        }

        public event Action<ThreadPoolItem> TaskFinished;

        private void RaiseTaskFinishedEvent()
        {
            TaskFinished?.Invoke(this);
        }
    }
}