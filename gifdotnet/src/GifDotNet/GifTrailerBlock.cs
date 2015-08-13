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
    /// This class represents the Trailer block.
    /// <para>This block is a single-field block indicating the end of the GIF Data Stream.  It contains the fixed value 0x3B.</para>
    /// </summary>
    public class GifTrailerBlock : GifBlock
    {
        internal GifTrailerBlock()
            : base(GifBlockType.Trailer)
        {
        }

        internal override void Write(GifWriter gifWriter)
        {
            gifWriter.WriteByte(0x3B);
        }
    }
}
