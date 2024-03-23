using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public abstract class CompareAttribute : PropertyValidationAttribute
    {
        public CompareAttribute(string otherProperty, string errorMessage) : base(otherProperty, errorMessage) { }

        public override string FormatErrorMessage(string name)
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));

            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, OtherPropertyDisplayName ?? OtherProperty);
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext, PropertyContext otherPropertyContext)
        {
            if (!Compare(value, otherPropertyContext.Value))
            {
                string[]? memberNames = validationContext.MemberName is not null ? [validationContext.MemberName] : null;
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), memberNames);
            }

            return null;
        }

        protected abstract bool Compare(object? value, object? other);
    }
}
