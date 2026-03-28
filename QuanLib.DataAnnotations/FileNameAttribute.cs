using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QuanLib.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public class FileNameAttribute : ValidationAttribute
    {
        private const int MaxFileNameLength = 255;
        private static readonly char[] _invalidFileNameChars = Path.GetInvalidFileNameChars();

        public FileNameAttribute() : base(ErrorMessageHelper.FileNameAttribute) { }

        public override bool IsValid(object? value)
        {
            if (value is not string fileName || string.IsNullOrEmpty(fileName) || fileName.Length > MaxFileNameLength)
                return false;

            return !fileName.Any(c => _invalidFileNameChars.Contains(c));
        }
    }
}
