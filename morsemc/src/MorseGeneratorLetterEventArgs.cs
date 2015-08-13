using System;
using System.ComponentModel; // for CancelEventArgs

namespace Vurdalakov.Morse
{
    public class MorseGeneratorLetterEventArgs : CancelEventArgs
    {
        public MorseGeneratorLetterEventArgs(Char letter, Int32 index)
        {
            this.letter = letter;
            this.index = index;
        }

        private Char letter;
        public Char Letter
        {
            get { return letter; }
        }

        private Int32 index;
        public Int32 Index
        {
            get { return index; }
        }
    }
}
