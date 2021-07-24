using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FrameworkConsoleAppSample
{
    class TaskDemo
    {
        static void Demo1()
        {
            Task task = new Task(() => { });
            task.Start();
        }

        static void Demo2()
        {
            Task.Run(() => { });
        }

        static void Demo3()
        {
            TaskFactory taskFactory = new TaskFactory();
            taskFactory = Task.Factory;
            Task task = taskFactory.StartNew(() => { });

            taskFactory.ContinueWhenAll(new[] { task }, t => { });
            taskFactory.ContinueWhenAny(new[] { task }, t => { });
        }

        static void Demo4()
        {
            Task.Delay(2000).ContinueWith(t =>
            {
                Thread.Sleep(2000);
            });

            Task.WaitAll();
            Task.WaitAny();
            Task.WhenAll().ContinueWith(t => { });
            Task.WhenAny().ContinueWith(t => { });
        }

        static void Demo5()
        {
            var task = Task.Run<int>(() => { return DateTime.Now.Year; });
            Console.WriteLine(task.Result);
            //task.Exception
        }

        static void Demo6()
        {
            var task = Task.Run<int>(() => { return DateTime.Now.Year; }).ContinueWith(t => { Console.WriteLine(t.Result); }); ;
        }

        static void Demo7()
        {
            // 并发执行, task wait all
            Parallel.Invoke(() => { Console.WriteLine(1); }, () => { Console.WriteLine(2); }, () => { Console.WriteLine(3); });

            Parallel.For(0, 10, i => { Console.WriteLine(i); });

            Parallel.ForEach(Enumerable.Range(12, 15), i => { Console.WriteLine(i); });

            // 控制执行数量
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = 2;
        }

        static void Demo8()
        {
            Thread thread = new Thread(() => { });

            //thread.Abort();
            //ThreadAbortException

            //thread.Interrupt();
            //ThreadInterruptedException

            //thread.IsAlive;

            //ThreadState.Aborted.HasFlag(ThreadState.Background);
            //thread.IsBackground;
            //thread.IsThreadPoolThread;

            //EventWaitHandle
            //AutoResetEvent // 调用set时只能释放一个wait one
            //ManualResetEvent
            //WaitHandle

            //Mutex
            //Semaphore

            Task task = new Task(() => { });
            task.Start();
        }

        // 线程取消
        static void Demo9()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            cancellationTokenSource.Cancel();

            // 全局变量
            // CancellationTokenSource
        }

        internal static void Demo10()
        {
            //ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 4 };
            ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 6 };
            List<Action> actions = new List<Action>();
            for (int i = 0; i < 1000; i++)
            {
                int j = i;

                #region
                //Task task = new Task(state =>
                //{
                //    Thread.Sleep(1000);
                //    Console.WriteLine($"state: {state}; Thread Id: {Thread.CurrentThread.ManagedThreadId}");
                //}, j);

                //task.Start();
                //Parallel.Invoke(parallelOptions, task.ConfigureAwait(false).GetAwaiter().GetResult);

                //Parallel.Invoke(parallelOptions, task.Start); 
                #endregion

                Action action = () =>
                {
                    //Thread.Sleep(0);
                    //Thread.Sleep(500);

                    Console.WriteLine($"j:{j}; Thread Id: {Thread.CurrentThread.ManagedThreadId}");
                };
                actions.Add(action);

                //Parallel.Invoke(parallelOptions, action);
            }

            Parallel.Invoke(parallelOptions, actions.ToArray());
        }

        static void Demo11()
        {
            // 创建CTS共享变量；try-catch；在开启的线程中判断 IsCancellationRequested
            CancellationTokenSource cts = new CancellationTokenSource();

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 50; i++)
            {
                int iValue = i;

                tasks.Add(Task.Run(() => 
                {
                    try
                    {
                        if(cts.IsCancellationRequested == false)
                        {

                        }
                    }
                    catch (Exception ex)
                    {
                        cts.Cancel();
                        Console.WriteLine(ex);
                    }
                }, cts.Token));
            }

            // 启动线程传递token，异常取消，token 抛异常取消线程
        }

        internal static void Demo12()
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 4; i++)
            {
                int _i = i;
                tasks.Add(Task.Run<int>(() => 
                {
                    Thread.Sleep(500);
                    return _i;
                }));
            }

            var result = Task.Factory.ContinueWhenAll<List<int>>(tasks.ToArray(), (_tasks) => 
            {
                List<int> results = new List<int>();

                foreach (Task task in _tasks)
                {
                    if(task is Task<int> _task)
                    {
                        results.Add(_task.Result);
                    }
                }

                return results;
            });
        }
    }
}

// 多线程使用的前提：可以并发执行