using QuanLib.Core.FileListeners;
using QuanLib.Core.IO;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace QuanLib.Demo
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            Process process = new()
            {
                StartInfo = new("java", "-jar fabric-server-mc.1.20.1-loader.0.14.22-launcher.0.11.2.jar nogui")
                {
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    WorkingDirectory = "D:\\程序\\HMCL\\fabric-server-mc.1.20.1-loader.0.14.22-launcher.0.11.2"
                }
            };

            process.Start();

            while (true)
            {
                Console.ReadLine();
                string s = process.StandardOutput.ReadLine();
                Console.WriteLine(s);
            }
        }
    }
}
