using QuanLib.IO;

namespace QuanLib.Demo
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            ZipPack zipPack = new("1.20.1.jar");
            var entries = zipPack.GetDirectorys();
            foreach (var entry in entries)
            {
                Console.WriteLine(entry);
            }
        }
    }
}