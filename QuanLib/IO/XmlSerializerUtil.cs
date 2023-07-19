using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QuanLib.IO
{
    /// <summary>
    /// XMl序列化工具类
    /// </summary>
    public static class XmlSerializerUtil
    {
        /// <summary>
        /// 向指定路径写入xml序列化文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        public static void WriteFile<T>(string path, T obj) where T : new()
        {
            using FileStream fileStream = new(path, FileMode.Create);
            XmlSerializer xmlSerializer = new(typeof(T));
            xmlSerializer.Serialize(fileStream, obj);
            fileStream.Flush();
        }

        /// <summary>
        /// 尝试向指定路径写入xml序列化文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool TryWriteFile<T>(string path, T obj) where T : new()
        {
            try
            {
                WriteFile<T>(path, obj);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 读取指定文件路径的xml序列化文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T? ReadFile<T>(string path) where T : new()
        {
            using FileStream fileStream = new(path, FileMode.Open);
            return (T?)new XmlSerializer(typeof(T)).Deserialize(fileStream);
        }

        /// <summary>
        /// 尝试读取指定文件路径的xml序列化文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool TryReadFile<T>(string path, out T? obj) where T : new()
        {
            try
            {
                obj = ReadFile<T>(path);
                return true;
            }
            catch
            {
                obj = default;
                return false;
            }
        }

        /// <summary>
        /// 读取指定xml字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T? ReadXml<T>(string xml) where T : new()
        {
            using StringReader stringReader = new(xml);
            return (T?)new XmlSerializer(typeof(T)).Deserialize(stringReader);
        }

        /// <summary>
        /// 尝试读取指定xml字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool TryReadXml<T>(string xml, out T? obj) where T : new()
        {
            try
            {
                obj = ReadXml<T>(xml);
                return true;
            }
            catch
            {
                obj = default;
                return false;
            }
        }

        /// <summary>
        /// 获取序列化后的xml字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetXml<T>(T obj) where T : new()
        {
            MemoryStream memoryStream = new();
            new XmlSerializer(typeof(T)).Serialize(memoryStream, obj);
            memoryStream.Position = 0;
            StreamReader streamReader = new(memoryStream);
            string str = streamReader.ReadToEnd();
            streamReader.Close();
            memoryStream.Close();
            return str;
        }

        /// <summary>
        /// 尝试通过xml序列化深度复制对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(T obj) where T : new()
        {
            using MemoryStream memoryStream = new();
            XmlSerializer xmlSerializer = new(typeof(T));
            xmlSerializer.Serialize(memoryStream, obj);
            memoryStream.Position = 0;
            return (T)xmlSerializer.Deserialize(memoryStream);
        }
    }
}
