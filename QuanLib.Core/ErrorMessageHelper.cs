using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public static class ErrorMessageHelper
    {
        private const string Prefix = "属性“{0}”的值验证失败：";

        public const string Required = Prefix + "不能为空";

        public const string Range = Prefix + "范围应该为{1}到{2}";

        public const string MinLength = Prefix + "长度最小为{1}";

        public const string MaxLength = Prefix + "长度最大为{1}";

        public const string Length = Prefix + "长度范围应该为{1}到{2}";

        public const string StringLength = Prefix + "文本长度应该为{2}到{1}";

        public const string AllowedValues = Prefix + "不合法";

        public const string DeniedValues = Prefix + "不合法";

        public const string Compare = Prefix + "必须与另一个属性“{1}”的值匹配";

        public const string FileExtensions = Prefix + "文件扩展名只能为{1}";

        public const string Phone = Prefix + "号码不合法";

        public const string EmailAddress = Prefix + "邮箱地址不合法";

        public const string Url = Prefix + "网址不合法";

        public static string Format(string text)
        {
            ArgumentException.ThrowIfNullOrEmpty(text, nameof(text));

            return Prefix + text;
        }
    }
}
