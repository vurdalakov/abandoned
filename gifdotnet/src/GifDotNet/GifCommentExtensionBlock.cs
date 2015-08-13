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
    public class GifCommentExtensionBlock : GifBlock
    {
        internal GifCommentExtensionBlock(GifReader reader)
            : base(GifBlockType.CommentExtension)
        {
            Read(reader);
        }

        private String comment;
        /// <summary>
        /// Contains textual information which is not part of the actual graphics in the GIF Data Stream. It is suitable for including comments about the graphics, credits, descriptions or any other type of non-control and non-graphic data.
        /// </summary>
        public String Comment
        {
            get { return comment; }
        }

        internal void Read(GifReader reader)
        {
            comment = reader.ReadTextSubBlocks();
        }

        internal override void Write(GifWriter gifWriter)
        {
            gifWriter.WriteByte(0x21);
            gifWriter.WriteByte(0xFE);
            gifWriter.WriteTextSubBlocks(comment);
        }
    }
}
