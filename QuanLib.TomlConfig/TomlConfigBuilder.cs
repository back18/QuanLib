using Nett;
using QuanLib.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.TomlConfig
{
    public static class TomlConfigBuilder
    {
        public static TomlTable Build(object value)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            Type type = value.GetType();
            TomlTable tomlTable = Toml.Create(value);

            foreach (var item in tomlTable)
            {
                if (!TryGetMemberInfo(type, item.Key, out var memberInfo))
                    continue;

                TomlObject tomlObject = item.Value;
                IEnumerable<Attribute> attributes = memberInfo.MemberInfo.GetCustomAttributes();
                foreach (Attribute attribute in attributes)
                {
                    if (attribute is DisplayAttribute displayAttribute)
                    {
                        if (!string.IsNullOrEmpty(displayAttribute.Name))
                            tomlObject.AddComment("名称: " + displayAttribute.Name);
                        if (!string.IsNullOrEmpty(displayAttribute.Description))
                        {
                            string[] lines = displayAttribute.Description.Split('\n');
                            tomlObject.AddComment("描述: " + lines[0]);
                            for (int i = 1; i < lines.Length; i++)
                                tomlObject.AddComment("      " + lines[i]);
                        }
                        break;
                    }
                }

                tomlObject.AddComment("默认值: " + Format(memberInfo.GetValue(value)));

                foreach (Attribute attribute in attributes)
                {
                    if (attribute is RequiredAttribute requiredAttribute)
                    {
                        tomlObject.AddComment("- 此项不能为空");
                    }
                    else if (attribute is RangeAttribute rangeAttribute)
                    {
                        tomlObject.AddComment($"- 最小值: {Format(rangeAttribute.Minimum)}, 最大值: {Format(rangeAttribute.Maximum)}");
                    }
                    else if (attribute is MinLengthAttribute minLengthAttribute)
                    {
                        tomlObject.AddComment("- 最小长度为" + minLengthAttribute.Length);
                    }
                    else if (attribute is MaxLengthAttribute maxLengthAttribute)
                    {
                        tomlObject.AddComment("- 最大长度为" + maxLengthAttribute.Length);
                    }
                    else if (attribute is LengthAttribute lengthAttribute)
                    {
                        tomlObject.AddComment("- 最小长度为" + lengthAttribute.MinimumLength);
                        tomlObject.AddComment("- 最大长度为" + lengthAttribute.MaximumLength);
                    }
                    else if (attribute is StringLengthAttribute stringLengthAttribute)
                    {
                        tomlObject.AddComment("- 文本最小长度为" + stringLengthAttribute.MinimumLength);
                        tomlObject.AddComment("- 文本最大长度为" + stringLengthAttribute.MaximumLength);
                    }
                    else if (attribute is AllowedValuesAttribute allowedValuesAttribute)
                    {
                        tomlObject.AddComment("- 允许的值: " + Format(allowedValuesAttribute.Values));
                    }
                    else if (attribute is DeniedValuesAttribute deniedValuesAttribute)
                    {
                        tomlObject.AddComment("- 不允许的值: " + Format(deniedValuesAttribute.Values));
                    }
                    else if (attribute is CompareAttribute compareAttribute)
                    {
                        tomlObject.AddComment($"- 需要与“{compareAttribute.OtherProperty}”匹配");
                    }
                    else if (attribute is FileExtensionsAttribute fileExtensionsAttribute)
                    {
                        tomlObject.AddComment($"- 只支持后缀为{fileExtensionsAttribute.Extensions}的文件");
                    }
                    else if (attribute is PhoneAttribute phoneAttribute)
                    {
                        tomlObject.AddComment("- 需要填写合法的号码");
                    }
                    else if (attribute is EmailAddressAttribute emailAddressAttribute)
                    {
                        tomlObject.AddComment("- 需要填写合法的邮箱地址");
                    }
                    else if (attribute is UrlAttribute urlAttribute)
                    {
                        tomlObject.AddComment("- 需要填写合法的网址");
                    }
                }
            }

            return tomlTable;
        }

        private static bool TryGetMemberInfo(Type type, string name, [MaybeNullWhen(false)] out SerializationMemberInfo result)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

            PropertyInfo? propertyInfo = type.GetProperty(name);
            if (propertyInfo is not null)
            {
                result = new(propertyInfo);
                return true;
            }
            else
            {
                FieldInfo? fieldInfo = type.GetField(name);
                if (fieldInfo is not null)
                {
                    result = new(fieldInfo);
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }
        }

        private static string Format<T>(T? value)
        {
            if (value is null)
                return "null";

            if (value is string str)
                return $"\"{str}\"";

            if (value is IEnumerable enumerable)
            {
                List<string> list = [];
                foreach (var item in enumerable)
                    list.Add(Format(item));
                return $"[{string.Join(", ", list)}]";
            }

            return value.ToString() ?? "null";
        }
    }
}
