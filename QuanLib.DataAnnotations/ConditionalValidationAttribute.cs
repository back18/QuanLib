using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public abstract class ConditionalValidationAttribute : PropertyValidationAttribute
    {
        public ConditionalValidationAttribute(string otherProperty, CompareOperator compareOperator, object? rightValue, string errorMessage) : base(otherProperty, errorMessage)
        {
            CompareOperator = compareOperator;
            RightValue = rightValue;
        }

        public CompareOperator CompareOperator { get; }

        public object? RightValue { get; }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext, PropertyContext otherPropertyContext)
        {
            if (!CompareHelper.Compare(otherPropertyContext.Value, RightValue, CompareOperator))
                return null;

            return IsValidWhenTrue(value, validationContext, otherPropertyContext);
        }

        protected abstract ValidationResult? IsValidWhenTrue(object? value, ValidationContext validationContext, PropertyContext otherPropertyContext);
    }
}
