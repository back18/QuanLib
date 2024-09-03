using QuanLib.Core;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Clipping.DataObjects
{
    [DataObject(FORMAT)]
    public class ImageDataObject : UnmanagedBase, IDataObject<Image>
    {
        public const string FORMAT = "Image";

        public ImageDataObject(Image image)
        {
            ArgumentNullException.ThrowIfNull(image, nameof(image));

            _image = image;
        }

        private readonly Image _image;

        public string Format => FORMAT;

        public Image GetData()
        {
            return _image;
        }

        object IDataObject.GetDate()
        {
            return GetData();
        }

        protected override void DisposeUnmanaged()
        {
            _image.Dispose();
        }

        public static IDataObject Create(object data)
        {
            ArgumentNullException.ThrowIfNull(data, nameof(data));

            if (data is Image image)
                return new ImageDataObject(image);
            else
                return new DataObject(FORMAT, data);
        }
    }
}
