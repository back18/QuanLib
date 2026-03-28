using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QuanLib.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public class FileSystemPathAttribute : ValidationAttribute
    {
        private const int MaxPathLength = 260;
        private static readonly char[] _invalidPathChars = Path.GetInvalidPathChars();

        public FileSystemPathAttribute() : base(ErrorMessageHelper.FileSystemPathAttribute) { }

        public override bool IsValid(object? value)
        {
            if (value is not string path || string.IsNullOrEmpty(path) || path.Length > MaxPathLength)
                return false;

            return !path.Any(c => _invalidPathChars.Contains(c));
        }
    }
}
