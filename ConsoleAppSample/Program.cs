using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

            Func<string, string> func = (str) => { return str; };
            AsyncCallback asyncCallback = (ar) => 
            {
                Console.WriteLine("");
            };
            IAsyncResult asyncResult = func.BeginInvoke("jieke", asyncCallback, null);
        }
    }
}

/*
 进程
 线程
 多线程
 句柄
 同步：发起调用时，只有在调用的方法完成后，才能继续执行下面的逻辑，按顺序执行
 异步：发起调用时，不用等待调用的方法执行完成，就可继续执行下面的逻辑，会启动一个新的线程去执行调用的方法
*/
/*
 cpu是多核多线程的，系统是分时系统
 操作系统是分时执行各个进程指令的，达到宏观并行的效果
*/
