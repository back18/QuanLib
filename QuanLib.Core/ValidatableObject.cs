using QuanLib.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QuanLib.Core
{
    public class ValidatableObject : IValidatableObject
    {
        public ValidatableObject(object instance)
        {
            ArgumentNullException.ThrowIfNull(instance, nameof(instance));

            _instance = instance;
        }

        private readonly object _instance;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = [];
            Validator.TryValidateObject(_instance, validationContext, results, true);

            if (_instance is IValidatable validatable)
            {
                var properties = validatable.GetValidatableProperties();
                foreach (var property in properties)
                {
                    var errors = property.GetValidator().Validate(validationContext.Clone(property));
                    results.AddRange(errors);
                }
            }

            if (results.Count > 0)
                return results;
            else
                return Array.Empty<ValidationResult>();
        }
    }
}
