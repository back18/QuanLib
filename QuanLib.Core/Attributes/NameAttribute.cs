using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public sealed class NameAttribute : Attribute
    {
        public NameAttribute(string name)
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));

            Name = name;
        }

        public string Name { get; }
    }
}
