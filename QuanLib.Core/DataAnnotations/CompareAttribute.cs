using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public abstract class CompareAttribute : ValidationAttribute
    {
        public CompareAttribute(string otherProperty) : base(ErrorMessageHelper.CompareAttribute)
        {
            ArgumentNullException.ThrowIfNull(otherProperty);

            OtherProperty = otherProperty;
        }

        public string OtherProperty { get; }

        public string? OtherPropertyDisplayName { get; private set; }

        public override bool RequiresValidationContext => true;

        public override string FormatErrorMessage(string name)
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));
            
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, OtherPropertyDisplayName ?? OtherProperty);
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            ArgumentNullException.ThrowIfNull(validationContext, nameof(validationContext));

            PropertyInfo? otherPropertyInfo = validationContext.ObjectType.GetRuntimeProperty(OtherProperty);
            object? otherPropertyValue = otherPropertyInfo?.GetValue(validationContext.ObjectInstance, null);

            if (!Compare(value, otherPropertyValue))
            {
                OtherPropertyDisplayName ??= GetDisplayNameForProperty(otherPropertyInfo);
                string[]? memberNames = validationContext.MemberName is not null ? [validationContext.MemberName] : null;
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), memberNames);
            }

            return null;
        }

        protected abstract bool Compare(object? value, object? other);

        private string? GetDisplayNameForProperty(PropertyInfo? propertyInfo)
        {
            DisplayAttribute? displayAttribute = propertyInfo?.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute is not null)
                return displayAttribute.GetName();
            else
                return OtherProperty;
        }
    }
}
