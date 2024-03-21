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

        public const string RequiredAttribute = Prefix + "不能为空";

        public const string RequiredIfAttribute = Prefix + "当另一个属性“{1}”的值 {2} {3} 时，当前属性的值不能为空";

        public const string RangeAttribute = Prefix + "范围应该为{1}到{2}";

        public const string MinLengthAttribute = Prefix + "长度最小为{1}";

        public const string MaxLengthAttribute = Prefix + "长度最大为{1}";

        public const string LengthAttribute = Prefix + "长度范围应该为{1}到{2}";

        public const string StringLengthAttribute = Prefix + "文本长度范围应该为{2}到{1}";

        public const string AllowedValuesAttribute = Prefix + "无效的值";

        public const string DeniedValuesAttribute = Prefix + "无效的值";

        public const string CompareAttribute = Prefix + "需要与另一个属性“{1}”的值匹配";

        public const string EqualsAttribute = Prefix + "需要与另一个属性“{1}”的值相等";

        public const string NotEqualsAttribute = Prefix + "不能与另一个属性“{1}”的值相等";

        public const string LessThanAttribute = Prefix + "需要小于另一个属性“{1}”的值";

        public const string LessThanOrEqualsAttribute = Prefix + "需要小于或等于另一个属性“{1}”的值";

        public const string GreaterThanAttribute = Prefix + "需要大于另一个属性“{1}”的值";

        public const string GreaterThanOrEqualsAttribute = Prefix + "需要大于或等于另一个属性“{1}”的值";

        public const string UrlAttribute = Prefix + "URL格式不合法";

        public const string EmailAddressAttribute = Prefix + "邮箱地址格式不合法";

        public const string PhoneAttribute = Prefix + "电话号码格式不合法";

        public const string CreditCardAttribute = Prefix + "信用卡号格式不合法";

        public const string FileExistsAttribute = Prefix + "指定的路径文件不存在";

        public const string DirectoryExistsAttribute = Prefix + "指定的路径目录不存在";

        public const string FileExtensionsAttribute = Prefix + "文件扩展名只能为{1}";
    }
}
