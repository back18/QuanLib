using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.ClocAnalyzer.Services
{
    public interface IClocArgumentsProvider
    {
        public string BuildArguments(string analyzeTargetPath, ClocOptions? options = null);
    }
}
