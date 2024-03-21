using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.DataAnnotations
{
    public class LessThanAttribute : CompareAttribute
    {
        public LessThanAttribute(string otherProperty) : base(otherProperty)
        {
            ErrorMessage = ErrorMessageHelper.LessThanAttribute;
        }

        protected override bool Compare(object? value, object? other)
        {
            return CompareHelper.CompareLessThan(value, other);
        }
    }
}
