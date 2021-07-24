using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FrameworkConsoleAppSample
{
    //[Synchronization]
    class ThreadDemo
    {
        static void Demo1()
        {
            //ContextBoundObject
            ManualResetEvent manualResetEvent = new ManualResetEvent(false); // 默认关闭

            ThreadPool.QueueUserWorkItem(o =>
            {
                Thread.Sleep(3000);
                manualResetEvent.Set();
            });
            manualResetEvent.WaitOne();
        }

        static void Demo2()
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false); // 默认关闭

            ThreadPool.QueueUserWorkItem(o =>
            {
                Thread.Sleep(3000);
                autoResetEvent.Set();
            });
            autoResetEvent.WaitOne();

            //ThreadPool.SetMaxThreads
        }

        static void Demo3()
        {
            Thread thread = new Thread(() => { });
            //thread.Start();
            //thread.Interrupt();
            //thread.Join();
            //thread.Abort();
            //Thread.ResetAbort();
        }

        static void Demo4()
        {
            Thread.Sleep(0);    // 释放CPU时间片
            Thread.Sleep(1000);    // 休眠1000毫秒
            Thread.Sleep(TimeSpan.FromHours(1));    // 休眠1小时
            Thread.Sleep(Timeout.Infinite);    // 休眠直到中断

            //Thread.SpinWait(100);
        }

        static void Demo5()
        {
            object objLock = new object();

            // C# 的 lock 语句实际上是调用 Monitor.Enter 和 Monitor.Exit，中间夹杂 try-finally 语句的简略版
            lock (objLock)
            { }
            /**********************************************************************************************/
            Monitor.Enter(objLock);
            try
            {

            }
            finally
            {
                Monitor.Exit(objLock);
            }
        }

        static void Demo6()
        {
            int i = 0;
            _ = Interlocked.Increment(ref i);

            //CountdownEvent
            //ReaderWriterLock
            //Mutex
            //Semaphore
        }
    }
}
