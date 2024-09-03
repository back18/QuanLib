using QuanLib.Clipping.DataObjects;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Clipping
{
    public class DataContainer : IDataContainer
    {
        public DataContainer(IDataObject dataObject, ClipboardMode clipboardMode = ClipboardMode.None)
        {
            ArgumentNullException.ThrowIfNull(dataObject, nameof(dataObject));

            _dataObject = dataObject;
            ClipboardMode = clipboardMode;
        }

        private readonly IDataObject _dataObject;

        public ClipboardMode ClipboardMode { get; }

        public IDataObject GetDataObject()
        {
            return _dataObject;
        }

        public object GetData()
        {
            return _dataObject.GetDate();
        }

        public object? GetData(string format)
        {
            return GetData(format, false);
        }

        public object? GetData(string format, bool autoConvert)
        {
            ArgumentException.ThrowIfNullOrEmpty(format, nameof(format));

            if (_dataObject.Format == format)
                return _dataObject.GetDate();

            if (!autoConvert)
                return null;

            DataObjectConverter.TryConvert(_dataObject, format, out var dataObject);
            return dataObject;
        }

        public object? GetData(Type dataType)
        {
            return GetData(dataType, false);
        }

        public object? GetData(Type dataType, bool autoConvert)
        {
            ArgumentNullException.ThrowIfNull(dataType, nameof(dataType));

            if (_dataObject.GetType().IsAssignableTo(dataType))
                return _dataObject.GetDate();

            if (!autoConvert)
                return null;

            DataObjectConverter.TryConvert(_dataObject, dataType, out var dataObject);
            return dataObject;
        }

        public string? GetText()
        {
            return GetData(typeof(TextDataObject)) as string;
        }

        public Image? GetImage()
        {
            return GetData(typeof(ImageDataObject)) as Image;
        }

        public Image<Rgba32>? GetBitmap()
        {
            return GetData(typeof(BitmapDataObject)) as Image<Rgba32>;
        }

        public string[]? GetFileDrop()
        {
            return GetData(typeof(FileDropDataObject)) as string[];
        }

        public static DataContainer CreateData(string format, object data, ClipboardMode clipboardMode = ClipboardMode.None)
        {
            IDataObject dataObject = DataObjectFactory.Create(format, data);
            return new(dataObject, clipboardMode);
        }

        public static DataContainer CreateText(string text, ClipboardMode clipboardMode = ClipboardMode.None)
        {
            TextDataObject dataObject = new(text);
            return new(dataObject, clipboardMode);
        }

        public static DataContainer CreateImage(Image image, ClipboardMode clipboardMode = ClipboardMode.None)
        {
            ImageDataObject dataObject = new(image);
            return new(dataObject, clipboardMode);
        }

        public static DataContainer CreateBitmap(Image<Rgba32> image, ClipboardMode clipboardMode = ClipboardMode.None)
        {
            BitmapDataObject dataObject = new(image);
            return new(dataObject, clipboardMode);
        }

        public static DataContainer CreateFileDrop(string[] patgs, ClipboardMode clipboardMode = ClipboardMode.None)
        {
            FileDropDataObject dataObject = new(patgs);
            return new(dataObject, clipboardMode);
        }
    }
}
