using QuanLib.IO;

namespace QuanLib.Demo
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            ZipPack zipPack = new("D:\\程序\\HMCL\\模拟殖民地\\versions\\1.19.2\\1.19.2.jar");
            var entries = zipPack.GetEntrys("assets");
        }
    }
}