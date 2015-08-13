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
    public class GifNetscapeLoopingApplicationExtensionBlock : GifApplicationExtensionBlock
    {
        internal GifNetscapeLoopingApplicationExtensionBlock(GifApplicationExtensionBlock gifApplicationExtensionBlock)
            : base(GifBlockType.NetscapeLoopingApplicationExtension)
        {
            applicationIdentifier = gifApplicationExtensionBlock.ApplicationIdentifier;
            applicationAuthenticationCode = gifApplicationExtensionBlock.ApplicationAuthenticationCode;
            bytes = gifApplicationExtensionBlock.Bytes;

            if ((null == bytes) || (bytes.Length != 3) || (bytes[0] != 1))
            {
                System.Diagnostics.Debug.WriteLine("Wrong format of " + ToString());
            }

            loopCount = (UInt16)(((UInt16)bytes[2] << 8) | (UInt16)bytes[1]);
        }

        private UInt16 loopCount;
        /// <summary>
        /// Indicates the number of iterations the loop should be executed.
        /// </summary>
        public UInt16 LoopCount
        {
            get { return loopCount; }
        }

        internal override void Write(GifWriter gifWriter)
        {
            gifWriter.WriteByte(0x21);
            gifWriter.WriteByte(0xFF);
            gifWriter.WriteByte(0x0B);
            gifWriter.WriteString(applicationIdentifier.PadRight(8));
            gifWriter.WriteString(applicationAuthenticationCode.PadRight(3));
            gifWriter.WriteByte(3);
            gifWriter.WriteByte(1);
            gifWriter.WriteUInt16(loopCount);
            gifWriter.WriteByte(0);
        }
    }
}
