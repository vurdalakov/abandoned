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
    /// This class represents the Table Based Image Data block.
    /// The image data for a table based image consists of bytes containing an index into the active color table, for each pixel in the image.
    /// </summary>
    public class GifTableBasedImageDataBlock : GifBlock
    {
        internal GifTableBasedImageDataBlock(GifReader reader)
            : base(GifBlockType.TableBasedImageData)
        {
            Read(reader);
        }

        private Byte lzwMinimumCodeSize;
        /// <summary>
        /// LZW Minimum Code Size - This byte determines the initial number of bits used for LZW codes in the image data.
        /// </summary>
        public Byte LzwMinimumCodeSize
        {
            get { return lzwMinimumCodeSize; }
        }

        private Byte[] imageData;
        /// <summary>
        /// The image data for a table based image consists of bytes containing an index into the active color table, for each pixel in the image.
        /// <para>Pixel indices are in order of left to right and from top to bottom.
        /// Each index must be within the range of the size of the active color table, starting at 0.
        /// The sequence of indices is encoded using the LZW Algorithm with variable-length code.</para>
        /// </summary>
        public Byte[] ImageData
        {
            get { return imageData; }
        }

        internal void Read(GifReader reader)
        {
            lzwMinimumCodeSize = reader.ReadByte();

            imageData = reader.ReadBinarySubBlocks();
        }

        internal override void Write(GifWriter gifWriter)
        {
            gifWriter.WriteByte(lzwMinimumCodeSize);
            gifWriter.WriteBinarySubBlocks(imageData);
        }
    }
}
