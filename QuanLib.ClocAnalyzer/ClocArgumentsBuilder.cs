using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.ClocAnalyzer
{
    public class ClocArgumentsBuilder
    {
        private string? _analyzeTarget;
        private string? _outputTarget;
        private string? _outputFormat;
        private bool _byFile;
        private bool _byLang;
        private bool _quiet;
        private List<string>? _excludeDir;
        private List<string>? _excludeLang;
        private List<string>? _excludeExt;
        private List<string>? _includeLang;
        private List<string>? _includeExt;
        private List<string>? _otherOptions;

        public ClocArgumentsBuilder WithAnalyzeTarget(string target)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(target, nameof(target));

            _analyzeTarget = target;
            return this;
        }

        public ClocArgumentsBuilder WithCsvFormat()
        {
            _outputFormat = "csv";
            return this;
        }

        public ClocArgumentsBuilder WithCsvFormat(char delimiter)
        {
            _outputFormat = "csv --csv-delimiter=" + delimiter;
            return this;
        }

        public ClocArgumentsBuilder WithJsonFormat()
        {
            _outputFormat = "json";
            return this;
        }

        public ClocArgumentsBuilder WithXmlFormat()
        {
            _outputFormat = "xml";
            return this;
        }

        public ClocArgumentsBuilder WithYamlFormat()
        {
            _outputFormat = "yaml";
            return this;
        }

        public ClocArgumentsBuilder WithMdFormat()
        {
            _outputFormat = "md";
            return this;
        }

        public ClocArgumentsBuilder ExcludeDir(string dir)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(dir, nameof(dir));

            _excludeDir ??= [];
            if (!_excludeDir.Contains(dir))
                _excludeDir.Add(dir);

            return this;
        }

        public ClocArgumentsBuilder ExcludeLang(string lang)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(lang, nameof(lang));

            _excludeLang ??= [];
            if (!_excludeLang.Contains(lang))
                _excludeLang.Add(lang);

            return this;
        }

        public ClocArgumentsBuilder ExcludeExt(string ext)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(ext, nameof(ext));

            _excludeExt ??= [];
            if (!_excludeExt.Contains(ext))
                _excludeExt.Add(ext);

            return this;
        }

        public ClocArgumentsBuilder IncludeLang(string lang)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(lang, nameof(lang));

            _includeLang ??= [];
            if (!_includeLang.Contains(lang))
                _includeLang.Add(lang);

            return this;
        }

        public ClocArgumentsBuilder IncludeExt(string ext)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(ext, nameof(ext));

            _includeExt ??= [];
            if (!_includeExt.Contains(ext))
                _includeExt.Add(ext);

            return this;
        }

        public ClocArgumentsBuilder WithOtherOption(string option)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(option, nameof(option));

            _otherOptions ??= [];
            if (!_otherOptions.Contains(option))
                _otherOptions.Add(option);

            return this;
        }

        public ClocArgumentsBuilder WithQuiet()
        {
            _quiet = true;
            return this;
        }

        public ClocArgumentsBuilder ByFile()
        {
            _byFile = true;
            return this;
        }

        public ClocArgumentsBuilder ByLang()
        {
            _byLang = true;
            return this;
        }

        public ClocArgumentsBuilder OutputToFile(string filePath)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath, nameof(filePath));

            _outputTarget = filePath;
            return this;
        }

        public string Build()
        {
            if (string.IsNullOrEmpty(_analyzeTarget))
                throw new InvalidOperationException("Analyze target must be specified.");

            var args = new StringBuilder();
            args.AppendFormat("\"{0}\"", _analyzeTarget);

            if (_excludeDir is not null && _excludeDir.Count > 0)
                args.Append(" --exclude-dir=").Append(string.Join(",", _excludeDir));
            if (_excludeLang is not null && _excludeLang.Count > 0)
                args.Append(" --exclude-lang=").Append(string.Join(",", _excludeLang));
            if (_excludeExt is not null && _excludeExt.Count > 0)
                args.Append(" --exclude-ext=").Append(string.Join(",", _excludeExt));
            if (_includeLang is not null && _includeLang.Count > 0)
                args.Append(" --include-lang=").Append(string.Join(",", _includeLang));
            if (_includeExt is not null && _includeExt.Count > 0)
                args.Append(" --include-ext=").Append(string.Join(",", _includeExt));

            if (!string.IsNullOrEmpty(_outputFormat))
                args.Append(" --").Append(_outputFormat);

            if (!string.IsNullOrEmpty(_outputTarget))
                args.Append(" --out=").AppendFormat("\"{0}\"", _outputTarget);

            if (_byFile)
            {
                if (_byLang)
                    args.Append(" --by-file-by-lang");
                else
                    args.Append(" --by-file");
            }

            if (_quiet)
                args.Append(" --quiet");

            if (_otherOptions is not null && _otherOptions.Count > 0)
            {
                foreach (var option in _otherOptions)
                {
                    args.Append(' ');
                    if (!option.StartsWith("--"))
                        args.Append("--");
                    args.Append(option);
                }
            }

            return args.ToString();
        }
    }
}
