using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.DataAnnotations
{
    public class GreaterThanAttribute : CompareValidationAttribute
    {
        public GreaterThanAttribute(string otherProperty) : base(otherProperty, ErrorMessageHelper.GreaterThanAttribute) { }

        protected override bool Compare(object? value, object? other)
        {
            return CompareHelper.CompareGreaterThan(value, other);
        }
    }
}
