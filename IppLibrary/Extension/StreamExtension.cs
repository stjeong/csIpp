using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IppLibrary
{
    public static class StreamExtension
    {
        public static ushort ReadUInt16(this Stream stream)
        {
            int low = stream.ReadByte();
            int high = stream.ReadByte();

            return (ushort)(low | (high << 8));
        }

        public static uint ReadUInt32(this Stream stream)
        {
            byte[] buffer = new byte[4];
            stream.Read(buffer, 0, 4);

            return BitConverter.ToUInt32(buffer, 0);
        }

        public static int ReadInt32(this Stream stream)
        {
            byte[] buffer = new byte[4];
            stream.Read(buffer, 0, 4);

            return BitConverter.ToInt32(buffer, 0);
        }
    }
}
