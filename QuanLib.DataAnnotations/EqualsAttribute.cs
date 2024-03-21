using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.DataAnnotations
{
    public class EqualsAttribute : CompareAttribute
    {
        public EqualsAttribute(string otherProperty) : base(otherProperty)
        {
            ErrorMessage = ErrorMessageHelper.EqualsAttribute;
        }

        protected override bool Compare(object? value, object? other)
        {
            return CompareHelper.CompareEqual(value, other);
        }
    }
}
