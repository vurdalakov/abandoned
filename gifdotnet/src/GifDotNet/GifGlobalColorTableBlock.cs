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
    /// This class represents the Global Color Table block.
    /// <para>This block contains a color table, which is a sequence of bytes representing red-green-blue color triplets.
    /// The Global Color Table is used by images without a Local Color Table and by Plain Text Extensions.
    /// Its presence is marked by the Global Color Table Flag being set to 1 in the Logical Screen Descriptor;
    /// if present, it immediately follows the Logical Screen Descriptor and contains a number of bytes equal to</para>
    /// <para>            3 x 2^(Size of Global Color Table+1).</para>
    /// </summary>
    public class GifGlobalColorTableBlock : GifBlock
    {
        internal GifGlobalColorTableBlock(GifReader reader, GifLogicalScreenDescriptorBlock gifLogicalScreenDescriptorBlock)
            : base(GifBlockType.GlobalColorTable)
        {
            Read(reader, gifLogicalScreenDescriptorBlock);
        }

        private Byte[] bytes;
        /// <summary>
        /// Contains a color table, which is a sequence of bytes representing red-green-blue color triplets.
        /// </summary>
        public Byte[] Bytes
        {
            get { return bytes; }
        }

        internal void Read(GifReader reader, GifLogicalScreenDescriptorBlock gifLogicalScreenDescriptorBlock)
        {
            bytes = reader.ReadBytes(gifLogicalScreenDescriptorBlock.GlobalColorTableSize * 3);

            //byte[] globalColorTableBuffer = reader.ReadBytes(globalColorTableSize * 3);

            //globalColorTable = new Color[globalColorTableSize];

            //int baseIndex = 0;
            //for (UInt16 colorIndex = 0; colorIndex < globalColorTableSize; colorIndex++)
            //{
            //    globalColorTable[colorIndex] = Color.FromArgb(globalColorTableBuffer[baseIndex], globalColorTableBuffer[baseIndex + 1], globalColorTableBuffer[baseIndex + 2]);
            //    baseIndex += 3;
            //}
        }

        internal override void Write(GifWriter gifWriter)
        {
            gifWriter.Write(bytes);
        }
    }
}
