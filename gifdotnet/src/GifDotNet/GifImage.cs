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
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Vurdalakov.GifDotNet
{
    public class GifImage
    {
        public GifImage()
        {
        }
        
        public GifImage(String fileName)
        {
            Read(fileName);
        }

        public GifImage(Stream stream)
        {
            Read(stream);
        }

        private List<GifBlock> blocks = new List<GifBlock>();
        public List<GifBlock> Blocks // TODO
        {
            get { return blocks; }
        }

        public void Read(String fileName)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                Read(fileStream);
            }
        }

        public void Read(Stream stream)
        {
            blocks.Clear();

            // reader

            GifReader reader = new GifReader(stream);

            // Header

            AddBlock(new GifHeaderBlock(reader));

            // Logical Screen Descriptor

            GifLogicalScreenDescriptorBlock gifLogicalScreenDescriptorBlock = new GifLogicalScreenDescriptorBlock(reader);
            AddBlock(gifLogicalScreenDescriptorBlock);

            // Global Color Table

            if (gifLogicalScreenDescriptorBlock.GlobalColorTableFlag)
            {
                AddBlock(new GifGlobalColorTableBlock(reader, gifLogicalScreenDescriptorBlock));
            }

            // Segments

            while (true)
            {
                Byte identifier = reader.ReadByte();
                switch (identifier)
                {
                    // Frames

                    case 0x2C:
                        GifImageDescriptorBlock gifImageDescriptorBlock = new GifImageDescriptorBlock(reader);
                        AddBlock(gifImageDescriptorBlock);

                        if (gifImageDescriptorBlock.LocalColorTableFlag)
                        {
                            AddBlock(new GifLocalColorTableBlock(reader, gifImageDescriptorBlock));
                        }

                        AddBlock(new GifTableBasedImageDataBlock(reader));
                        break;

                    // Extensions

                    case 0x21:
                        Byte label = reader.ReadByte();

                        switch (label)
                        {
                            case 0xF9:
                                AddBlock(new GifGraphicControExtensionBlock(reader));
                                break;

                            case 0xFE:
                                AddBlock(new GifCommentExtensionBlock(reader));
                                break;

                            case 0x01:
                                AddBlock(new GifPlainTextExtensionBlock(reader));
                                break;

                            case 0xFF:
                                AddBlock(GifApplicationExtensionBlock.Create(reader));
                                break;

                            default:
                                AddBlock(new GifExtensionBlock(label, reader));
                                break;
                        }
                        break;

                    // Trailer

                    case 0x3B:
                        AddBlock(new GifTrailerBlock());
                        return;

                    default:
                        throw new Exception("Unknown segment identifier: 0x" + identifier.ToString("X2"));
                }
            }
        }

        /// <summary>
        /// Represents the method that will handle the BlockRead event.
        /// </summary>
        /// <param name="sender">The source of the event. </param>
        /// <param name="e">An GifBlockEventArgs that contains the event data.</param>
        public delegate void BlockReadEventHandler(object sender, GifBlockEventArgs e);

        /// <summary>
        /// Occurs when GIF block is read from the source.
        /// </summary>
        public event BlockReadEventHandler BlockRead;

        private void AddBlock(GifBlock gifBlock)
        {
            blocks.Add(gifBlock);

            if (BlockRead != null)
            {
                BlockRead(this, new GifBlockEventArgs(gifBlock));
            }
        }

        public void Write(String fileName)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                Write(fileStream);
            }
        }

        public void Write(Stream stream)
        {
            GifWriter gifWriter = new GifWriter(stream);

            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].Write(gifWriter);
            }
        }
    }
}
