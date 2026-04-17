using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.ClocAnalyzer.Services.Implementations
{
    public class ClocCsvParseService : IClocFileParseService
    {
        private static readonly string[] _header = ["language", "filename", "blank", "comment", "code"];

        public FileStats[] Parse(IEnumerable<string> clocOutput, char delimiter = ',')
        {
            ArgumentNullException.ThrowIfNull(clocOutput, nameof(clocOutput));

            string headerLine = string.Join(delimiter, _header);
            bool inDataSection = false;
            List<FileStats> result = [];

            foreach (string line in clocOutput)
            {
                if (!inDataSection)
                {
                    if (line.StartsWith(headerLine))
                        inDataSection = true;
                    continue;
                }

                string[] parts = line.Split(delimiter);
                if (parts.Length != _header.Length || parts[0] == "SUM")
                    break;

                FileStats fileStats = new(parts[0], parts[1],
                    int.TryParse(parts[2], out int blank) ? blank : 0,
                    int.TryParse(parts[3], out int comment) ? comment : 0,
                    int.TryParse(parts[4], out int code) ? code : 0
                );

                result.Add(fileStats);
            }

            return result.ToArray();
        }
    }
}
