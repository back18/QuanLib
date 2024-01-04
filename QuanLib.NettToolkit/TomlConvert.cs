using Nett;
using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuanLib.NettToolkit
{
    public static partial class TomlConvert
    {
        public static void PopulateObject(string toml, object target)
        {
            ArgumentException.ThrowIfNullOrEmpty(toml, nameof(toml));
            ArgumentNullException.ThrowIfNull(target, nameof(target));

            TomlTable tomlTable = Toml.ReadString(toml);
            PopulateObject(tomlTable, target);
        }

        public static void PopulateObject(string toml, TomlSettings settings, object target)
        {
            ArgumentException.ThrowIfNullOrEmpty(toml, nameof(toml));
            ArgumentNullException.ThrowIfNull(settings, nameof(settings));
            ArgumentNullException.ThrowIfNull(target, nameof(target));

            TomlTable tomlTable = Toml.ReadString(toml, settings);
            PopulateObject(tomlTable, target);
        }

        public static void PopulateObject(TomlTable tomlTable, object target)
        {
            ArgumentNullException.ThrowIfNull(tomlTable, nameof(tomlTable));
            ArgumentNullException.ThrowIfNull(target, nameof(target));

            Type type = target.GetType();
            Dictionary<string, SerializationMemberInfo> members = GetMembers(target);
            foreach (var item in tomlTable)
            {
                if (members.TryGetValue(item.Key, out var member))
                {
                    if (item.Value is TomlTable table)
                    {
                        object? value = member.GetValue(target);

                        if (value is not null)
                        {
                            PopulateObject(table, value);
                            member.SetValue(target, value);
                            continue;
                        }

                        value = DeserializeObject(item.Value, member.MemberType);
                        member.SetValue(target, value);
                    }
                    else
                    {
                        object? value = DeserializeObject(item.Value, member.MemberType);
                        member.SetValue(target, value);
                    }
                }
            }
        }

        public static T DeserializeObject<T>(string toml)
        {
            ArgumentException.ThrowIfNullOrEmpty(toml, nameof(toml));

            TomlTable tomlTable = Toml.ReadString(toml);
            return DeserializeObject<T>(tomlTable);
        }

        public static T DeserializeObject<T>(string toml, TomlSettings settings)
        {
            ArgumentException.ThrowIfNullOrEmpty(toml, nameof(toml));
            ArgumentNullException.ThrowIfNull(settings, nameof(settings));

            TomlTable tomlTable = Toml.ReadString(toml, settings);
            return DeserializeObject<T>(tomlTable);
        }

        public static T DeserializeObject<T>(TomlObject tomlObject)
        {
            return (T)DeserializeObject(tomlObject, typeof(T));
        }

        public static object DeserializeObject(TomlObject tomlObject, Type type)
        {
            ArgumentNullException.ThrowIfNull(tomlObject, nameof(tomlObject));
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            if (tomlObject is TomlArray tomlArray)
            {
                return tomlArray.Get(type);
            }
            else if (tomlObject is TomlValue tomlValue)
            {
                return tomlValue.Get(type);
            }
            else if (tomlObject is TomlTableArray tomlTableArray)
            {
                TomlTable[] tomlTables = new TomlTable[tomlTableArray.Count];
                tomlTableArray.Items.CopyTo(tomlTables, 0);
                Type elementType = type.GetElementType() ?? throw new InvalidOperationException();
                Array array = Array.CreateInstance(elementType, tomlTables.Length);
                for (int i = 0; i < tomlTables.Length; i++)
                    array.SetValue(DeserializeObject(tomlTables[i], elementType), i);
                return array;
            }
            else if (tomlObject is TomlTable tomlTable)
            {
                object result = Activator.CreateInstance(type) ?? throw new InvalidOperationException($"无法创建 {type} 类型的对象");
                Dictionary<string, SerializationMemberInfo> members = GetMembers(result);
                foreach (var item in tomlTable)
                {
                    if (members.TryGetValue(item.Key, out var member))
                        member.SetValue(result, DeserializeObject(item.Value, member.MemberType));
                }

                return result;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private static Dictionary<string, SerializationMemberInfo> GetMembers(object obj)
        {
            ArgumentNullException.ThrowIfNull(obj, nameof(obj));

            Dictionary<string, SerializationMemberInfo> result = [];
            List<SerializationMemberInfo> memberInfos = [];
            Type type = obj.GetType();
            memberInfos.AddRange(type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(s => new SerializationMemberInfo(s)));
            memberInfos.AddRange(type.GetFields(BindingFlags.Public | BindingFlags.Instance).Select(s => new SerializationMemberInfo(s)));

            foreach (var memberInfo in memberInfos)
            {
                string propertyName;
                TomlPropertyAttribute? attribute = memberInfo.MemberInfo.GetCustomAttribute<TomlPropertyAttribute>();
                if (attribute is null)
                    propertyName = memberInfo.MemberName;
                else
                    propertyName = FormatPropertyName(attribute.PropertyName, obj);

                result.Add(propertyName, memberInfo);
            }

            return result;
        }

        private static string FormatPropertyName(string propertyName, object obj)
        {
            ArgumentException.ThrowIfNullOrEmpty(propertyName, nameof(propertyName));
            ArgumentNullException.ThrowIfNull(obj, nameof(obj));

            Type type = obj.GetType();
            MatchCollection matches = GetBraces().Matches(propertyName);
            foreach (Match match in matches.Cast<Match>())
            {
                string memberName = match.Groups[1].Value;
                PropertyInfo? propertyInfo = type.GetProperty(memberName);
                FieldInfo? fieldInfo = type.GetField(memberName);
                if (propertyInfo is null && fieldInfo is null)
                    throw new InvalidOperationException($"无法从类型 {type} 获取名称为 {memberName} 的属性或字段");

                string value = propertyInfo?.GetValue(obj)?.ToString() ?? fieldInfo?.GetValue(obj)?.ToString() ?? string.Empty;
                propertyName = propertyName.Replace(match.Groups[0].Value, value);
            }

            return propertyName;
        }

        [GeneratedRegex(@"\{([^{}]+)\}")]
        private static partial Regex GetBraces();
    }
}
