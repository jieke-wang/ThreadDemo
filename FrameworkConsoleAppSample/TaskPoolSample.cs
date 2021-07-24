using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections;

namespace FrameworkConsoleAppSample
{
    public class TaskPoolSample
    {
        static ConcurrentQueue<Action> _TaskQueue = null;
        static volatile int _MaxDegreeOfParallelism;
        static volatile bool IsRunning;

        static TaskPoolSample()
        {
            _TaskQueue = new ConcurrentQueue<Action>();
            _MaxDegreeOfParallelism = 1000;
            IsRunning = false;
        }

        /// <summary>
        /// 设置最大并发量
        /// </summary>
        /// <param name="maxDegreeOfParallelism">最大并发数</param>
        public static void SetMaxDegreeOfParallelism(UInt16 maxDegreeOfParallelism)
        {
            _MaxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        public static async void Invoke(IEnumerable<Action> taskFactories)
        {
            lock (_TaskQueue)
            {
                foreach (var taskFactory in taskFactories)
                {
                    _TaskQueue.Enqueue(taskFactory);
                }
            }

            if (IsRunning == false) await DoTask();
        }

        static async Task DoTask()
        {
            IsRunning = true;

            lock (_TaskQueue)
            {
                if (_TaskQueue.Count == 0) goto End;
            }

            List<Task> tasksInFlight = new List<Task>(_MaxDegreeOfParallelism);

            do
            {
                while (tasksInFlight.Count < _MaxDegreeOfParallelism && _TaskQueue.Count > 0) // 添加任务到当前批次中
                {
                    Action taskFactory;
                    if (_TaskQueue.TryDequeue(out taskFactory))
                    {
                        lock (tasksInFlight)
                        {
                            tasksInFlight.Add(Task.Run(taskFactory));
                        }
                    }
                }

                Task completedTask = await Task.WhenAny(tasksInFlight).ConfigureAwait(false);

                try
                {
                    // 传播异常，如果发生异常，则任务会中断执行
                    await completedTask.ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    lock (tasksInFlight)
                    {
                        tasksInFlight.Remove(completedTask);
                    }
                }
            } while (_TaskQueue.Count > 0 || tasksInFlight.Count > 0);

        End:
            IsRunning = false;
        }
    }

    public class TaskPoolSampleTester
    {
        public static void Demo1()
        {
            TaskPoolSample.SetMaxDegreeOfParallelism(10);
            Random random = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < 1000; i++)
            {
                int iValue = i + 1;
                List<Action> tasks = new List<Action>();
                for (int j = 0; j < 1000; j++)
                {
                    int jValue = j + 1;
                    Action taskFactory = () =>
                    {
                        int timeout = random.Next(10, 100);
                        Thread.Sleep(timeout);
                        Console.WriteLine($"i: {iValue.ToString("000")};j: {jValue.ToString("000")}; Thread ID: {Thread.CurrentThread.ManagedThreadId.ToString("000")};timeout: {timeout.ToString("00")}");

                        if (timeout % 20 == 0)
                        {
                            Console.WriteLine("异常测试");
                            throw new Exception("异常测试");
                        }
                    };

                    tasks.Add(taskFactory);
                }

                TaskPoolSample.Invoke(tasks);
            }
        }
    }
}

// https://stackoverflow.com/questions/35734051/c-sharp-task-thread-pool-running-100-tasks-across-only-10-threads
// https://stackoverflow.com/questions/14075029/have-a-set-of-tasks-with-only-x-running-at-a-time

// https://docs.microsoft.com/zh-cn/dotnet/standard/collections/thread-safe/index?redirectedfrom=MSDN