using System;
using System.Threading;
using ThreadsTestApp.Threads;

namespace ThreadsTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var threadPool = new CustomThreadPool(2);

            threadPool.Execute(new ActionTask(() => SleepAndPrint(1500, "slept 1500")), TaskPriority.Normal);
            threadPool.Execute(new ActionTask(() => SleepAndPrint(1500, "slept 1500")), TaskPriority.Normal);
            threadPool.Execute(new ActionTask(() => SleepAndPrint(1500, "slept 1500")), TaskPriority.Normal);
            threadPool.Execute(new ActionTask(() => SleepAndPrint(1500, "slept 1500")), TaskPriority.Normal);
            
            threadPool.Stop();
            Console.WriteLine("Stopped");
            Console.ReadLine();
        }

        private static void SleepAndPrint(int milliseconds, string message)
        {
            Thread.Sleep(milliseconds);
            Console.WriteLine($"Thread with id {Thread.CurrentThread.ManagedThreadId} - {message}");
        }

        private class ActionTask : ITask
        {
            private readonly Action _action;

            public ActionTask(Action action)
            {
                _action = action;
            }

            public void Execute()
            {
                _action?.Invoke();
            }
        }
    }
}
