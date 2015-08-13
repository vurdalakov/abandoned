using System;

namespace Vurdalakov.Morse
{
    public class MorseGeneratorTextEventArgs
    {
        public MorseGeneratorTextEventArgs(String text)
        {
            this.text = text;
        }

        private String text;
        public String Text
        {
            get { return text; }
        }
    }
}
