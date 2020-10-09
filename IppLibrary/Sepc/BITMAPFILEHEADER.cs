using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IppLibrary
{
    // http://pinvoke.net/default.aspx/Structures/BITMAPFILEHEADER.html
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct BITMAPFILEHEADER
    {
        public ushort bfType;       // always 'B', 'M' == 0x42 0x4d
        public uint bfSize;         // size of BITMAP File in bytes, simply file size.
        public ushort bfReserved1;  // not used
        public ushort bfReserved2;  // not used
        public uint bfOffBits;      // offset to pixel data
                                    //  == sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER) + sizeof(color table)

        public override string ToString()
        {
            return string.Format("FileSize: {0}", bfSize);
        }
    }
}
