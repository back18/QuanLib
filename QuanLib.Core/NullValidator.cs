using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public static class NullValidator
    {
        public static bool TryValidateObject(object instance, out FieldInfo[] nullFields, out PropertyInfo[] nullProperties)
        {
            ArgumentNullException.ThrowIfNull(instance, nameof(instance));

            Type type = instance.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance);
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance);
            List<FieldInfo> nullFieldList = new();
            List<PropertyInfo> nullPropertyList = new();

            foreach (FieldInfo field in fields)
            {
                if (field.GetCustomAttribute<NullableAttribute>() is not null)
                    continue;
                if (field.GetValue(instance) is null)
                    nullFieldList.Add(field);
            }

            foreach (PropertyInfo property in properties)
            {
                if (property.GetCustomAttribute<NullableAttribute>() is not null)
                    continue;
                if (property.GetValue(instance) is null)
                    nullPropertyList.Add(property);
            }

            nullFields = nullFieldList.ToArray();
            nullProperties = nullPropertyList.ToArray();
            return nullFields.Length == 0 && nullProperties.Length == 0;
        }

        public static void ValidateObject(object instance, string name)
        {
            ArgumentNullException.ThrowIfNull(instance, nameof(instance));
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

            if (TryValidateObject(instance, out var nullFields, out var nullProperties))
                return;

            StringBuilder message = new();
            message.AppendLine("对象的一个或多个成员为null：");
            foreach (var field in nullFields)
                message.AppendLine($"字段“{field.Name}”为null");
            foreach (var property in nullProperties)
                message.AppendLine($"属性“{property.Name}”为null");

            throw new ArgumentException(message.ToString().TrimEnd(), name);
        }
    }
}
