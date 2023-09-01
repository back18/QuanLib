using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace QuanLib.Core.IO
{
    /// <summary>
    /// XmlDocument扩展类
    /// </summary>
    public static class XmlDocumentExtend
    {
        /// <summary>
        /// 增加一级节点（根节点）
        /// </summary>
        /// <param name="xml">xml实例</param>
        /// <param name="name">根节点名</param>
        public static void AppendNode_1(this XmlDocument xml, string name)
        {
            xml.AppendChild(xml.CreateElement(name));
        }

        /// <summary>
        /// 增加二级节点
        /// </summary>
        /// <param name="xml">xml实例</param>
        /// <param name="name">节点名</param>
        public static void AppendNode_2(this XmlDocument xml, string name)
        {
            foreach (XmlNode node in xml.ChildNodes)
            {
                if (node is XmlElement)
                    node.AppendChild(xml.CreateElement(name));
            }

        }

        /// <summary>
        /// 增加二级节点
        /// </summary>
        /// <param name="xml">xml实例</param>
        /// <param name="names">节点名列表</param>
        public static void AppendNode_2(this XmlDocument xml, string[] names)
        {
            if (names is null)
                throw new ArgumentNullException(nameof(names));

            foreach (XmlNode node in xml.ChildNodes)
            {
                if (node is XmlElement)
                    foreach (var item in names)
                        node.AppendChild(xml.CreateElement(item));
            }
        }

        /// <summary>
        /// 增加二级节点
        /// </summary>
        /// <param name="xml">xml实例</param>
        /// <param name="name">节点名</param>
        /// <param name="text">节点文本</param>
        public static void AppendNode_2<T>(this XmlDocument xml, string name, T text)
        {
            if (text is null)
                throw new ArgumentNullException(nameof(text));

            foreach (XmlNode node in xml.ChildNodes)
            {
                if (node is XmlElement)
                    node.AppendChild(xml.CreateElement(name)).InnerText = text.ToString();
            }

        }

        /// <summary>
        /// 增加二级节点
        /// </summary>
        /// <param name="xml">xml实例</param>
        /// <param name="keyValuePairs">string键值集合</param>
        public static void AppendNode_2<T>(this XmlDocument xml, IDictionary<string, T> keyValuePairs)
        {
            if (keyValuePairs is null)
                throw new ArgumentNullException(nameof(keyValuePairs));

            foreach (XmlNode node in xml.ChildNodes)
            {
                if (node is XmlElement)
                    foreach (var pair in keyValuePairs)
                        node.AppendChild(xml.CreateElement(pair.Key)).Value = pair.Value.ToString();
            }
        }

        /// <summary>
        /// 在指定二级节点处增加三级节点
        /// </summary>
        /// <param name="xml">xml实例</param>
        /// <param name="node_2_name">二级节点名</param>
        /// <param name="name">节点名</param>
        public static void AppendNode_3(this XmlDocument xml, string node_2_name, string name)
        {
            XmlNode? node_2 = null;
            foreach (XmlNode node in xml.ChildNodes)
            {
                if (node is XmlElement)
                    node_2 = node.SelectSingleNode(node_2_name);
            }

            if (node_2 is null)
                throw new Exception("二级节点不存在");

            node_2.AppendChild(xml.CreateElement(name));
        }

        /// <summary>
        /// 在指定二级节点处增加三级节点
        /// </summary>
        /// <param name="xml">xml实例</param>
        /// <param name="node_2_name">二级节点名</param>
        /// <param name="names">节点名列表</param>
        public static void AppendNode_3(this XmlDocument xml, string node_2_name, string[] names)
        {
            XmlNode? node_2 = null;
            foreach (XmlNode node in xml.ChildNodes)
            {
                if (node is XmlElement)
                    node_2 = node.SelectSingleNode(node_2_name);
            }

            if (node_2 is null)
                throw new Exception("二级节点不存在");

            foreach (var item in names)
                node_2.AppendChild(xml.CreateElement(item));
        }

        /// <summary>
        /// 在指定二级节点处增加三级节点
        /// </summary>
        /// <param name="xml">xml实例</param>
        /// <param name="node_2_name">二级节点名</param>
        /// <param name="name">节点名</param>
        /// <param name="text">节点文本</param>
        public static void AppendNode_3<T>(this XmlDocument xml, string node_2_name, string name, T text)
        {
            XmlNode? node_2 = null;
            foreach (XmlNode node in xml.ChildNodes)
            {
                if (node is XmlElement)
                    node_2 = node.SelectSingleNode(node_2_name);
            }

            if (node_2 is null)
                throw new Exception("二级节点不存在");

            node_2.AppendChild(xml.CreateElement(name)).InnerText = text.ToString();
        }

        /// <summary>
        /// 在指定二级节点处增加三级节点
        /// </summary>
        /// <param name="xml">xml实例</param>
        /// <param name="node_2_name">二级节点名</param>
        /// <param name="keyValuePairs">string键值集合</param>
        public static void AppendNode_3<T>(this XmlDocument xml, string node_2_name, IDictionary<string, T> keyValuePairs)
        {
            XmlNode? node_2 = null;
            foreach (XmlNode node in xml.ChildNodes)
            {
                if (node is XmlElement)
                    node_2 = node.SelectSingleNode(node_2_name);
            }

            if (node_2 is null)
                throw new Exception("二级节点不存在");

            foreach (var pair in keyValuePairs)
                node_2.AppendChild(xml.CreateElement(pair.Key)).InnerText = pair.Value.ToString();
        }
    }
}
