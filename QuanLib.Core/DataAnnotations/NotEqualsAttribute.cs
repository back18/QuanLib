﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.DataAnnotations
{
    public class NotEqualsAttribute : CompareAttribute
    {
        public NotEqualsAttribute(string otherProperty) : base(otherProperty)
        {
            ErrorMessage = ErrorMessageHelper.NotEqualsAttribute;
        }

        protected override bool Compare(object? value, object? other)
        {
            return CompareHelper.CompareNotEqual(value, other);
        }
    }
}
