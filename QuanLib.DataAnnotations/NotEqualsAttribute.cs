using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.DataAnnotations
{
    public class NotEqualsAttribute : CompareAttribute
    {
        public NotEqualsAttribute(string otherProperty) : base(otherProperty, ErrorMessageHelper.NotEqualsAttribute) { }

        protected override bool Compare(object? value, object? other)
        {
            return CompareHelper.CompareNotEqual(value, other);
        }
    }
}
