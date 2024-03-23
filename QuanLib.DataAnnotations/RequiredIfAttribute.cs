using QuanLib.Core;
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
    public class RequiredIfAttribute : ConditionalValidationAttribute
    {
        public RequiredIfAttribute(string otherProperty, CompareOperator compareOperator, object? rightValue) : base(otherProperty, compareOperator, rightValue, ErrorMessageHelper.RequiredIfAttribute)
        {
            AllowEmptyStrings = false;
        }

        public bool AllowEmptyStrings { get; set; }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, OtherPropertyDisplayName ?? OtherProperty, CompareOperator.ToSymbol(), RightValue);
        }

        protected override ValidationResult? IsValidWhenTrue(object? value, ValidationContext validationContext, PropertyContext otherPropertyContext)
        {
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

            string[]? memberNames = validationContext.MemberName is not null ? [validationContext.MemberName] : null;
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), memberNames);
        }
    }
}
