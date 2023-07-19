using System;
using System.IO;

namespace QuanLib.ConsoleUtil
{
    /// <summary>
    /// 控制台字符宽度
    /// </summary>
    internal class ConsoleCharWidth
    {
        /// <summary>
        /// 构造并读取/生成缓存
        /// </summary>
        internal ConsoleCharWidth()
        {
            if (ExistsCache)
                ReadCache();
            else GenerateCache();
        }

        /// <summary>
        /// 获取char字符的对应宽度
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        internal byte this[char c] => _width[c];
        private byte[] _width;

        /// <summary>
        /// 缓存文件路径
        /// </summary>
        private static readonly string CacheFilePath = "ConsoleCharWidth.cache";

        /// <summary>
        /// 是否存在缓存文件
        /// </summary>
        private static bool ExistsCache => File.Exists(CacheFilePath);

        /// <summary>
        /// 生成缓存
        /// </summary>
        /// <returns></returns>
        private void GenerateCache()
        {
            byte[] widths = new byte[65536];

            Console.WriteLine("开始生成ConsoleCharWidth缓存，请稍后……");
            for (int u = 32; u < widths.Length; u++)
            {
                Console.Write((char)u);
                widths[u] = (byte)Console.CursorLeft;
                while (Console.CursorLeft > 0)
                    Console.Write('\b');
            }
            Console.WriteLine("ConsoleCharWidth缓存生成完成");

            _width = widths;

            FileStream fileStrea = new(CacheFilePath, FileMode.Create);
            fileStrea.Write(widths, 0, widths.Length);
            fileStrea.Flush();
            fileStrea.Close();
        }

        /// <summary>
        /// 读取缓存
        /// </summary>
        private void ReadCache()
        {
            if (!ExistsCache)
                throw new IOException("缓存文件不存在");

            byte[] cache = File.ReadAllBytes(CacheFilePath);
            if (cache.Length != 65536)
                throw new IOException("缓存文件大小异常，应为65536字节");

            _width = cache;
        }
    }
}
