#region License
// GifDotNet is a .NET class library for processing GIF images
// http://code.google.com/p/gifdotnet/ 
//
// Copyright (c) 2011 Vurdalakov
// email: vurdalakov@gmail.com

/*
    This file is part of GifDotNet class library.
    It is licensed under LGPL, and all supplementary tools and examples are available under MIT license.

    The MIT License (MIT)

    Copyright (c) 2011 Vurdalakov

    Permission is hereby granted, free of charge, to any person obtaining
    a copy of this software and associated documentation files (the
    "Software"), to deal in the Software without restriction, including
    without limitation the rights to use, copy, modify, merge, publish,
    distribute, sublicense, and/or sell copies of the Software, and to permit
    persons to whom the Software is furnished to do so, subject to the
    following conditions:

    The above copyright notice and this permission notice shall be included
    in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
    OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
    OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion

using System;

namespace Vurdalakov.GifDotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("GifDump | Copyright (c) 2011 Vurdalakov | http://gifdotnet.googlecode.com/");
            Console.WriteLine();

            if (0 == args.Length)
            {
                Console.WriteLine("Usage:\n\tGifDump filename.gif");
                return;
            }

            String fileName = args[0];
            Console.WriteLine("= Dumping " + fileName);

            Console.WriteLine("\nMethod 1\n");
            {
                GifImage gifImage = new GifImage();
                gifImage.BlockRead += new GifImage.BlockReadEventHandler(OnBlockRead);

                gifImage.Read(fileName);
            }

            Console.WriteLine("\nMethod 2\n");
            {
                GifImage gifImage = new GifImage(fileName);

                for (int i = 0; i < gifImage.Blocks.Count; i++)
                {
                    GifBlock gifBlock = gifImage.Blocks[i];
                    Console.WriteLine(String.Format("= Block {0} of {1} ({2})", i + 1, gifImage.Blocks.Count, gifBlock.BlockName));

                    DumpBlock(gifBlock);
                }
            }
        }

        static private void OnBlockRead(object sender, GifBlockEventArgs e)
        {
            Console.WriteLine(String.Format("* '{0}' block", e.Block.BlockName));

            DumpBlock(e.Block);
        }

        static private void DumpBlock(GifBlock gifBlock)
        {
            GifExtensionBlock gifUnknownBlock = gifBlock as GifExtensionBlock;
            if (gifUnknownBlock != null)
            {
                Console.WriteLine("\tLength:\t\t\t\t" + gifUnknownBlock.Bytes.Length);
            }

            GifHeaderBlock gifHeaderBlock = gifBlock as GifHeaderBlock;
            if (gifHeaderBlock != null)
            {
                Console.WriteLine("\tVersion:\t\t\t" + gifHeaderBlock.Version);
            }

            GifLogicalScreenDescriptorBlock gifLogicalScreenDescriptorBlock = gifBlock as GifLogicalScreenDescriptorBlock;
            if (gifLogicalScreenDescriptorBlock != null)
            {
                Console.WriteLine("\tWidth:\t\t\t\t" + gifLogicalScreenDescriptorBlock.Width);
                Console.WriteLine("\tHeight:\t\t\t\t" + gifLogicalScreenDescriptorBlock.Height);

                Console.WriteLine("\tGlobalColorTableFlag:\t\t" + gifLogicalScreenDescriptorBlock.GlobalColorTableFlag);
                Console.WriteLine("\tColorResolution:\t\t" + gifLogicalScreenDescriptorBlock.ColorResolution);
                Console.WriteLine("\tGlobalColorTableSorted:\t\t" + gifLogicalScreenDescriptorBlock.GlobalColorTableSorted);
                Console.WriteLine("\tGlobalColorTableSize:\t\t" + gifLogicalScreenDescriptorBlock.GlobalColorTableSize);

                Console.WriteLine("\tBackgroundColorIndex:\t\t" + gifLogicalScreenDescriptorBlock.BackgroundColorIndex);
                Console.WriteLine("\tPixelAspectRatio:\t\t" + gifLogicalScreenDescriptorBlock.PixelAspectRatio);
            }

            GifGlobalColorTableBlock gifGlobalColorTableBlock = gifBlock as GifGlobalColorTableBlock;
            if (gifGlobalColorTableBlock != null)
            {
                Console.WriteLine("\tLength:\t\t\t\t" + gifGlobalColorTableBlock.Bytes.Length);
            }

            GifImageDescriptorBlock gifImageDescriptorBlock = gifBlock as GifImageDescriptorBlock;
            if (gifImageDescriptorBlock != null)
            {
                Console.WriteLine("\tLeft:\t\t\t\t" + gifImageDescriptorBlock.Left);
                Console.WriteLine("\tTop:\t\t\t\t" + gifImageDescriptorBlock.Top);
                Console.WriteLine("\tWidth:\t\t\t\t" + gifImageDescriptorBlock.Width);
                Console.WriteLine("\tHeight:\t\t\t\t" + gifImageDescriptorBlock.Height);
                Console.WriteLine("\tInterlaced:\t\t\t" + gifImageDescriptorBlock.Interlaced);
                Console.WriteLine("\tLocalColorTableFlag:\t\t" + gifImageDescriptorBlock.LocalColorTableFlag);
                Console.WriteLine("\tLocalColorTableSorted:\t\t" + gifImageDescriptorBlock.LocalColorTableSorted);
                Console.WriteLine("\tLocalColorTableSize:\t\t" + gifImageDescriptorBlock.LocalColorTableSize);
            }

            GifLocalColorTableBlock gifLocalColorTableBlock = gifBlock as GifLocalColorTableBlock;
            if (gifLocalColorTableBlock != null)
            {
                Console.WriteLine("\tLength:\t\t\t\t" + gifLocalColorTableBlock.Bytes.Length);
            }

            GifTableBasedImageDataBlock gifTableBasedImageDataBlock = gifBlock as GifTableBasedImageDataBlock;
            if (gifTableBasedImageDataBlock != null)
            {
                Console.WriteLine("\tLzwMinimumCodeSize:\t\t" + gifTableBasedImageDataBlock.LzwMinimumCodeSize);
                Console.WriteLine("\tLength:\t\t\t\t" + gifTableBasedImageDataBlock.ImageData.Length);
            }

            GifGraphicControExtensionBlock gifGraphicControExtensionBlock = gifBlock as GifGraphicControExtensionBlock;
            if (gifGraphicControExtensionBlock != null)
            {
                Console.WriteLine("\tDisposalMethod:\t\t\t" + gifGraphicControExtensionBlock.DisposalMethod + " (" + (int)gifGraphicControExtensionBlock.DisposalMethod + ")");
                Console.WriteLine("\tUserInputExpected:\t\t" + gifGraphicControExtensionBlock.UserInputExpected);
                Console.WriteLine("\tTransparency:\t\t\t" + gifGraphicControExtensionBlock.Transparency);
                Console.WriteLine("\tDelay:\t\t\t\t" + gifGraphicControExtensionBlock.Delay);
                Console.WriteLine("\tTransparencyIndex:\t\t" + gifGraphicControExtensionBlock.TransparencyIndex);
            }

            GifCommentExtensionBlock gifCommentExtensionBlock = gifBlock as GifCommentExtensionBlock;
            if (gifCommentExtensionBlock != null)
            {
                Console.WriteLine("\tComment:\t\t\t" + gifCommentExtensionBlock.Comment);
            }

            GifPlainTextExtensionBlock gifPlainTextExtensionBlock = gifBlock as GifPlainTextExtensionBlock;
            if (gifPlainTextExtensionBlock != null)
            {
                Console.WriteLine("\tLeft:\t\t\t\t" + gifPlainTextExtensionBlock.Left);
                Console.WriteLine("\tTop:\t\t\t\t" + gifPlainTextExtensionBlock.Top);
                Console.WriteLine("\tWidth:\t\t\t\t" + gifPlainTextExtensionBlock.Width);
                Console.WriteLine("\tHeight:\t\t\t\t" + gifPlainTextExtensionBlock.Height);
                Console.WriteLine("\tCellWidth:\t\t\t" + gifPlainTextExtensionBlock.CellWidth);
                Console.WriteLine("\tCellHeight:\t\t\t" + gifPlainTextExtensionBlock.CellHeight);
                Console.WriteLine("\tForegroundColor:\t\t" + gifPlainTextExtensionBlock.ForegroundColor);
                Console.WriteLine("\tBackgroundColor:\t\t" + gifPlainTextExtensionBlock.BackgroundColor);
                Console.WriteLine("\tText:\t\t\t" + gifPlainTextExtensionBlock.Text);
            }

            GifApplicationExtensionBlock gifApplicationExtensionBlock = gifBlock as GifApplicationExtensionBlock;
            if (gifApplicationExtensionBlock != null)
            {
                Console.WriteLine("\tApplicationIdentifier:\t\t" + gifApplicationExtensionBlock.ApplicationIdentifier);
                Console.WriteLine("\tApplicationAuthenticationCode:\t" + gifApplicationExtensionBlock.ApplicationAuthenticationCode);
            }
            GifNetscapeLoopingApplicationExtensionBlock gifNetscapeExtensionBlock = gifBlock as GifNetscapeLoopingApplicationExtensionBlock;
            if (gifNetscapeExtensionBlock != null)
            {
                Console.WriteLine("\tLoopCount:\t\t\t" + gifNetscapeExtensionBlock.LoopCount);
            }
            GifNetscapeBufferingApplicationExtensionBlock gifNetscapeBufferingExtensionBlock = gifBlock as GifNetscapeBufferingApplicationExtensionBlock;
            if (gifNetscapeBufferingExtensionBlock != null)
            {
                Console.WriteLine("\tBufferSize:\t\t\t" + gifNetscapeBufferingExtensionBlock.BufferSize);
            }
            GifAnimextsLoopingApplicationExtensionBlock gifAnimextsLoopingExtensionBlock = gifBlock as GifAnimextsLoopingApplicationExtensionBlock;
            if (gifAnimextsLoopingExtensionBlock != null)
            {
                Console.WriteLine("\tLoopCount:\t\t\t" + gifAnimextsLoopingExtensionBlock.LoopCount);
            }
        }
    }
}
