using System;
using System.Threading;

namespace ThreadsTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var threadPool = new CustomThreadPool(1);

            threadPool.Execute(new ActionTask(() => SleepAndPrint(1500, "task 1")), TaskPriority.Normal);
            threadPool.Execute(new ActionTask(() => SleepAndPrint(1500, "task 2")), TaskPriority.Low);
            threadPool.Execute(new ActionTask(() => SleepAndPrint(1500, "task 3")), TaskPriority.Normal);
            threadPool.Execute(new ActionTask(() => SleepAndPrint(1500, "task 4")), TaskPriority.High);
            
            threadPool.Stop();
            Console.WriteLine("Stopped");

            bool isAdded = threadPool.Execute(new ActionTask(() => SleepAndPrint(1500, "task 4")), TaskPriority.High);
            Console.WriteLine($"Is task added after stop - {isAdded}");

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
