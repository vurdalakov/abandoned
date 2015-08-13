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
    /// This class represents the Logical Screen Descriptor block.
    /// <para>The Logical Screen Descriptor contains the parameters necessary to define the area of the display device within which the images will be rendered.</para>
    /// <para>The coordinates in this block are given with respect to the top-left corner of the virtual screen; they do not necessarily refer to absolute coordinates on the display device.
    /// This implies that they could refer to window coordinates in a window-based environment or printer coordinates when a printer is used.</para>
    /// </summary>
    public class GifLogicalScreenDescriptorBlock : GifBlock
    {
        internal GifLogicalScreenDescriptorBlock(GifReader reader)
            : base(GifBlockType.LogicalScreenDescriptor)
        {
            Read(reader);
        }


        private UInt16 width;
        /// <summary>
        /// Logical Screen Width - Width, in pixels, of the Logical Screen where the images will be rendered in the displaying device.
        /// </summary>
        public UInt16 Width
        {
            get { return width; }
        }

        private UInt16 height;
        /// <summary>
        /// Logical Screen Height - Height, in pixels, of the Logical Screen where the images will be rendered in the displaying device.
        /// </summary>
        public UInt16 Height
        {
            get { return height; }
        }

        private Boolean globalColorTableFlag;
        /// <summary>
        /// Global Color Table Flag - Flag indicating the presence of a Global Color Table; if the flag is set, the Global Color Table will immediately follow the Logical Screen Descriptor.
        /// This flag also selects the interpretation of the Background Color Index; if the flag is set, the value of the Background Color Index field should be used as the table index of the background color.
        /// </summary>
        public Boolean GlobalColorTableFlag
        {
            get { return globalColorTableFlag; }
        }

        private Byte colorResolution;
        /// <summary>
        /// Color Resolution - Number of bits per primary color available to the original image.
        /// This value represents the size of the entire palette from which the colors in the graphic were selected, not the number of colors actually used in the graphic.
        /// For example, if the value in this field is 3, then the palette of the original image had 4 bits per primary color available to create the image.
        /// This value should be set to indicate the richness of the original palette, even if not every color from the whole palette is available on the source machine.
        /// </summary>
        public Byte ColorResolution
        {
            get { return colorResolution; }
        }

        private Boolean globalColorTableSorted;
        /// <summary>
        /// Sort Flag - Indicates whether the Global Color Table is sorted.
        /// If the flag is set, the Global Color Table is sorted, in order of decreasing importance. Typically, the order would be decreasing frequency, with most frequent color first.
        /// This assists a decoder, with fewer available colors, in choosing the best subset of colors; the decoder may use an initial segment of the table to render the graphic.
        /// </summary>
        public Boolean GlobalColorTableSorted
        {
            get { return globalColorTableSorted; }
        }

        private UInt16 globalColorTableSize;
        /// <summary>
        /// Size of Global Color Table - If the Global Color Table Flag is set, the value reprtesents the number of bytes contained in the Global Color Table.
        /// Even if there is no Global Color Table specified, set this field so that decoders can choose the best graphics mode to display the stream in.
        /// </summary>
        public UInt16 GlobalColorTableSize
        {
            get { return globalColorTableSize; }
        }

        private Byte backgroundColorIndex;
        /// <summary>
        /// Background Color Index - Index into the Global Color Table for the Background Color.
        /// The Background Color is the color used for those pixels on the screen that are not covered by an image.
        /// If the Global Color Table Flag is set to (zero), this field should be zero and should be ignored.
        /// </summary>
        public Byte BackgroundColorIndex
        {
            get { return backgroundColorIndex; }
        }

        private Byte pixelAspectRatio;
        /// <summary>
        /// Pixel Aspect Ratio - Factor used to compute an approximation of the aspect ratio of the pixel in the original image.
        /// If the value of the field is not 0, this approximation of the aspect ratio is computed based on the formula:
        /// <para>Aspect Ratio = (Pixel Aspect Ratio + 15) / 64</para>
        /// The Pixel Aspect Ratio is defined to be the quotient of the pixel's width over its height.
        /// The value range in this field allows specification of the widest pixel of 4:1 to the tallest pixel of 1:4 in increments of 1/64th.
        /// </summary>
        public Byte PixelAspectRatio
        {
            get { return pixelAspectRatio; }
        }

        internal void Read(GifReader reader)
        {
            width = reader.ReadUInt16();
            height = reader.ReadUInt16();

            Byte flags = reader.ReadByte();
            globalColorTableFlag = 0x80 == (flags & 0x80);
            colorResolution = (Byte)(((flags & 0x70) >> 4) + 1);
            globalColorTableSorted = 0x08 == (flags & 0x08);
            globalColorTableSize = (UInt16)(2 << (flags & 0x07));

            backgroundColorIndex = reader.ReadByte();
            pixelAspectRatio = reader.ReadByte();
        }

        internal override void Write(GifWriter gifWriter)
        {
            gifWriter.WriteUInt16(width);
            gifWriter.WriteUInt16(height);

            Byte flags = (byte)(Log2(globalColorTableSize) | (globalColorTableSorted ? 0x08 : 0) | ((colorResolution - 1) << 4) | (globalColorTableFlag ? 0x80 : 0));
            gifWriter.WriteByte(flags);

            gifWriter.WriteByte(backgroundColorIndex);
            gifWriter.WriteByte(pixelAspectRatio);
        }
    }
}
