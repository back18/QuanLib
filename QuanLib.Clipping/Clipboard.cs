using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Clipping
{
    public class Clipboard : IClipboard
    {
        public Clipboard()
        {
            _dataContainers = [];
        }

        private readonly List<DataContainer> _dataContainers;

        public ClipboardMode GetClipboardMode()
        {
            return GetDataContainer()?.ClipboardMode ?? ClipboardMode.None;
        }

        public IDataObject? GetDataObject()
        {
            return GetDataContainer()?.GetDataObject();
        }

        public object? GetData()
        {
            return GetDataContainer()?.GetData();
        }

        public object? GetData(string format)
        {
            return GetDataContainer()?.GetData(format);
        }

        public object? GetData(string format, bool autoConvert)
        {
            return GetDataContainer()?.GetData(format, autoConvert);
        }

        public string? GetText()
        {
            return GetDataContainer()?.GetText();
        }

        public Image? GetImage()
        {
            return GetDataContainer()?.GetImage();
        }

        public Image<Rgba32>? GetBitmap()
        {
            return GetDataContainer()?.GetBitmap();
        }

        public string[]? GetFileDrop()
        {
            return GetDataContainer()?.GetFileDrop();
        }

        public void SetDataObject(IDataObject dataObject, ClipboardMode clipboardMode = ClipboardMode.None)
        {
            _dataContainers.Add(new(dataObject, clipboardMode));
        }

        public void SetData(string format, object data, ClipboardMode clipboardMode = ClipboardMode.None)
        {
            _dataContainers.Add(DataContainer.CreateData(format, data, clipboardMode));
        }

        public void SetText(string text, ClipboardMode clipboardMode = ClipboardMode.None)
        {
            _dataContainers.Add(DataContainer.CreateText(text, clipboardMode));
        }

        public void SetImage(Image image, ClipboardMode clipboardMode = ClipboardMode.None)
        {
            _dataContainers.Add(DataContainer.CreateImage(image, clipboardMode));
        }

        public void SetBitmap(Image<Rgba32> image, ClipboardMode clipboardMode = ClipboardMode.None)
        {
            _dataContainers.Add(DataContainer.CreateBitmap(image, clipboardMode));
        }

        public void SetFileDrop(string[] paths, ClipboardMode clipboardMode = ClipboardMode.None)
        {
            _dataContainers.Add(DataContainer.CreateFileDrop(paths, clipboardMode));
        }

        public void Clear()
        {
            DataContainer[] dataContainers = _dataContainers.ToArray();
            _dataContainers.Clear();

            foreach (DataContainer dataContainer in dataContainers)
            {
                if (dataContainer.GetDataObject() is IDisposable disposable)
                    disposable.Dispose();
            }
        }

        private DataContainer? GetDataContainer()
        {
            if (_dataContainers.Count > 0)
                return _dataContainers[^1];
            else
                return null;
        }
    }
}
