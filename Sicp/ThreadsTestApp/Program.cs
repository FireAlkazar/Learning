using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThreadsTestApp.Threads;

namespace ThreadsTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var threadPool = new CustomThreadPool(3);

            threadPool.Execute(new ActionTask(() =>
            {
                Thread.Sleep(1500);
                Console.WriteLine("Task slept 1500");
            }));

            threadPool.Execute(new ActionTask(() =>
            {
                Thread.Sleep(1500);
                Console.WriteLine("Task slept 1500");
            }));

            threadPool.Execute(new ActionTask(() =>
            {
                Thread.Sleep(1500);
                Console.WriteLine("Task slept 1500");
            }));

            threadPool.Execute(new ActionTask(() =>
            {
                Thread.Sleep(1500);
                Console.WriteLine("Task slept 1500");
            }));

            threadPool.Stop();
            Console.WriteLine("Stopped");
            Console.ReadLine();
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
