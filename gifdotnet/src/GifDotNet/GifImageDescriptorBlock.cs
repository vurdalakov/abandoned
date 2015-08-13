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

namespace Vurdalakov.GifDotNet
{
    /// <summary>
    /// This class represents the Image Descriptor block.
    /// <para>Each image in the Data Stream is composed of an Image Descriptor, an optional Local Color Table, and the image data.
    /// Each image must fit within the boundaries of the Logical Screen, as defined in the Logical Screen Descriptor.</para>
    /// <para>The Image Descriptor contains the parameters necessary to process a table based image.
    /// The coordinates given in this block refer to coordinates within the Logical Screen, and are given in pixels.
    /// This block is a Graphic-Rendering Block, optionally preceded by one or more Control blocks such as the Graphic Control Extension,
    /// and may be optionally followed by a Local Color Table; the Image Descriptor is always followed by the image data.</para>
    /// </summary>
    public class GifImageDescriptorBlock : GifBlock
    {
        internal GifImageDescriptorBlock(GifReader reader)
            : base(GifBlockType.ImageDescriptor)
        {
            Read(reader);
        }

        private UInt16 left;
        /// <summary>
        /// Column number, in pixels, of the left edge of the image, with respect to the left edge of the Logical Screen. Leftmost column of the Logical Screen is 0.
        /// </summary>
        public UInt16 Left
        {
            get { return left; }
        }

        private UInt16 top;
        /// <summary>
        /// Row number, in pixels, of the top edge of the image with respect to the top edge of the Logical Screen. Top row of the Logical Screen is 0.
        /// </summary>
        public UInt16 Top
        {
            get { return top; }
        }

        private UInt16 width;
        /// <summary>
        /// Width of the image in pixels.
        /// </summary>
        public UInt16 Width
        {
            get { return width; }
        }

        private UInt16 height;
        /// <summary>
        /// Height of the image in pixels.
        /// </summary>
        public UInt16 Height
        {
            get { return height; }
        }

        private Boolean interlaced;
        /// <summary>
        /// Indicates if the image is interlaced.
        /// </summary>
        public Boolean Interlaced
        {
            get { return interlaced; }
        }

        private Boolean localColorTableFlag;
        /// <summary>
        /// Indicates the presence of a Local Color Table.
        /// </summary>
        public Boolean LocalColorTableFlag
        {
            get { return localColorTableFlag; }
        }

        private Boolean localColorTableSorted;
        /// <summary>
        /// Indicates whether the Local Color Table is sorted.  If the flag is set, the Local Color Table is sorted, in order of decreasing importance.
        /// </summary>
        public Boolean LocalColorTableSorted
        {
            get { return localColorTableSorted; }
        }

        private UInt16 localColorTableSize;
        /// <summary>
        /// Number of RGB entries in Local Color Table.
        /// </summary>
        public UInt16 LocalColorTableSize
        {
            get { return localColorTableSize; }
        }

        internal void Read(GifReader reader)
        {
            left = reader.ReadUInt16();
            top = reader.ReadUInt16();

            width = reader.ReadUInt16();
            height = reader.ReadUInt16();

            Byte flags = reader.ReadByte();
            localColorTableFlag = 0x80 == (flags & 0x80);
            interlaced = 0x40 == (flags & 0x40);
            localColorTableSorted = 0x20 == (flags & 0x20);
            localColorTableSize = (UInt16)(2 << (flags & 0x07));
        }

        internal override void Write(GifWriter gifWriter)
        {
            gifWriter.WriteByte(0x2C);

            gifWriter.WriteUInt16(left);
            gifWriter.WriteUInt16(top);

            gifWriter.WriteUInt16(width);
            gifWriter.WriteUInt16(height);

            Byte flags = (byte)(Log2(localColorTableSize) | (localColorTableSorted ? 0x20 : 0) | (interlaced ? 0x40 : 0) | (localColorTableFlag ? 0x80 : 0));
            gifWriter.WriteByte(flags);
        }
    }
}
