using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Parsing
{
    public delegate T ParseHandler<T>(string s, IFormatProvider? provider);
}
