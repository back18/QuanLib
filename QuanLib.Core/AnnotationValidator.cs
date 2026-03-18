using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QuanLib.Core
{
    public class AnnotationValidator : IValidator
    {
        public AnnotationValidator(ValidationAttribute[] validationAttributes)
        {
            ArgumentNullException.ThrowIfNull(validationAttributes, nameof(validationAttributes));

            _validationAttributes = validationAttributes;
        }

        private readonly ValidationAttribute[] _validationAttributes;

        public bool IsValid(object? value)
        {
            if (_validationAttributes.Length == 0)
                return true;

            foreach(ValidationAttribute validationAttribute in _validationAttributes)
            {
                if (!validationAttribute.IsValid(value))
                    return false;
            }

            return true;
        }

        public void Validate(object? value, string name)
        {
            if (_validationAttributes.Length == 0)
                return;

            foreach (ValidationAttribute validationAttribute in _validationAttributes)
                validationAttribute.Validate(value, name);
        }
    }
}
