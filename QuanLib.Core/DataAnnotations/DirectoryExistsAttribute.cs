using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public class DirectoryExistsAttribute : ValidationAttribute
    {
        public DirectoryExistsAttribute() : base(ErrorMessageHelper.DirectoryExistsAttribute) { }

        public override bool IsValid(object? value)
        {
            if (value is string text && Directory.Exists(text))
                return true;
            else
                return false;
        }
    }
}
