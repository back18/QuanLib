﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Game
{
    /// <summary>
    /// 二维方向
    /// </summary>
    [Flags]
    public enum Direction
    {
        None = 0,

        Top = 1,

        Bottom = 2,

        Left = 4,

        Right = 8
    }
}
