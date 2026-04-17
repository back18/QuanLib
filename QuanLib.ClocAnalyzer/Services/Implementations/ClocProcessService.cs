using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace QuanLib.ClocAnalyzer.Services.Implementations
{
    public class ClocProcessService : IClocProcessService
    {
        public ClocProcessService(string clocExePath, IClocArgumentsProvider argumentsProvider)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(clocExePath, nameof(clocExePath));
            ArgumentNullException.ThrowIfNull(argumentsProvider, nameof(argumentsProvider));

            string? workingDirectory = Path.GetDirectoryName(clocExePath);
            if (string.IsNullOrEmpty(workingDirectory))
                throw new ArgumentException("Invalid cloc executable path.", nameof(clocExePath));

            _workingDirectory = workingDirectory;
            _clocExePath = clocExePath;
            _argumentsProvider = argumentsProvider;
        }

        private readonly string _workingDirectory;
        private readonly string _clocExePath;
        private readonly IClocArgumentsProvider _argumentsProvider;

        public async Task<string[]> ExecuteAsync(string analyzeTargetPath, ClocOptions? options = null, IProgress<int>? progress = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(analyzeTargetPath, nameof(analyzeTargetPath));

            string arguments = _argumentsProvider.BuildArguments(analyzeTargetPath, options);
            ProcessStartInfo processStartInfo = new(_clocExePath, arguments)
            {
                WorkingDirectory = _workingDirectory,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };

            using Process process = new()
            {
                StartInfo = processStartInfo
            };

            if (!process.Start())
                throw new InvalidOperationException("Failed to start cloc process.");

            List<string> outputLines = [];
            try
            {
                while (true)
                {
                    string? line = await process.StandardOutput.ReadLineAsync();
                    if (line is null)
                        break;

                    outputLines.Add(line);
                    progress?.Report(outputLines.Count);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (!process.HasExited)
                        process.Kill();
                }
                catch (InvalidOperationException)
                {
                    // Process has already exited, ignore.
                }

                throw new InvalidOperationException("An error occurred while reading cloc output.", ex);
            }
            finally
            {
                process.WaitForExit();
            }

            if (process.ExitCode != 0)
                throw new InvalidOperationException($"cloc process exited with code {process.ExitCode}.");

            return outputLines.ToArray();
        }
    }
}
