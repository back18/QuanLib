using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.Core
{
    public interface IValidator
    {
        public bool IsValid(object? value);

        public void Validate(object? value, string name);
    }
}
