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
    /// Defines GIF block types. Values corresponds to appropriate section number in GIF89a specification if applicable.
    /// </summary>
    public enum GifBlockType
    {
        Header = 17, LogicalScreenDescriptor = 18, GlobalColorTable = 19, ImageDescriptor = 20, LocalColorTable = 21, TableBasedImageData = 22,
        GraphicControlExtension = 23, CommentExtension = 24, PlainTextExtension = 25, ApplicationExtension = 26, Trailer = 27,
        Extension = 1000,
        NetscapeLoopingApplicationExtension = 2600, NetscapeBufferingApplicationExtension = 2601, AnimextsLoopingApplicationExtension = 2602
    }
}
