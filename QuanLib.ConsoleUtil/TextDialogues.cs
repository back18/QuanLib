using QuanLib.ExceptionHelper;
using QuanLib.Verification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace QuanLib.ConsoleUtil
{
    /// <summary>
    /// 文本对话器
    /// </summary>
    public class TextDialogues : Dialogues<string?>
    {
        public TextDialogues(string questions, int minLen, int maxLen, bool isCanCancel = false) : base(questions, isCanCancel)
        {
            MinLength= minLen;
            MaxLength= maxLen;
        }

        /// <summary>
        /// 文本长度下限
        /// </summary>
        public int MinLength
        {
            get => _MinLength;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(MinLength), value, MessageHelper.GetMessage(MessageKey.文本长度下限小于0));
                else _MinLength= value;
            }
        }
        private int _MinLength;


        /// <summary>
        /// 文本长度上限
        /// </summary>
        public int MaxLength
        {
            get => _MaxLength;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(MaxLength), value, MessageHelper.GetMessage(MessageKey.文本长度上限小于0));
                else _MaxLength = value;
            }
        }
        private int _MaxLength;

        protected override bool Verification(string text, out string? result, out string? errorMessage)
        {
            if (ValidateHelper.TextLen(text, MinLength, MaxLength, out errorMessage))
            {
                result = text;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }
    }
}
