using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.DataAnnotations
{
    public class GreaterThanOrEqualsAttribute : CompareAttribute
    {
        public GreaterThanOrEqualsAttribute(string otherProperty) : base(otherProperty, ErrorMessageHelper.GreaterThanOrEqualsAttribute) { }

        protected override bool Compare(object? value, object? other)
        {
            return CompareHelper.CompareGreaterThanOrEquals(value, other);
        }
    }
}
