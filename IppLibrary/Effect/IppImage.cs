using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IppLibrary.Effect
{
    class IppImage<T>
    {
        int _width;
        public int Width { get { return _width; } }

        int _height;

        public int Height { get { return _height; } }

        public int Size { get { return _width * _height; } }

        T[,] _pixels;

        public IppImage(int width, int height)
        {
            CreateImage(width, height);
        }

        public void CreateImage(int width, int height)
        {
            _width = width;
            _height = height;

            _pixels = new T[height, width];
        }

        public void DestroyImage()
        {
            _width = 0;
            _height = 0;
            _pixels = null;
        }

        byte limit(int value)
        {
            return (byte)((value > 255) ? 255 : ((value < 0) ? 0 : value));
        }

        //void Convert<U>(IppImage<U> ipp, bool useLimit)
        //{
        //    CreateImage(ipp.Width, ipp.Height);

        //    if (useLimit == true)
        //    {
        //        for (int i = 0; i < this.Height; i++)
        //        {
        //        }
        //    }
        //}
    }
}
