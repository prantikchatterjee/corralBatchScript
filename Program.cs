using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace corralBatch
{
    class Program
    {
        public static string[] filePaths;
        public static Queue<string> fileQueue;
        static void Main(string[] args)
        {
            string inputFilesDirectory = args[0];
            string corralExecutablePath = @"C:\corral\bin\Debug\corral.exe";
            //Console.WriteLine(inputFilesDirectory);
            //Console.ReadLine();
            filePaths = Directory.GetFiles(inputFilesDirectory, "*.bpl");
            fileQueue = new Queue<string>(filePaths);
            while (fileQueue.Count > 0)
            {
                //Console.ReadLine();
                string workingFile = fileQueue.Dequeue();
                Console.WriteLine(workingFile);
                Process p = new Process();
                p.StartInfo.FileName = corralExecutablePath;
                p.StartInfo.Arguments = workingFile + " /useProverEvaluate /si /di /doNotUseLabels /recursionBound:3 /newStratifiedInlining:split /killAfter:3600";
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.CreateNoWindow = false;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                //p.StartInfo.CreateNoWindow = true;
                //p.StartInfo.UseShellExecute = false;
                //p.StartInfo.RedirectStandardOutput = true;
                //string output = "";
                DateTime startTime = DateTime.Now;
                p.Start();
                /*var timeout = 3600;
                var timeouttask = new System.Threading.Tasks.Task(() =>
                {
                    System.Threading.Thread.Sleep(timeout * 1000);
                    string output = "Corral timed out";
                    Console.WriteLine(output);
                    File.AppendAllText(workingFile + ".txt", output);
                    //Process.GetCurrentProcess().Kill();
                    p.StandardOutput.Close();
                    p.StandardError.Close();
                    p.Kill();
                });
                timeouttask.Start();
                while (!p.HasExited)
                    output = output + p.StandardOutput.ReadLine();*/
                p.WaitForExit();
                if ((DateTime.Now-startTime).TotalSeconds >= 3600)
                    File.AppendAllText(workingFile + ".txt", "Corral timed out");
                Process killAllZ3Instances = new Process();
                killAllZ3Instances.StartInfo.FileName = "taskkill.exe";
                killAllZ3Instances.StartInfo.Arguments = "/F /IM z3.exe /T";
                killAllZ3Instances.Start();
                killAllZ3Instances.WaitForExit();
            }
        }
    }
}
