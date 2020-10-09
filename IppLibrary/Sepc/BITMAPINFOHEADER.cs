using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace IppLibrary
{
    // http://www.pinvoke.net/default.aspx/Structures/BITMAPINFOHEADER.html
    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPINFOHEADER
    {
        public uint biSize;         // sizeof(BITMAPINFOHEADER) == 40
                                    // when biSize != 40, extended DIB
        public int biWidth;         // width of bitmap 
        public int biHeight;        // height of bitmap
                                    // when height > 0, pixel data in bottom-up
                                    // when height < 0, pixel data in top-down
        public ushort biPlanes;     // always 1
        public ushort biBitCount;   // bit count per one pixel
                                    // 1, 4, 8, 16, 24, 32
        public BitmapCompressionMode biCompression;
        public uint biSizeImage;    // size for memory to load pixel data, 4bytes-aligned in width
                                    // when biSizeImage == BI_RGB, 0
        public int biXPelsPerMeter; 
        public int biYPelsPerMeter; 
        public uint biClrUsed;      // # of color used at color table
                                    // when 0, biBitCount
                                    // when != 0, sizeof(RGBQUAD[])
        public uint biClrImportant; // # of color index
                                    // in the mose cases, will be 0

        public override string ToString()
        {
            return string.Format("Size: {0}x{1}, Color: {2}, # of RGBQuad; {3}", biWidth, biHeight, biBitCount, biClrUsed);
        }
    }
}
