using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public static class ValidationHelper
    {
        public static void Validate(object instance, string name)
        {
            ArgumentNullException.ThrowIfNull(instance, nameof(instance));
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

            List<ValidationResult> results = [];
            if (!Validator.TryValidateObject(instance, new(instance), results, true))
            {
                StringBuilder message = new();
                message.AppendLine();
                int count = 0;

                foreach (var result in results)
                {
                    message.AppendLine(result.ErrorMessage);
                    count++;
                }

                if (count > 0)
                {
                    message.Insert(0, $"解析“{name}”时遇到{count}个错误：");
                    throw new ValidationException(message.ToString().TrimEnd());
                }
            }
        }
    }
}
