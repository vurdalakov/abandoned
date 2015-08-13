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
    /// This class represents Plain Text Extension.
    /// <para>The Plain Text Extension contains textual data and the parameters necessary to render that data as a graphic, in a simple form.</para>
    /// </summary>
    public class GifPlainTextExtensionBlock : GifBlock
    {
        internal GifPlainTextExtensionBlock(GifReader reader)
            : base(GifBlockType.PlainTextExtension)
        {
            Read(reader);
        }

        private UInt16 left;
        /// <summary>
        /// Returns Text Grid Left Position - Column number, in pixels, of the left edge of the text grid, with respect to the left edge of the Logical Screen.
        /// </summary>
        public UInt16 Left
        {
            get { return left; }
        }

        private UInt16 top;
        /// <summary>
        /// Returns Text Grid Top Position - Row number, in pixels, of the top edge of the text grid, with respect to the top edge of the Logical Screen.
        /// </summary>
        public UInt16 Top
        {
            get { return top; }
        }

        private UInt16 width;
        /// <summary>
        /// Returns Image Grid Width - Width of the text grid in pixels.
        /// </summary>
        public UInt16 Width
        {
            get { return width; }
        }

        private UInt16 height;
        /// <summary>
        /// Returns Image Grid Height - Height of the text grid in pixels.
        /// </summary>
        public UInt16 Height
        {
            get { return height; }
        }

        private Byte cellWidth;
        /// <summary>
        /// Returns Character Cell Width - Width, in pixels, of each cell in the grid.
        /// </summary>
        public Byte CellWidth
        {
            get { return cellWidth; }
        }

        private Byte cellHeight;
        /// <summary>
        /// Returns Character Cell Height - Height, in pixels, of each cell in the grid.
        /// </summary>
        public Byte CellHeight
        {
            get { return cellHeight; }
        }

        private Byte foregroundColor;
        /// <summary>
        /// Returns Text Foreground Color Index - Index into the Global Color Table to be used to render the text foreground.
        /// </summary>
        public Byte ForegroundColor
        {
            get { return foregroundColor; }
        }

        private Byte backgroundColor;
        /// <summary>
        /// Returns Text Background Color Index - Index into the Global Color Table to be used to render the text background.
        /// </summary>
        public Byte BackgroundColor
        {
            get { return backgroundColor; }
        }

        private String text;
        /// <summary>
        /// Returns text to be rendered.
        /// </summary>
        public String Text
        {
            get { return text; }
        }

        internal void Read(GifReader reader)
        {
            Byte size = reader.ReadByte();
            if (size != 12)
            {
                System.Diagnostics.Debug.WriteLine("Wrong size of " + ToString());
            }

            left = reader.ReadUInt16();
            top = reader.ReadUInt16();
            width = reader.ReadUInt16();
            height = reader.ReadUInt16();

            cellWidth = reader.ReadByte();
            cellHeight = reader.ReadByte();

            foregroundColor = reader.ReadByte();
            backgroundColor = reader.ReadByte();

            text = reader.ReadTextSubBlocks();
        }

        internal override void Write(GifWriter gifWriter)
        {
            gifWriter.WriteByte(0x21);
            gifWriter.WriteByte(0x01);
            gifWriter.WriteByte(12);

            gifWriter.WriteUInt16(left);
            gifWriter.WriteUInt16(top);
            gifWriter.WriteUInt16(width);
            gifWriter.WriteUInt16(height);

            gifWriter.WriteByte(cellWidth);
            gifWriter.WriteByte(cellHeight);

            gifWriter.WriteByte(foregroundColor);
            gifWriter.WriteByte(backgroundColor);

            gifWriter.WriteTextSubBlocks(text);

            gifWriter.WriteByte(0);
        }
    }
}
