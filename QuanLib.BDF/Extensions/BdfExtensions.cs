using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.BDF.Extensions
{
    public static class BdfExtensions
    {
        public static int GetLeftLayoutMaxCount(this BdfFont bdfFont, int maxWidth, string value)
        {
            ArgumentNullException.ThrowIfNull(bdfFont, nameof(bdfFont));
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            int width = 0;
            for (int i = 0; i < value.Length; i++)
            {
                width += bdfFont[value[i]].Width;
                if (width == maxWidth)
                    return i + 1;
                else if (width > maxWidth)
                    return i;
            }
            return width;
        }

        public static int GetRightLayoutMaxCount(this BdfFont bdfFont, int maxWidth, string value)
        {
            ArgumentNullException.ThrowIfNull(bdfFont, nameof(bdfFont));
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            int width = 0;
            for (int i = value.Length - 1; i >= 0; i--)
            {
                width += bdfFont[value[i]].Width;
                if (width == maxWidth)
                    return value.Length - i + 1;
                else if (width > maxWidth)
                    return value.Length - i;
            }
            return width;
        }

        public static int GetTopLayoutMaxCount(this BdfFont bdfFont, int maxHeight, string value)
        {
            ArgumentNullException.ThrowIfNull(bdfFont, nameof(bdfFont));
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            int height = 0;
            for (int i = 0; i < value.Length; i++)
            {
                height += bdfFont[value[i]].Height;
                if (height == maxHeight)
                    return i + 1;
                else if (height > maxHeight)
                    return i;
            }
            return height;
        }

        public static int GetBottomLayoutMaxCount(this BdfFont bdfFont, int maxHeight, string value)
        {
            ArgumentNullException.ThrowIfNull(bdfFont, nameof(bdfFont));
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            int height = 0;
            for (int i = value.Length - 1; i >= 0; i--)
            {
                height += bdfFont[value[i]].Height;
                if (height == maxHeight)
                    return value.Length - i + 1;
                else if (height > maxHeight)
                    return value.Length - i;
            }
            return height;
        }
    }
}
