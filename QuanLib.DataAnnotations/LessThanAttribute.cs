﻿using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.DataAnnotations
{
    public class LessThanAttribute : CompareValidationAttribute
    {
        public LessThanAttribute(string otherProperty) : base(otherProperty, ErrorMessageHelper.LessThanAttribute) { }

        protected override bool Compare(object? value, object? other)
        {
            return CompareHelper.CompareLessThan(value, other);
        }
    }
}
