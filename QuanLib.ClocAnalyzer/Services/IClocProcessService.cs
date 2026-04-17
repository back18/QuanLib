using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.ClocAnalyzer.Services
{
    public interface IClocProcessService
    {
        public Task<string[]> ExecuteAsync(string analyzeTargetPath, ClocOptions? options = null, IProgress<int>? progress = null);
    }
}
