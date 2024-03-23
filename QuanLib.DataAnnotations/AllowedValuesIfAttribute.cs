using QuanLib.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.DataAnnotations
{
    public class AllowedValuesIfAttribute : ConditionalValidationAttribute
    {
        public AllowedValuesIfAttribute(string otherProperty, CompareOperator compareOperator, object? rightValue, params object?[] values) : base(otherProperty, compareOperator, rightValue, ErrorMessageHelper.AllowedValuesIfAttribute)
        {
            ArgumentNullException.ThrowIfNull(values, nameof(values));

            Values = values.AsReadOnly();
        }

        public ReadOnlyCollection<object?> Values { get; }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, OtherPropertyDisplayName ?? OtherProperty, CompareOperator.ToSymbol(), RightValue, ObjectFormatter.Format(Values));
        }

        protected override ValidationResult? IsValidWhenTrue(object? value, ValidationContext validationContext, PropertyContext otherPropertyContext)
        {
            if (Values.Contains(value))
                return null;

            string[]? memberNames = validationContext.MemberName is not null ? [validationContext.MemberName] : null;
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), memberNames);
        }
    }
}
