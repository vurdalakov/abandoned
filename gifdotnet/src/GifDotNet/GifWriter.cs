#region License
// GifDotNet is a .NET class library for processing GIF images
// http://code.google.com/p/gifdotnet/ 
//
// Copyright (c) 2011 Vurdalakov
// email: vurdalakov@gmail.com

/*
    This file is part of GifDotNet class library.

    GifDotNet is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    GifDotNet is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with GifDotNet.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.IO;
using System.Text;

namespace Vurdalakov.GifDotNet
{
    internal class GifWriter : BinaryWriter
    {
        public GifWriter(Stream stream)
            : base(stream, new ASCIIEncoding())
        {
        }

        private ASCIIEncoding asciiEncoding = new ASCIIEncoding();

        public void WriteString(String value)
        {
            Byte[] buffer = asciiEncoding.GetBytes(value);

            Write(buffer);
        }

        public void WriteTextSubBlocks(String text)
        {
            Byte[] buffer = asciiEncoding.GetBytes(text);

            WriteBinarySubBlocks(buffer);
        }

        public void WriteBinarySubBlocks(Byte[] buffer)
        {
            int length = buffer.Length;
            int offset = 0;

            while (offset < length)
            {
                int size = length - offset;
                if (size > 255)
                {
                    size = 255;
                }

                WriteByte((byte)size);
                Write(buffer, offset, size);

                offset += size;
            }

            WriteByte(0);
        }

        public void WriteByte(Byte value)
        {
            Write(value);
        }

        public void WriteUInt16(UInt16 value)
        {
            WriteByte((byte)(value & 0xFF));
            WriteByte((byte)((value >> 8) & 0xFF));
        }

        public void WriteUInt32(UInt32 value)
        {
            WriteByte((byte)(value & 0xFF));
            WriteByte((byte)((value >> 8) & 0xFF));
            WriteByte((byte)((value >> 16) & 0xFF));
            WriteByte((byte)((value >> 24) & 0xFF));
        }
    }
}
