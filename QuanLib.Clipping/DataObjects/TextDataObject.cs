using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Clipping.DataObjects
{
    [DataObject(FORMAT)]
    public class TextDataObject : IDataObject<string>
    {
        public const string FORMAT = "Text";

        public TextDataObject(string text)
        {
            ArgumentNullException.ThrowIfNull(text, nameof(text));

            _text = text;
        }

        private readonly string _text;

        public string Format => FORMAT;

        public string GetData()
        {
            return _text;
        }

        object IDataObject.GetDate()
        {
            return GetData();
        }

        public static IDataObject Create(object data)
        {
            ArgumentNullException.ThrowIfNull(data, nameof(data));

            if (data is string text)
                return new TextDataObject(text);
            else
                return new DataObject(FORMAT, data);
        }
    }
}
