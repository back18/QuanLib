using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.DataAnnotations
{
    public class GreaterThanOrEqualsAttribute : CompareAttribute
    {
        public GreaterThanOrEqualsAttribute(string otherProperty) : base(otherProperty)
        {
            ErrorMessage = ErrorMessageHelper.GreaterThanOrEqualsAttribute;
        }

        protected override bool Compare(object? value, object? other)
        {
            return CompareHelper.CompareGreaterThanOrEquals(value, other);
        }
    }
}
