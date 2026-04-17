using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.ClocAnalyzer.Services.Implementations
{
    public class ClocArgumentsProvider : IClocArgumentsProvider
    {
        public string BuildArguments(string analyzeTargetPath, ClocOptions? options = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(analyzeTargetPath, nameof(analyzeTargetPath));

            if (options is null)
                return analyzeTargetPath;

            ClocArgumentsBuilder builder = new();
            builder.WithAnalyzeTarget(analyzeTargetPath);

            options.ExcludeDir?.ForEach(dir => builder.ExcludeDir(dir));
            options.ExcludeLang?.ForEach(lang => builder.ExcludeLang(lang));
            options.ExcludeExt?.ForEach(ext => builder.ExcludeExt(ext));
            options.IncludeLang?.ForEach(lang => builder.IncludeLang(lang));
            options.IncludeExt?.ForEach(ext => builder.IncludeExt(ext));

            switch (options.OutputFileFormat)
            {
                case OutputFileFormat.Csv:
                    if (options.CsvDelimiter == ClocOptions.DefaultCsvDelimiter)
                        builder.WithCsvFormat();
                    else
                        builder.WithCsvFormat(options.CsvDelimiter);
                    break;
                case OutputFileFormat.Json:
                    builder.WithJsonFormat();
                    break;
                case OutputFileFormat.Xml:
                    builder.WithXmlFormat();
                    break;
                case OutputFileFormat.Yaml:
                    builder.WithYamlFormat();
                    break;
                case OutputFileFormat.Md:
                    builder.WithMdFormat();
                    break;
            }

            if (!string.IsNullOrWhiteSpace(options.OutputTarget))
                builder.OutputToFile(options.OutputTarget);

            switch (options.OutputContentFormat)
            {
                case OutputContentFormat.ByFile:
                    builder.ByFile();
                    break;
                case OutputContentFormat.ByFileAndLang:
                    builder.ByFile().ByLang();
                    break;
            }

            if (options.IsQuiet)
                builder.WithQuiet();

            options.OtherOptions?.ForEach(opt => builder.WithOtherOption(opt));

            return builder.Build();
        }
    }
}
