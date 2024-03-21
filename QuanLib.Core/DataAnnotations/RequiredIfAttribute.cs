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
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RequiredIfAttribute : ValidationAttribute
    {
        public RequiredIfAttribute(string otherProperty, CompareOperator compareOperator, object? rightValue) : base(ErrorMessageHelper.RequiredIfAttribute)
        {
            ArgumentException.ThrowIfNullOrEmpty(otherProperty, nameof(otherProperty));

            OtherProperty = otherProperty;
            CompareOperator = compareOperator;
            RightValue = rightValue;
            AllowEmptyStrings = false;
        }

        public string OtherProperty { get; }

        public string? OtherPropertyDisplayName { get; private set; }

        public CompareOperator CompareOperator { get; }

        public object? RightValue { get; }

        public bool AllowEmptyStrings { get; set; }

        public override bool RequiresValidationContext => true;

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, OtherPropertyDisplayName ?? OtherProperty, CompareOperator.ToSymbol(), RightValue);
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            ArgumentNullException.ThrowIfNull(validationContext, nameof(validationContext));

            PropertyInfo? otherPropertyInfo = validationContext.ObjectType.GetRuntimeProperty(OtherProperty);
            object? otherPropertyValue = otherPropertyInfo?.GetValue(validationContext.ObjectInstance, null);

            if (!CompareHelper.Compare(otherPropertyValue, RightValue, CompareOperator))
                return null;

            if (AllowEmptyStrings || value is not string stringValue)
            {
                if (value is not null)
                    return null;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(stringValue))
                    return null;
            }

            OtherPropertyDisplayName ??= GetDisplayNameForProperty(otherPropertyInfo);
            string[]? memberNames = validationContext.MemberName is not null ? [validationContext.MemberName] : null;
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), memberNames);
        }

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
