using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Clipping.DataObjects
{
    [DataObject(FORMAT)]
    public class FileDropDataObject : IDataObject<string[]>, IConvertible<TextDataObject>
    {
        public const string FORMAT = "FileDrop";

        public FileDropDataObject(params string[] paths)
        {
            ArgumentNullException.ThrowIfNull(paths, nameof(paths));

            _paths = paths;
        }

        private readonly string[] _paths;

        public string Format => FORMAT;

        public string[] GetData()
        {
            return _paths;
        }

        object IDataObject.GetDate()
        {
            return GetData();
        }

        TextDataObject IConvertible<TextDataObject>.Convert()
        {
            return new(ToString());
        }

        public override string ToString()
        {
            if (_paths.Length == 0)
            {
                return string.Empty;
            }
            else if (_paths.Where(w => w.Contains(' ')).Any())
            {
                return string.Join(',', _paths.Select(s => $"\"{s}\""));
            }
            else
            {
                return string.Join(',', _paths);
            }
        }

        public static IDataObject Create(object data)
        {
            ArgumentNullException.ThrowIfNull(data, nameof(data));

            if (data is string[] paths)
                return new FileDropDataObject(paths);
            else
                return new DataObject(FORMAT, data);
        }
    }
}
