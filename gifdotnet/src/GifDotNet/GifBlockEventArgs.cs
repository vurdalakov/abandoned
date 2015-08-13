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
    /// Encapsulates GIF block associated with a BlockRead event.
    /// </summary>
    public class GifBlockEventArgs
    {
        internal GifBlockEventArgs(GifBlock gifBlock)
        {
            this.gifBlock = gifBlock;
        }

        private GifBlock gifBlock;
        /// <summary>
        /// Gets GIF block associated with this event.
        /// </summary>
        /// <value>The GIF block associated with this event.</value>
        public GifBlock Block
        {
            get { return gifBlock; }
        }
    }
}
