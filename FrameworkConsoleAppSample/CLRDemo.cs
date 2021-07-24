using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkConsoleAppSample
{
    class CLRDemo
    {
        public static void StringDemo()
        {
            string s1 = string.Format("hello {0}", "word");
            string s2 = $"hello {"word"}";
            string s3 = $"{s2}!`";
        }
    }
}
