using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkConsoleAppSample
{
    public class AsyncAwaitDemo
    {
        static void Sync()
        {
            Console.WriteLine("start Sync");
            Async().Wait();
            Console.WriteLine("end Sync");
        }

        static async Task Async()
        {
            Console.WriteLine("start Async");
            await Task.Delay(1000);
            Console.WriteLine("end Async");
        }
    }
}
