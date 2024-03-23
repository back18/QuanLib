using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.DataAnnotations
{
    public class PropertyContext
    {
        public PropertyContext(ValidationContext validationContext, string propertyName)
        {
            PropertyInfo = validationContext.ObjectType.GetRuntimeProperty(propertyName);
            Value = PropertyInfo?.GetValue(validationContext.ObjectInstance, null);

            DisplayAttribute? displayAttribute = PropertyInfo?.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute is not null)
                DisplayName = displayAttribute.GetName();
            else
                DisplayName = propertyName;
        }

        public PropertyInfo? PropertyInfo { get; }

        public object? Value { get; }

        public string? DisplayName { get; }
    }
}
