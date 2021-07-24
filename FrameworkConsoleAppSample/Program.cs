using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FrameworkConsoleAppSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //TaskDemo.Demo10();
            //TaskPoolSampleTester.Demo1();

            //ProxySample.Test();

            //UnsafeClass.Demo();

            //TaskDemo.Demo12();
            Thread.Sleep(Timeout.Infinite);
        }

        static void FuncDemo()
        {
            Func<string, string> func = (str) => { return str; };
            AsyncCallback asyncCallback = (ar) =>
            {
                Console.WriteLine("运行结束");
            };
            IAsyncResult asyncResult = func.BeginInvoke("jieke", asyncCallback, null);
            string result = func.EndInvoke(asyncResult);

            while (asyncResult.IsCompleted == false)
            {
                Thread.Sleep(1000);
            }
        }

        static void ActionDemo()
        {
            Action<string> action = (str) => { Console.WriteLine(str); };
            AsyncCallback asyncCallback = (ar) =>
            {
                Console.WriteLine("运行结束");
                action.EndInvoke(ar);
            };
            action.BeginInvoke("jieke", asyncCallback, null);
            IAsyncResult asyncResult = action.BeginInvoke("jieke", asyncCallback, null);
            //action.EndInvoke(asyncResult);
            while (asyncResult.IsCompleted == false)
            {
                Thread.Sleep(1000);
            }
        }

        static void ThreadPoolDemo()
        {
            // 开启线程
            //ThreadPool.QueueUserWorkItem(o => Console.WriteLine("QueueUserWorkItem"));
            //ThreadPool.QueueUserWorkItem(o => Console.WriteLine(o), "QueueUserWorkItem2");

            // 默认与cpu线程数有关
            //ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxCompletionPortThreads);
            //ThreadPool.GetMinThreads(out int minWorkerThreads, out int minCompletionPortThreads);

            //ThreadPool.SetMaxThreads(4, 4); // 不能低于cpu的线程数
            //ThreadPool.SetMinThreads(4, 4);
        }
    }

    // 取地址（指针）https://blog.csdn.net/qq_41894840/article/details/79777691
    public unsafe class UnsafeClass
    {
        static int* iPtr;

        public static void Demo()
        {
            var i = 1;
            iPtr = &i;

            var ptr = *iPtr;
            Console.WriteLine(ptr);
        }
    }
}

// 回调、join、endinvock、循环状态检测