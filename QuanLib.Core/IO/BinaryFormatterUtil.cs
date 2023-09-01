using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.IO
{
    /// <summary>
    /// 二进制序列化工具类
    /// </summary>
    public static class BinaryFormatterUtil
    {
        private static BinaryFormatter BinaryFormatter = new();

        /// <summary>
        /// 向指定路径写入二进制序列化文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        public static void Write(string path, object obj)
        {
            using FileStream fileStream = new(path, FileMode.Create);
            BinaryFormatter.Serialize(fileStream, obj);
            fileStream.Flush();
        }

        /// <summary>
        /// 尝试向指定路径写入二进制序列化文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        /// <returns>这个返回true，失败返回false</returns>
        public static bool TryWrite(string path, object obj)
        {
            try
            {
                Write(path, obj);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 从指定路径写读取进制序列化文件并执行类型转换
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T Read<T>(string path) => (T)Read(path);

        /// <summary>
        /// 从指定路径写读取进制序列化文件
        /// </summary>
        /// <param name="path"></param>
        public static object Read(string path)
        {
            using FileStream fileStream = new(path, FileMode.Open);
            object obj = BinaryFormatter.Deserialize(fileStream);
            return obj;
        }

        /// <summary>
        /// 尝试从指定路径写读取进制序列化文件与尝试类型转换
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        /// <returns>这个返回true，失败返回fals</returns>
        public static bool TryRead<T>(string path, out T obj)
        {
            try
            {
                obj = (T)Read(path);
                return true;
            }
            catch
            {
                obj = default;
                return false;
            }
        }

        /// <summary>
        /// 尝试从指定路径写读取进制序列化文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        /// <returns>这个返回true，失败返回fals</returns>
        public static bool TryRead(string path, out object obj)
        {
            try
            {
                obj = Read(path);
                return true;
            }
            catch
            {
                obj = default;
                return false;
            }
        }

        /// <summary>
        /// 通过序列化深度复制对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(T obj)
        {
            using MemoryStream memoryStream = new();
            BinaryFormatter.Serialize(memoryStream, obj);
            memoryStream.Position = 0;
            return (T)BinaryFormatter.Deserialize(memoryStream);
        }

        /// <summary>
        /// 尝试通过序列化深度复制对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldObject"></param>
        /// <param name="newObject"></param>
        /// <returns></returns>
        public static bool TryDeepCopy<T>(T oldObject, out T newObject)
        {
            try
            {
                newObject = DeepCopy(oldObject);
                return true;
            }
            catch
            {
                newObject = default;
                return false;
            }
        }
    }
}
