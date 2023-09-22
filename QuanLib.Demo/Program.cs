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
            string hash = HashUtil.GetHashString("E:\\下载\\谷歌浏览器\\client.jar", HashType.SHA1);
        }
    }
}
