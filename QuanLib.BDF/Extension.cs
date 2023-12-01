using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.BDF
{
    public static class Extension
    {
        public static int GetLeftLayoutMaxCount(this BdfFont source, int maxWidth, string value)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            int width = 0;
            for (int i = 0; i < value.Length; i++)
            {
                width += source[value[i]].Width;
                if (width == maxWidth)
                    return i + 1;
                else if (width > maxWidth)
                    return i;
            }
            return width;
        }

        public static int GetRightLayoutMaxCount(this BdfFont source, int maxWidth, string value)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            int width = 0;
            for (int i = value.Length - 1; i >= 0; i--)
            {
                width += source[value[i]].Width;
                if (width == maxWidth)
                    return value.Length - i + 1;
                else if (width > maxWidth)
                    return value.Length - i;
            }
            return width;
        }

        public static int GetTopLayoutMaxCount(this BdfFont source, int maxHeight, string value)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            int height = 0;
            for (int i = 0; i < value.Length; i++)
            {
                height += source[value[i]].Height;
                if (height == maxHeight)
                    return i + 1;
                else if (height > maxHeight)
                    return i;
            }
            return height;
        }

        public static int GetBottomLayoutMaxCount(this BdfFont source, int maxHeight, string value)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            int height = 0;
            for (int i = value.Length - 1; i >= 0; i--)
            {
                height += source[value[i]].Height;
                if (height == maxHeight)
                    return value.Length - i + 1;
                else if (height > maxHeight)
                    return value.Length - i;
            }
            return height;
        }
    }
}
