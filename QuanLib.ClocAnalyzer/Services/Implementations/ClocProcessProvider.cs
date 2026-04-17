using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.ClocAnalyzer.Services.Implementations
{
    public class ClocProcessProvider : IClocProcessProvider
    {
        public ClocProcessProvider(IClocArgumentsProvider argumentsProvider)
        {
            ArgumentNullException.ThrowIfNull(argumentsProvider, nameof(argumentsProvider));

            _argumentsProvider = argumentsProvider;
        }

        private readonly IClocArgumentsProvider _argumentsProvider;

        public IClocProcessService CreateService(string clocExePath)
        {
            return new ClocProcessService(clocExePath, _argumentsProvider);
        }
    }
}
