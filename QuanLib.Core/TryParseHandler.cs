using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public delegate bool TryParseHandler<T>([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out T result);
}
