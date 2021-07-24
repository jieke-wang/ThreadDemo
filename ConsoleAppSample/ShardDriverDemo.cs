using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ConsoleAppSample
{
    public class ShardDriverDemo
    {
        public static void Demo1()
        {
            string host = "192.168.199.100";
            string username = "user";
            string password = "pwd";
            string tag = "k"; // 映射的虚拟盘符
            string shareName = "wwwroot";

            string sharedDriver = $@"net use {tag} \\{host}\{shareName} {password} /user:{username}";

            StartCommand(null, sharedDriver, errorAction: (error) => 
            {
                Console.WriteLine(error);
            });
        }

        static int StartCommand(string workingDir, string command, string filename = "cmd.exe", Action<string> errorAction = null)
        {
            Process process = new Process();
            process.StartInfo.FileName = filename;
            process.StartInfo.Arguments = command;
            process.StartInfo.WorkingDirectory = workingDir;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;

            if(errorAction != null)
            {
                process.ErrorDataReceived += (source, eventArges) => 
                {
                    errorAction.Invoke(eventArges.Data);
                };
            }

            process.Start();
            process.WaitForExit(3000);

            return process.ExitCode;
        }

    }
}
