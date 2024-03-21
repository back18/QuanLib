using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.DataAnnotations
{
    public class GreaterThanAttribute : CompareAttribute
    {
        public GreaterThanAttribute(string otherProperty) : base(otherProperty)
        {
            ErrorMessage = ErrorMessageHelper.GreaterThanAttribute;
        }

        protected override bool Compare(object? value, object? other)
        {
            return CompareHelper.CompareGreaterThan(value, other);
        }
    }
}
