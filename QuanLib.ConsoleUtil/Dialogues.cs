using QuanLib.Verification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace QuanLib.ConsoleUtil
{
    /// <summary>
    /// 对话器
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    public abstract class Dialogues<T>
    {
        public Dialogues(string questions, bool isCanCancel = false)
        {
            Questions = questions;
            IsCanCancel = isCanCancel;
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Questions { get; set; }

        /// <summary>
        /// 是否可以取消输入
        /// </summary>
        public bool IsCanCancel
        {
            get => _IsCanCancel;
            set
            {
                if (value)
                    Message_Retry = "请重新输入，或输入\"Crrl + Z\"来取消输入";
                else
                    Message_Retry = "请重新输入";
                _IsCanCancel = value;
            }
        }
        private bool _IsCanCancel;

        /// <summary>
        /// 提示用户重新输入的消息
        /// </summary>
        protected string Message_Retry { get; private set; }

        /// <summary>
        /// 开始对话
        /// </summary>
        /// <param name="result">结果，如果取消输入则为null</param>
        /// <returns>是否成功读取到</returns>
        public bool Start(out T? result)
        {
            while (true)
            {
                if (GetReadText(out string? text))
                {
                    if (Verification(text!, out result, out string? errorMessage))
                        return true;
                    else WriteErrorMessage(errorMessage!);
                }
                else
                {
                    result = default;
                    return false;
                }
            }
        }

        /// <summary>
        /// 获取读取到的文本
        /// </summary>
        /// <param name="text">文本，如果取消输入则为为null</param>
        /// <returns>是否读取成功</returns>
        private bool GetReadText(out string? text)
        {
            System.Console.WriteLine(Questions);

            while (true)
            {
                System.Console.Write("请输入>");
                string? temp = System.Console.ReadLine();
                if (temp is null)
                {
                    if (IsCanCancel)
                    {
                        text = null;
                        return false;
                    }
                    else WriteErrorMessage("无法取消输入");
                }
                else
                {
                    text = temp;
                    return true;
                }
            }
        }

        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="message"></param>
        private void WriteErrorMessage(string? message)
        {
            System.Console.WriteLine($"错误：{message}，{Message_Retry}。");
        }

        /// <summary>
        /// 验证读取到的文本
        /// </summary>
        /// <param name="text">读取到的文本</param>
        /// <param name="result">如果验证成功返回结果</param>
        /// <param name="errorMessage">如果验证失败返回错误消息</param>
        /// <returns>验证成功返回true，验证失败返回false</returns>
        protected abstract bool Verification(string text, out T? result, out string? errorMessage);
    }
}
