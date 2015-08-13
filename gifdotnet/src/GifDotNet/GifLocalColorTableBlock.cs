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
    /// This class represents the Local Color Table block.
    /// <para>This block contains a color table, which is a sequence of bytes representing red-green-blue color triplets.
    /// The Local Color Table is used by the image that immediately follows.
    /// Its presence is marked by the Local Color Table Flag being set to 1 in the Image Descriptor;
    /// if present, the Local Color Table immediately follows the Image Descriptor and contains a number of bytes equal to</para>
    /// <para>            3 x 2^(Size of Local Color Table+1).</para>
    /// If present, this color table temporarily becomes the active color table and the following image should be processed using it.
    /// </summary>
    public class GifLocalColorTableBlock : GifBlock
    {
        internal GifLocalColorTableBlock(GifReader reader, GifImageDescriptorBlock gifImageDescriptorBlock)
            : base(GifBlockType.LocalColorTable)
        {
            Read(reader, gifImageDescriptorBlock);
        }

        private Byte[] bytes;
        /// <summary>
        /// Contains a color table, which is a sequence of bytes representing red-green-blue color triplets.
        /// </summary>
        public Byte[] Bytes
        {
            get { return bytes; }
        }

        internal void Read(GifReader reader, GifImageDescriptorBlock gifImageDescriptorBlock)
        {
            bytes = reader.ReadBytes(gifImageDescriptorBlock.LocalColorTableSize * 3);
        }

        internal override void Write(GifWriter gifWriter)
        {
            gifWriter.Write(bytes);
        }
    }
}
