using System;
using System.Threading; // for Thread.Sleep

namespace Vurdalakov.Morse
{
    public class MorseGenerator
    {
        public MorseGenerator()
        {
        }

        #region Properties

        private Boolean state = false;
        /// <summary>
        /// Gets the on/off state of the object.
        /// </summary>
        public Boolean State
        {
            get
            {
                return state;
            }
        }

        private int unitDuration = 250;
        /// <summary>
        /// Sets or gets duration of one unit in milliseconds.
        /// <para>Default value is 250 milliseconds.</para>
        /// </summary>
        public int UnitDuration
        {
            get
            {
                return unitDuration;
            }
            set
            {
                unitDuration = value;
            }
        }

        private int dotDuration = 1;
        /// <summary>
        /// Sets or gets duration of one dot (dit) in units.
        /// <para>Default value is 1 unit.</para>
        /// </summary>
        public int DotDuration
        {
            get
            {
                return dotDuration;
            }
            set
            {
                dotDuration = value;
            }
        }

        private int dashDuration = 3;
        /// <summary>
        /// Sets or gets duration of one dash (dah) in units.
        /// <para>Default value is 3 units.</para>
        /// </summary>
        public int DashDuration
        {
            get
            {
                return dashDuration;
            }
            set
            {
                dashDuration = value;
            }
        }

        private int gapDuration = 1;
        /// <summary>
        /// Sets or gets duration of inter-element gap between the dots and dashes within a character.
        /// <para>Default value is 1 unit.</para>
        /// </summary>
        public int GapDuration
        {
            get
            {
                return gapDuration;
            }
            set
            {
                gapDuration = value;
            }
        }

        private int letterSpacing = 3;
        /// <summary>
        /// Sets or gets letter spacing in units.
        /// <para>Letter spacing is the size of the space between letters.</para>
        /// <para>Default value is 3 units.</para>
        /// <seealso cref="MorseGenerator.WordSpacing"/>
        /// </summary>
        public int LetterSpacing
        {
            get
            {
                return letterSpacing;
            }
            set
            {
                letterSpacing = value;
            }
        }

        private int wordSpacing = 7;
        /// <summary>
        /// Sets or gets word spacing in units.
        /// <para>Word spacing is the size of the space between words.</para>
        /// <para>Default value is 7 units.</para>
        /// <seealso cref="MorseGenerator.LetterSpacing"/>
        /// </summary>
        public int WordSpacing
        {
            get
            {
                return wordSpacing;
            }
            set
            {
                wordSpacing = value;
            }
        }

        #endregion

        #region Events

        private bool cancelled = false;

        public delegate void MorseGeneratorStateChangedEventHandler(object sender, MorseGeneratorStateChangedEventArgs e);
        public event MorseGeneratorStateChangedEventHandler StateChanged;

        private bool ChangeState(Boolean newState)
        {
            if (LetterStarted != null)
            {
                if (state != newState)
                {
                    state = newState;

                    MorseGeneratorStateChangedEventArgs morseGeneratorStateChangedEventArgs = new MorseGeneratorStateChangedEventArgs(state);
                    StateChanged(this, morseGeneratorStateChangedEventArgs);

                    cancelled = morseGeneratorStateChangedEventArgs.Cancel;
                }
            }

            return !cancelled;
        }

        public delegate void MorseGeneratorLetterStartedEventHandler(object sender, MorseGeneratorLetterEventArgs e);
        public event MorseGeneratorLetterStartedEventHandler LetterStarted;

        private bool StartLetter(Char letter, Int32 index)
        {
            if (LetterStarted != null)
            {
                MorseGeneratorLetterEventArgs morseGeneratorLetterEventArgs = new MorseGeneratorLetterEventArgs(letter, index);
                LetterStarted(this, morseGeneratorLetterEventArgs);

                cancelled = morseGeneratorLetterEventArgs.Cancel;
            }

            return !cancelled;
        }

        public delegate void MorseGeneratorLetterFinishedEventHandler(object sender, MorseGeneratorLetterEventArgs e);
        public event MorseGeneratorLetterFinishedEventHandler LetterFinished;

        private bool FinishLetter(Char letter, Int32 index)
        {
            if (LetterStarted != null)
            {
                MorseGeneratorLetterEventArgs morseGeneratorLetterEventArgs = new MorseGeneratorLetterEventArgs(letter, index);
                LetterFinished(this, morseGeneratorLetterEventArgs);

                cancelled = morseGeneratorLetterEventArgs.Cancel;
            }

            return !cancelled;
        }

        public delegate void MorseGeneratorTextStartedEventHandler(object sender, MorseGeneratorTextEventArgs e);
        public event MorseGeneratorTextStartedEventHandler TextStarted;

        private bool StartText(String text)
        {
            if (TextStarted != null)
            {
                MorseGeneratorTextEventArgs morseGeneratorTextEventArgs = new MorseGeneratorTextEventArgs(text);
                TextStarted(this, morseGeneratorTextEventArgs);
            }

            return !cancelled;
        }

        public delegate void MorseGeneratorTextFinishedEventHandler(object sender, MorseGeneratorTextEventArgs e);
        public event MorseGeneratorTextFinishedEventHandler TextFinished;

        private bool FinishText(String text)
        {
            if (TextFinished != null)
            {
                MorseGeneratorTextEventArgs morseGeneratorTextEventArgs = new MorseGeneratorTextEventArgs(text);
                TextFinished(this, morseGeneratorTextEventArgs);
            }

            return !cancelled;
        }

        #endregion

        #region Generator

        //    ' ',
        //    '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/',
        //    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        //    ':', ';', '<', '=', '>', '?', '@', 
        //    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        //    '[', '\', ']', '^', '_', '`'

        private static String[] morseCodes = new String[]
        {
            " ",
            "--..--", ".-..-.", "", "...-..-", "", ".-...", ".----.", "-.--.-", "-.--.-", "", ".-.-.", ".-.-.-", "-....-", "......", "-..-.",
            "-----", ".----", "..---", "...--", "....-", ".....", "-....", "--...", "---..", "----.",
            "---...", "-.-.-", "", "-...-", "", "..--..", ".--.-.",
            ".-", "-...", "-.-.", "-..", ".", "..-.", "--.", "....", "..", ".---", "-.-", ".-..",  "--", "-.", "---", ".--.", "--.-", ".-.", "...", "-", "..-", "...-", ".--", "-..-", "-.--", "--..",
            "", "", "", "", "..--.-", ""
        };

        private static String unknownCharMorse = morseCodes['?' - ' '];

        public void Generate(String text)
        {
            cancelled = false;

            try
            {
                StartText(text);

                for (int i = 0; i < text.Length; i++)
                {
                    if (i > 0)
                    {
                        Thread.Sleep(unitDuration * letterSpacing);
                    }

                    Char c = text[i];

                    if (!StartLetter(c, i))
                    {
                        return;
                    }

                    String morseCode = ConvertCharToMorse(c);

                    for (int j = 0; j < morseCode.Length; j++)
                    {
                        Char ch = morseCode[j];

                        if (' ' == ch)
                        {
                            if (i > 0)
                            {
                                Thread.Sleep(unitDuration * (wordSpacing - letterSpacing));
                            }
                        }
                        else
                        {
                            if (j > 0)
                            {
                                Thread.Sleep(unitDuration * gapDuration);
                            }

                            if (!ChangeState(true))
                            {
                                return;
                            }

                            if ('.' == ch)
                            {
                                Thread.Sleep(unitDuration * dotDuration);
                            }
                            else if ('-' == ch)
                            {
                                Thread.Sleep(unitDuration * dashDuration);
                            }
                            else
                            {
                                throw new Exception(String.Format("Unknown symbol '{0}' (0x{1:X4}) in character '{2}' ({3})", ch, (int)ch, c, (int)c));
                            }

                            if (!ChangeState(false))
                            {
                                return;
                            }
                        }
                    }

                    if (!FinishLetter(c, i))
                    {
                        return;
                    }
                }
            }
            finally
            {
                ChangeState(false);

                FinishText(text);
            }
        }

        public String ConvertCharToMorse(Char c)
        {
            int index = Char.ToUpper(c) - ' ';

            // ignore control characters
            if (index < 0)
            {
                return "";
            }

            // replace unsuported characters with ?
            if (index >= morseCodes.Length)
            {
                return unknownCharMorse;
            }

            String morse = morseCodes[index];

            // replace unsuported characters with ?
            if (0 == morse.Length)
            {
                return unknownCharMorse;
            }

            return morse;
        }

        public String ConvertTextToMorse(String text)
        {
            text = text.Trim();

            while (text.IndexOf("  ") > 0)
            {
                text = text.Replace("  ", " ");
            }

            String morse = "";

            foreach (Char c in text)
            {
                if (' ' == c)
                {
                    morse += "   ";
                }
                else
                {
                    if (morse.Length > 0)
                    {
                        morse += ' ';
                    }

                    morse += ConvertCharToMorse(c);
                }
            }

            return morse;
        }

        #endregion
    }
}
