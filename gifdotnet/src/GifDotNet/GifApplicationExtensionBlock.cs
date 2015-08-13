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
    public class GifApplicationExtensionBlock : GifBlock
    {
        internal GifApplicationExtensionBlock(GifBlockType gifBlockType)
            : base(gifBlockType)
        {
        }

        internal GifApplicationExtensionBlock(GifReader reader)
            : base(GifBlockType.ApplicationExtension)
        {
            Read(reader);
        }

        protected String applicationIdentifier;
        /// <summary>
        /// Application Identifier is a equence of eight printable ASCII characters used to identify the application owning the Application Extension.
        /// </summary>
        public String ApplicationIdentifier
        {
            get { return applicationIdentifier; }
        }

        protected String applicationAuthenticationCode;
        /// <summary>
        /// Application Authentication Code - Sequence of three bytes used to authenticate the Application Identifier. An Application program may use an algorithm to compute a binary code that uniquely identifies it as the application owning the Application Extension.
        /// </summary>
        public String ApplicationAuthenticationCode
        {
            get { return applicationAuthenticationCode; }
        }

        protected Byte[] bytes;
        /// <summary>
        /// Data contained in the block.
        /// </summary>
        public Byte[] Bytes
        {
            get { return bytes; }
        }

        internal void Read(GifReader reader)
        {
            Byte size = reader.ReadByte();
            if (size != 11)
            {
                System.Diagnostics.Debug.WriteLine("Wrong size of " + ToString());
            }

            applicationIdentifier = reader.ReadString(8);
            applicationAuthenticationCode = reader.ReadString(3);

            bytes = reader.ReadBinarySubBlocks();
        }

        internal override void Write(GifWriter gifWriter)
        {
            gifWriter.WriteByte(0x21);
            gifWriter.WriteByte(0xFF);
            gifWriter.WriteByte(0x0B);
            gifWriter.WriteString(applicationIdentifier.PadRight(8));
            gifWriter.WriteString(applicationAuthenticationCode.PadRight(3));
            gifWriter.WriteBinarySubBlocks(bytes);
            gifWriter.WriteByte(0);
        }

        internal static GifApplicationExtensionBlock Create(GifReader reader)
        {
            GifApplicationExtensionBlock gifApplicationExtensionBlock = new GifApplicationExtensionBlock(reader);

            if (gifApplicationExtensionBlock.ApplicationIdentifier.Equals("NETSCAPE") && gifApplicationExtensionBlock.ApplicationAuthenticationCode.Equals("2.0"))
            {
                if ((null != gifApplicationExtensionBlock.Bytes))
                {
                    if ((3 == gifApplicationExtensionBlock.Bytes.Length) && (1 == gifApplicationExtensionBlock.Bytes[0]))
                    {
                        gifApplicationExtensionBlock = new GifNetscapeLoopingApplicationExtensionBlock(gifApplicationExtensionBlock);
                    }
                    else if ((5 == gifApplicationExtensionBlock.Bytes.Length) && (2 == gifApplicationExtensionBlock.Bytes[0]))
                    {
                        gifApplicationExtensionBlock = new GifNetscapeBufferingApplicationExtensionBlock(gifApplicationExtensionBlock);
                    }

                }
            }
            else if (gifApplicationExtensionBlock.ApplicationIdentifier.Equals("ANIMEXTS") && gifApplicationExtensionBlock.ApplicationAuthenticationCode.Equals("1.0"))
            {
                if ((null != gifApplicationExtensionBlock.Bytes) && (3 == gifApplicationExtensionBlock.Bytes.Length) && (1 == gifApplicationExtensionBlock.Bytes[0]))
                {
                    gifApplicationExtensionBlock = new GifAnimextsLoopingApplicationExtensionBlock(gifApplicationExtensionBlock);
                }
            }

            return gifApplicationExtensionBlock;
        }
    }
}
