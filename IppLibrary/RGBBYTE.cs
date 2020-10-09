using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IppLibrary
{
    public class RGBBYTE
    {
        public byte _b;
        public byte _g;
        public byte _r;

        public RGBBYTE()
        {
            _b = _g = _r = 0;
        }

        public RGBBYTE(byte gray)
        {
            _b = _g = _r = gray;
        }

        public RGBBYTE(byte b, byte g, byte r)
        {
            _b = b;
            _g = g;
            _r = r;
        }
    }
}
