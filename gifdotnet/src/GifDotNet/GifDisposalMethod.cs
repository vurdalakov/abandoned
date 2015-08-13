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
    /// Disposal Method indicates the way in which the graphic is to be treated after being displayed.
    /// Values:
    /// 0 - No disposal specified. The decoder is not required to take any action.
    /// 1 - Do not dispose. The graphic is to be left in place.
    /// 2 - Restore to background color. The area used by the graphic must be restored to the background color.
    /// 3 - Restore to previous. The decoder is required to restore the area overwritten by the graphic with what was there prior to rendering the graphic.
    /// </summary>
    public enum GifDisposalMethod { NoDisposalSpecified = 0, DoNotDispose = 1, RestoreToBackgroundColor = 2, RestoreToPrevious = 3 }
}
