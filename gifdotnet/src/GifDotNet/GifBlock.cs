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
using System.IO;

namespace Vurdalakov.GifDotNet
{
    public abstract class GifBlock
    {
        internal GifBlock(GifBlockType gifBlockType)//, String gifBlockName)
        {
            this.gifBlockType = gifBlockType;
            //this.gifBlockName = gifBlockName;
        }

        private GifBlockType gifBlockType;
        /// <summary>
        /// Gets GIF block type.
        /// </summary>
        public GifBlockType BlockType
        {
            get { return gifBlockType; }
        }

        public virtual String BlockName
        {
            get
            {
                String blockName = gifBlockType.ToString();

                String result = blockName[0].ToString();
                for (int i = 1; i < blockName.Length; i++)
                {
                    Char c = blockName[i];
                    if (Char.IsUpper(c))
                    {
                        result += ' ';
                    }
                    result += c;
                }

                return result;
            }
        }

        internal abstract void Write(GifWriter gifWriter);

        // TODO: move ?
        protected Byte Log2(UInt16 value)
        {
            UInt16 test = 2;
            for (Byte i = 0; i < 8; i++)
            {
                if (test == value)
                {
                    return i;
                }
                test <<= 1;
            }
            return 0;
        }
    }
}
