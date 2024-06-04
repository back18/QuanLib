using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public static class ObjectFormatter
    {
        public static string Format(object? obj)
        {
            if (obj is null)
                return "<null>";

            if (obj is string text)
                return Format(text);

            if (obj is Type type)
                return Format(type);

            if (obj is Exception exception)
                return Format(exception);

            if (obj is IEnumerable enumerable)
                return Format(enumerable);

            return obj?.ToString() ?? Format((object?)null);
        }

        public static string Format(string? text)
        {
            if (text is null)
                return Format((object?)null);

            StringBuilder stringBuilder = new(text);
            stringBuilder.Replace("\\", "\\\\");
            stringBuilder.Replace("\r\n", "\\r\\n");
            stringBuilder.Replace("\r", "\\r");
            stringBuilder.Replace("\n", "\\n");
            stringBuilder.Replace("\t", "\\t");
            stringBuilder.Replace("\v", "\\v");
            stringBuilder.Replace("\f", "\\f");
            stringBuilder.Replace("\'", "\\'");
            stringBuilder.Replace("\"", "\\\"");
            stringBuilder.Insert(0, '\"');
            stringBuilder.Append('\"');

            return stringBuilder.ToString();
        }

        public static string Format(Type? type)
        {
            if (type is null)
                return Format((object?)null);

            StringBuilder stringBuilder = new();

            stringBuilder.Append(type.Namespace);
            stringBuilder.Append('.');
            stringBuilder.Append(type.Name.TrimEnd('&').Replace("`" + type.GenericTypeArguments.Length, string.Empty));

            if (type.GenericTypeArguments.Length > 0)
            {
                stringBuilder.Append('<');
                stringBuilder.AppendJoin(',', type.GenericTypeArguments.Select(Format));
                stringBuilder.Append('>');
            }

            return stringBuilder.ToString();
        }

        public static string Format(Exception? exception)
        {
            if (exception is null)
                return Format((object?)null);

            string s = $"{Format(exception.GetType())}: {Format(exception.Message)}";
            if (exception.InnerException is not null)
                s += $" (InnerException: {Format(exception.InnerException)})";
            return s;
        }

        public static string Format<T>(IEnumerable<T>? enumerable)
        {
            if (enumerable is null)
                return Format((object?)null);

            return $"[{string.Join(", ", enumerable.Select(s => Format(s)))}]";
        }

        public static string Format(IEnumerable? enumerable)
        {
            if (enumerable is null)
                return Format((object?)null);

            List<string> list = [];
            foreach (var item in enumerable)
                list.Add(Format(item));
            return $"[{string.Join(", ", list)}]";
        }
    }
}
