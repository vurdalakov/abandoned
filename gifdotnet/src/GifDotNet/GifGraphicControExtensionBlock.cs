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
    public class GifGraphicControExtensionBlock : GifBlock
    {
        internal GifGraphicControExtensionBlock(GifReader reader)
            : base(GifBlockType.GraphicControlExtension)
        {
            Read(reader);
        }

        private GifDisposalMethod disposalMethod;
        /// <summary>
        /// Disposal Method indicates the way in which the graphic is to be treated after being displayed.
        /// Values:
        /// 0 - No disposal specified. The decoder is not required to take any action.
        /// 1 - Do not dispose. The graphic is to be left in place.
        /// 2 - Restore to background color. The area used by the graphic must be restored to the background color.
        /// 3 - Restore to previous. The decoder is required to restore the area overwritten by the graphic with what was there prior to rendering the graphic.
        /// </summary>
        public GifDisposalMethod DisposalMethod
        {
            get { return disposalMethod; }
        }

        private Boolean userInputExpected;
        /// <summary>
        /// Indicates whether or not user input is expected before continuing. If the flag is set, processing will continue when user input is entered. The nature of the User input is determined by the application (Carriage Return, Mouse Button Click, etc.).
        /// <para>When a Delay Time is used and the User Input Flag is set, processing will continue when user input is received or when the delay time expires, whichever occurs first.</para>
        /// </summary>
        public Boolean UserInputExpected
        {
            get { return userInputExpected; }
        }

        private Boolean transparency;
        /// <summary>
        /// Indicates whether a transparency index is given in the TransparentIndex field.
        /// </summary>
        public Boolean Transparency
        {
            get { return transparency; }
        }

        private UInt16 delay;
        /// <summary>
        /// If not 0, this field specifies the number of hundredths (1/100) of a second to wait before continuing with the processing of the Data Stream. The clock starts ticking immediately after the graphic is rendered.
        /// </summary>
        public UInt16 Delay
        {
            get { return delay; }
        }

        private Byte transparencyIndex;
        /// <summary>
        /// If not 0, this field specifies the number of hundredths (1/100) of a second to wait before continuing with the processing of the Data Stream. The clock starts ticking immediately after the graphic is rendered.
        /// </summary>
        public Byte TransparencyIndex
        {
            get { return transparencyIndex; }
        }

        internal void Read(GifReader reader)
        {
            Byte size = reader.ReadByte();
            if (size != 4)
            {
                System.Diagnostics.Debug.WriteLine("Wrong size of " + ToString());
            }

            Byte flags = reader.ReadByte();
            disposalMethod = (GifDisposalMethod)((flags & 0x1C) >> 2);
            userInputExpected = 0x02 == (flags & 0x02);
            transparency = 0x01 == (flags & 0x01);

            delay = reader.ReadUInt16();
            transparencyIndex = reader.ReadByte();

            Byte terminator = reader.ReadByte();
            if (terminator != 0x00)
            {
                throw new Exception("Unknown extension terminator: 0x" + terminator.ToString("X2"));
            }
        }

        internal override void Write(GifWriter gifWriter)
        {
            gifWriter.WriteByte(0x21);
            gifWriter.WriteByte(0xF9);
            gifWriter.WriteByte(4);

            Byte flags = (byte)((transparency ? 1 : 0) | (userInputExpected ? 2 : 0) | ((int)disposalMethod << 2));
            gifWriter.WriteByte(flags);

            gifWriter.WriteUInt16(delay);
            gifWriter.WriteByte(transparencyIndex);
            gifWriter.WriteByte(0);
        }
    }
}
