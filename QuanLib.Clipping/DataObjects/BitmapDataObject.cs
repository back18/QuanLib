using QuanLib.Core;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Clipping.DataObjects
{
    [DataObject(FORMAT)]
    public class BitmapDataObject : UnmanagedBase, IDataObject<Image<Rgba32>>, IConvertible<ImageDataObject>
    {
        public const string FORMAT = "Bitmap";

        public string Format => FORMAT;

        public BitmapDataObject(Image<Rgba32> image)
        {
            ArgumentNullException.ThrowIfNull(image, nameof(image));

            _image = image;
        }

        private readonly Image<Rgba32> _image;

        public Image<Rgba32> GetData()
        {
            return _image;
        }

        object IDataObject.GetDate()
        {
            return GetData();
        }

        ImageDataObject IConvertible<ImageDataObject>.Convert()
        {
            return new(_image);
        }

        protected override void DisposeUnmanaged()
        {
            _image.Dispose();
        }

        public static IDataObject Create(object data)
        {
            ArgumentNullException.ThrowIfNull(data, nameof(data));

            if (data is Image<Rgba32> image)
                return new BitmapDataObject(image);
            else
                return new DataObject(FORMAT, data);
        }
    }
}
