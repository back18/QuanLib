using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class PropertyValidationAttribute : ValidationAttribute
    {
        public PropertyValidationAttribute(string otherProperty, string errorMessage) : base(errorMessage)
        {
            ArgumentNullException.ThrowIfNull(otherProperty);

            OtherProperty = otherProperty;
        }

        public string OtherProperty { get; }

        public string? OtherPropertyDisplayName { get; private set; }

        public override bool RequiresValidationContext => true;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            PropertyContext propertyContext = new(validationContext, OtherProperty);
            OtherPropertyDisplayName = propertyContext.DisplayName;
            return IsValid(value, validationContext, propertyContext);
        }

        protected abstract ValidationResult? IsValid(object? value, ValidationContext validationContext, PropertyContext otherPropertyContext);
    }
}
