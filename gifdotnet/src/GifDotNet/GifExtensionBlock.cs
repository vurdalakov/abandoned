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

namespace Vurdalakov.GifDotNet
{
    public class GifExtensionBlock : GifBlock
    {
        internal GifExtensionBlock(Byte label, GifReader reader)
            : base(GifBlockType.Extension)
        {
            this.label = label;
            Read(reader);
        }

        private Byte label;
        /// <summary>
        /// Label identifies the block type.
        /// </summary>
        public Byte Label
        {
            get { return label; }
        }

        private Byte[] bytes;
        /// <summary>
        /// Data contained in the block.
        /// </summary>
        public Byte[] Bytes
        {
            get { return bytes; }
        }

        public override String BlockName
        {
            get { return String.Format("Extension 0x{0:X2}", label); }
        }

        internal void Read(GifReader reader)
        {
            bytes = reader.ReadBinarySubBlocks();
        }

        internal override void Write(GifWriter gifWriter)
        {
            gifWriter.WriteByte(0x21);
            gifWriter.WriteByte(label);
            gifWriter.WriteBinarySubBlocks(bytes);
            gifWriter.WriteByte(0);
        }
    }
}
