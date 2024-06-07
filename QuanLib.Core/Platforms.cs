using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    [Flags]
    public enum Platforms
    {
        None = 0,

        Windows = 1,

        Linux = 2,

        MacOS = 4
    }
}
