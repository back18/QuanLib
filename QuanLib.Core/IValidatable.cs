using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QuanLib.Core
{
    public interface IValidatable
    {
        public IValidatableObject GetValidator();

        public IEnumerable<IValidatable> GetValidatableProperties();
    }
}
