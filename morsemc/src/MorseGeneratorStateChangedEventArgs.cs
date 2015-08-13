using System;
using System.ComponentModel; // for CancelEventArgs

namespace Vurdalakov.Morse
{
    public class MorseGeneratorStateChangedEventArgs : CancelEventArgs
    {
        public MorseGeneratorStateChangedEventArgs(Boolean newState)
        {
            this.newState = newState;
        }

        private Boolean newState;
        public Boolean NewState
        {
            get { return newState; }
        }
    }
}
