using Nett;
using QuanLib.Core;
using QuanLib.DataAnnotations;
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
        public static TomlTable Build(object instance)
        {
            ArgumentNullException.ThrowIfNull(instance, nameof(instance));

            TomlTable tomlTable = Toml.Create(instance);
            BuildComment(tomlTable, instance);
            return tomlTable;
        }

        private static void BuildComment(TomlTable tomlTable, object instance)
        {
            ArgumentNullException.ThrowIfNull(tomlTable, nameof(tomlTable));
            ArgumentNullException.ThrowIfNull(instance, nameof(instance));

            Type type = instance.GetType();
            foreach (var item in tomlTable)
            {
                if (!TryGetMemberInfo(type, item.Key, out var memberInfo))
                    continue;

                TomlObject tomlObject = item.Value;
                object? defaultValue = memberInfo.GetValue(instance);
                BuildComment(tomlObject, memberInfo, defaultValue);

                if (defaultValue is not null && tomlObject is TomlTable childTomlTable)
                    BuildComment(childTomlTable, defaultValue);
            }
        }

        private static void BuildComment(TomlObject tomlObject, SerializationMemberInfo memberInfo, object? defaultValue)
        {
            ArgumentNullException.ThrowIfNull(tomlObject, nameof(tomlObject));
            ArgumentNullException.ThrowIfNull(memberInfo, nameof(memberInfo));

            IEnumerable<Attribute> attributes = memberInfo.MemberInfo.GetCustomAttributes();

            tomlObject.AddComment(string.Empty);

            if (attributes.OfType<DisplayAttribute>().FirstOrDefault() is DisplayAttribute displayAttribute)
            {
                string? name = displayAttribute.GetName();
                string? description = displayAttribute.GetDescription();

                if (!string.IsNullOrEmpty(name))
                    tomlObject.AddComment("名称: " + name);

                if (!string.IsNullOrEmpty(description))
                {
                    string[] lines = description.Split('\n');
                    tomlObject.AddComment("描述: " + lines[0]);
                    for (int i = 1; i < lines.Length; i++)
                        tomlObject.AddComment("      " + lines[i]);
                }
            }

            tomlObject.AddComment("默认值: " + ObjectFormatter.Format(defaultValue));

            foreach (Attribute attribute in attributes)
            {
                if (attribute is RequiredAttribute requiredAttribute)
                {
                    tomlObject.AddComment("约束: 不能为空");
                }
                else if (attribute is RequiredIfAttribute requiredIfAttribute)
                {
                    tomlObject.AddComment($"约束: 当另一个属性“{requiredIfAttribute.OtherProperty}”的值 {requiredIfAttribute.CompareOperator.ToSymbol()} {ObjectFormatter.Format(requiredIfAttribute.RightValue)} 时，当前属性的值不能为空");
                }
                else if (attribute is RangeAttribute rangeAttribute)
                {
                    tomlObject.AddComment($"约束: 最小值: {ObjectFormatter.Format(rangeAttribute.Minimum)}, 最大值: {ObjectFormatter.Format(rangeAttribute.Maximum)}");
                }
                else if (attribute is MinLengthAttribute minLengthAttribute)
                {
                    tomlObject.AddComment("约束: 最小长度为" + minLengthAttribute.Length);
                }
                else if (attribute is MaxLengthAttribute maxLengthAttribute)
                {
                    tomlObject.AddComment("约束: 最大长度为" + maxLengthAttribute.Length);
                }
                else if (attribute is LengthAttribute lengthAttribute)
                {
                    tomlObject.AddComment("约束: 最小长度为" + lengthAttribute.MinimumLength);
                    tomlObject.AddComment("约束: 最大长度为" + lengthAttribute.MaximumLength);
                }
                else if (attribute is StringLengthAttribute stringLengthAttribute)
                {
                    tomlObject.AddComment("约束: 文本最小长度为" + stringLengthAttribute.MinimumLength);
                    tomlObject.AddComment("约束: 文本最大长度为" + stringLengthAttribute.MaximumLength);
                }
                else if (attribute is AllowedValuesAttribute allowedValuesAttribute)
                {
                    tomlObject.AddComment("约束: 允许的值: " + ObjectFormatter.Format(allowedValuesAttribute.Values));
                }
                else if (attribute is DeniedValuesAttribute deniedValuesAttribute)
                {
                    tomlObject.AddComment("约束: 不允许的值: " + ObjectFormatter.Format(deniedValuesAttribute.Values));
                }
                else if (attribute is NewAllowedValuesAttribute newAllowedValuesAttribute)
                {
                    tomlObject.AddComment("约束: 允许的值: " + ObjectFormatter.Format(newAllowedValuesAttribute.Values));
                }
                else if (attribute is NewDeniedValuesAttribute newDeniedValuesAttribute)
                {
                    tomlObject.AddComment("约束: 不允许的值: " + ObjectFormatter.Format(newDeniedValuesAttribute.Values));
                }
                else if (attribute is AllowedValuesIfAttribute allowedValuesIfAttribute)
                {
                    tomlObject.AddComment($"约束: 当另一个属性“{allowedValuesIfAttribute.OtherProperty}”的值 {allowedValuesIfAttribute.CompareOperator.ToSymbol()} {ObjectFormatter.Format(allowedValuesIfAttribute.RightValue)} 时，当前属性的值只能为：{ObjectFormatter.Format(allowedValuesIfAttribute.Values)}");
                }
                else if (attribute is DeniedValuesIfAttribute deniedValuesIfAttribute)
                {
                    tomlObject.AddComment($"约束: 当另一个属性“{deniedValuesIfAttribute.OtherProperty}”的值 {deniedValuesIfAttribute.CompareOperator.ToSymbol()} {ObjectFormatter.Format(deniedValuesIfAttribute.RightValue)} 时，当前属性的值不能为：{ObjectFormatter.Format(deniedValuesIfAttribute.Values)}");
                }
                else if (attribute is CompareAttribute compareAttribute)
                {
                    tomlObject.AddComment($"约束: 需要与另一个属性“{compareAttribute.OtherProperty}”的值匹配");
                }
                else if (attribute is EqualsAttribute equalsAttribute)
                {
                    tomlObject.AddComment($"约束: 需要与另一个属性“{equalsAttribute.OtherProperty}”的值相等");
                }
                else if (attribute is NotEqualsAttribute notEqualsAttribute)
                {
                    tomlObject.AddComment($"约束: 不能与另一个属性“{notEqualsAttribute.OtherProperty}”的值相等");
                }
                else if (attribute is LessThanAttribute lessThanAttribute)
                {
                    tomlObject.AddComment($"约束: 需要小于另一个属性“{lessThanAttribute.OtherProperty}”的值");
                }
                else if (attribute is LessThanOrEqualsAttribute lessThanOrEqualsAttribute)
                {
                    tomlObject.AddComment($"约束: 需要小于或等于另一个属性“{lessThanOrEqualsAttribute.OtherProperty}”的值");
                }
                else if (attribute is GreaterThanAttribute greaterThanAttribute)
                {
                    tomlObject.AddComment($"约束: 需要大于另一个属性“{greaterThanAttribute.OtherProperty}”的值");
                }
                else if (attribute is GreaterThanOrEqualsAttribute greaterThanOrEqualsAttribute)
                {
                    tomlObject.AddComment($"约束: 需要大于或等于另一个属性“{greaterThanOrEqualsAttribute.OtherProperty}”的值");
                }
                else if (attribute is UrlAttribute)
                {
                    tomlObject.AddComment("约束: 需要填写合法的URL");
                }
                else if (attribute is PhoneAttribute)
                {
                    tomlObject.AddComment("约束: 需要填写合法的电话号码");
                }
                else if (attribute is EmailAddressAttribute)
                {
                    tomlObject.AddComment("约束: 需要填写合法的邮箱地址");
                }
                else if (attribute is CreditCardAttribute)
                {
                    tomlObject.AddComment("约束: 需要填写合法的信用卡号");
                }
                else if (attribute is FileExistsAttribute)
                {
                    tomlObject.AddComment("约束: 需要填写有效的文件路径");
                }
                else if (attribute is DirectoryExistsAttribute)
                {
                    tomlObject.AddComment("约束: 需要填写有效的目录路径");
                }
                else if (attribute is FileExtensionsAttribute fileExtensionsAttribute)
                {
                    tomlObject.AddComment($"约束: 文件扩展名只能为“{fileExtensionsAttribute.Extensions}”");
                }
            }
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
    }
}
