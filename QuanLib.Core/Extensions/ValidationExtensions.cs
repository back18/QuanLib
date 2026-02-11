using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QuanLib.Core.Extensions
{
    public static class ValidationExtensions
    {
        public static void ThrowIfFailed(this IValidatable source, string? displayName = null)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            ThrowIfFailed(source.GetValidator(), new ValidationContext(source), displayName);
        }

        public static void ThrowIfFailed(this IValidatable source, ValidationContext validationContext, string? displayName = null)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            ThrowIfFailed(source.GetValidator(), validationContext, displayName);
        }

        public static void ThrowIfFailed(this IValidatableObject source, string? displayName = null)
        {
            ThrowIfFailed(source, new ValidationContext(source), displayName);
        }

        public static void ThrowIfFailed(this IValidatableObject source, ValidationContext validationContext, string? displayName = null)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));
            ArgumentNullException.ThrowIfNull(validationContext, nameof(validationContext));

            IEnumerable<ValidationResult> errors = source.Validate(validationContext);

            if (errors.Any())
            {
                StringBuilder message = new();
                displayName ??= source.GetType().FullName ?? source.GetType().Name;

                message.AppendFormat("验证“{0}”时遇到了以下错误：", displayName);
                message.AppendLine();
                foreach (ValidationResult error in errors)
                    message.AppendLine(error.ErrorMessage);
                message.Length -= Environment.NewLine.Length;

                throw new ValidationException(message.ToString());
            }
        }

        public static ValidationContext Clone(this ValidationContext source, object instance)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));
            ArgumentNullException.ThrowIfNull(instance, nameof(instance));

            return new ValidationContext(instance, source, source.Items);
        }
    }
}
