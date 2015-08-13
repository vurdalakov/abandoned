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
    /// This class represents the Header block.
    /// <para>The Header identifies the GIF Data Stream in context.</para>
    /// </summary>
    public class GifHeaderBlock : GifBlock
    {
        internal GifHeaderBlock(GifReader reader)
            : base(GifBlockType.Header)
        {
            Read(reader);
        }

        private String version;
        /// <summary>
        /// Version - Version number used to format the data stream. Identifies the minimum set of capabilities necessary to a decoder to fully process the contents of the Data Stream.
        /// <para>Version Numbers as of 10 July 1990: "87a" - May 1987, "89a" - July 1989.</para>
        /// </summary>
        public String Version
        {
            get { return version; }
        }

        internal void Read(GifReader reader)
        {
            String signature = reader.ReadString(3);
            if (signature != "GIF")
            {
                throw new Exception("No GIF signature");
            }

            version = reader.ReadString(3);
            if ((version != "87a") && (version != "89a"))
            {
                throw new Exception("Unsupported GIF version: " + version);
            }
        }

        internal override void Write(GifWriter gifWriter)
        {
            gifWriter.WriteString("GIF");
            gifWriter.WriteString(version.PadRight(3));
        }
    }
}
