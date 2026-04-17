using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.ClocAnalyzer.Services
{
    public interface IClocFileParseService
    {
        public FileStats[] Parse(IEnumerable<string> clocOutput, char delimiter = ',');
    }
}
