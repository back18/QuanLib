using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.DataAnnotations
{
    public class LessThanOrEqualsAttribute : CompareAttribute
    {
        public LessThanOrEqualsAttribute(string otherProperty) : base(otherProperty)
        {
            ErrorMessage = ErrorMessageHelper.LessThanOrEqualsAttribute;
        }

        protected override bool Compare(object? value, object? other)
        {
            return CompareHelper.CompareLessThanOrEquals(value, other);
        }
    }
}
