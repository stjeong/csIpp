using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace IppLibrary
{
    public class IppDib
    {
        static string BMP_INVALIDFILEHEADER = "Invalid BMP Image File Header Format";

        BITMAPFILEHEADER _bmfh;
        BITMAPINFOHEADER _bmih;
        long _pvOffset;
        uint _widthStep;
        byte[] _dibBuffer; // start offset of BITMAPINFOHEADER - includes RGBQUAD arrays and pixel data

        int GetPaletteNums()
        {
            switch (_bmih.biBitCount)
            {
                case 1: return 2;
                case 4: return 16;
                case 8: return 256;
                default: return 0;
            }
        }

        long GetDIBitsAddrOffset()
        {
            return _pvOffset;
        }

        public static IppDib LoadFrom(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                IppDib bitmap = new IppDib();

                bitmap._bmfh = IppDib.FileHeaderFromStream(fs);
                bitmap._bmih = IppDib.InfoHeaderFromStream(fs);

                int width = bitmap._bmih.biWidth;
                int height = bitmap._bmih.biHeight;
                ushort bitCount = bitmap._bmih.biBitCount;

                bitmap._widthStep = (uint)((width * bitCount / 8 + 3) & ~3);
                uint sizeImage = (uint)(Math.Abs(height) * bitmap._widthStep);

                int infoHeaderSize = Marshal.SizeOf(bitmap._bmih);
                uint packedDibSize = (uint)infoHeaderSize;
                uint rgbQuadAndDataSize = (uint)((bitCount == 24) ? sizeImage : Marshal.SizeOf(RGBQUAD.Dummy) * (1 << bitCount) + sizeImage);

                packedDibSize += rgbQuadAndDataSize;

                bitmap._dibBuffer = new byte[packedDibSize];

                fs.Seek(Marshal.SizeOf(bitmap._bmfh), SeekOrigin.Begin);

                fs.Read(bitmap._dibBuffer, 0, bitmap._dibBuffer.Length);

                bitmap._pvOffset = (uint)infoHeaderSize;
                if (bitCount != 24)
                {
                    bitmap._pvOffset += (Marshal.SizeOf(RGBQUAD.Dummy) * (1 << bitCount));
                }

                return bitmap;
            }
        }

        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/54a096ff-46f3-45ce-8560-bf5a0618ef75/how-to-set-pixel-into-bitmap-with-pixel-format-of-format8bppindexed-?forum=csharpgeneral
        public Bitmap ToBitmap()
        {
            var bmp = new Bitmap(_bmih.biWidth, _bmih.biHeight, PixelFormat.Format8bppIndexed);
            {
                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0,
                                                                (int)_widthStep,
                                                                bmp.Height),
                                                  ImageLockMode.WriteOnly,
                                                  bmp.PixelFormat);

                int offset = (int)_pvOffset;

                if (_bmih.biHeight < 0)
                {
                    Marshal.Copy(_dibBuffer, (int)_pvOffset, bmpData.Scan0, (int)(_dibBuffer.Length - _pvOffset));
                }
                else
                {
                    for (int i = bmp.Height - 1; i >= 0; i--)
                    {
                        IntPtr ptr = new IntPtr(bmpData.Scan0.ToInt32() + (_widthStep * i));
                        Marshal.Copy(_dibBuffer, offset, ptr, bmp.Width);
                        offset += (int)bmp.Width;
                    }
                }

                bmp.UnlockBits(bmpData);

                if (bmp.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    ColorPalette grayScalePalette = bmp.Palette;

                    for (int i = 0; i < 256; i++)
                    {
                        grayScalePalette.Entries[i] = Color.FromArgb(i, i, i);
                    }

                    bmp.Palette = grayScalePalette;
                }
            }

            return bmp;
        }

        static BITMAPFILEHEADER FileHeaderFromStream(Stream stream)
        {
            BITMAPFILEHEADER header = new BITMAPFILEHEADER();

            header.bfType = stream.ReadUInt16();

            ushort marker = 'M' << 8 | 'B';
            if (header.bfType != marker)
            {
                throw new ApplicationException(BMP_INVALIDFILEHEADER);
            }

            header.bfSize = stream.ReadUInt32();
            stream.Seek(sizeof(ushort) * 2, SeekOrigin.Current);
            header.bfOffBits = stream.ReadUInt32();

            return header;
        }

        static BITMAPINFOHEADER InfoHeaderFromStream(Stream stream)
        {
            BITMAPINFOHEADER info = new BITMAPINFOHEADER();

            info.biSize = stream.ReadUInt32();

            info.biWidth = stream.ReadInt32();
            info.biHeight = stream.ReadInt32();

            info.biPlanes = stream.ReadUInt16();
            info.biBitCount = stream.ReadUInt16();

            info.biCompression = (BitmapCompressionMode)stream.ReadUInt32();
            info.biSizeImage = stream.ReadUInt32();

            info.biXPelsPerMeter = stream.ReadInt32();
            info.biYPelsPerMeter = stream.ReadInt32();

            info.biClrUsed = stream.ReadUInt32();
            info.biClrImportant = stream.ReadUInt32();

            return info;
        }
    }
}
