using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.ClocAnalyzer.Services
{
    public interface IClocProcessProvider
    {
        public IClocProcessService CreateService(string clocExePath);
    }
}
