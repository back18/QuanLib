using Nett;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.NettToolkit
{
    public static class TomlTableExtension
    {
        public static T? GetValueAs<T>(this TomlTable source, string? key) where T : TomlObject
        {
            if (key is not null && source.TryGetValue(key, out var tomlObject) && tomlObject is T value)
                return value;
            else
                return null;
        }
    }
}
