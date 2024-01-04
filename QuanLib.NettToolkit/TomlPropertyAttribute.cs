﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.NettToolkit
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public class TomlPropertyAttribute : Attribute
    {
        public TomlPropertyAttribute(string propertyName)
        {
            ArgumentException.ThrowIfNullOrEmpty(propertyName, nameof(propertyName));

            PropertyName = propertyName;
        }

        public string PropertyName { get; }
    }
}
