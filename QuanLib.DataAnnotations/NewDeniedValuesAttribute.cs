using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public class NewDeniedValuesAttribute : ValidationAttribute
    {
        public NewDeniedValuesAttribute(params object?[] values) : base(ErrorMessageHelper.NewDeniedValuesAttribute)
        {
            ArgumentNullException.ThrowIfNull(values, nameof(values));

            Values = values.AsReadOnly();
        }

        public ReadOnlyCollection<object?> Values { get; }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, ObjectFormatter.Format(Values));
        }

        public override bool IsValid(object? value)
        {
            return !Values.Contains(value);
        }
    }
}
