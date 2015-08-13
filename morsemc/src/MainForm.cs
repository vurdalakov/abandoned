using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using Vurdalakov.Configuration;
using Vurdalakov.Reflection;
using Vurdalakov.Morse;

namespace Vurdalakov.MorseMc
{
    public partial class MainForm : Form
    {
#if !OFFLINE
        private MessengerContact contact;
#endif

        private MorseGenerator generator;
        
        private ConfigurationFile configurationFile;

        private List<Bitmap> availabilityBitmaps = new List<Bitmap>();

        private BackgroundWorker backgroundWorker = new BackgroundWorker();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            configurationFile = new ConfigurationFile();

            this.Text = String.Format("{0} {1}", Application.ProductName, Application.ProductVersion);
#if OFFLINE
            this.Text += " (OFFLINE)";
#endif

            this.textBoxText.Font = configurationFile.GetSetting("font", "Courier New", 10);

            this.toolStripButtonCancel.Enabled = false;

            EmbeddedResources embeddedResources = new EmbeddedResources();
            for (int i = 0; i < Enum.GetValues(typeof(MessengerContactAvailability)).Length; i++)
            {
                String resourceName = String.Format("Resources.availability{0}.png", i);
                availabilityBitmaps.Add(embeddedResources.GetBitmap(resourceName));
            }

            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);

            generator = new MorseGenerator();
            generator.TextStarted += new MorseGenerator.MorseGeneratorTextStartedEventHandler(MorseGenerator_TextStarted);
            generator.TextFinished += new MorseGenerator.MorseGeneratorTextFinishedEventHandler(MorseGenerator_TextFinished);
            generator.LetterStarted += new MorseGenerator.MorseGeneratorLetterStartedEventHandler(MorseGenerator_LetterStarted);
            generator.LetterFinished += new MorseGenerator.MorseGeneratorLetterFinishedEventHandler(MorseGenerator_LetterFinished);
            generator.StateChanged += new MorseGenerator.MorseGeneratorStateChangedEventHandler(MorseGenerator_StateChanged);
           
#if OFFLINE
            toolStripStatusLabelFriendlyName.Text = "Vurdalakov";
#else
            toolStripStatusLabelFriendlyName.Text = contact.FriendlyName;

            contact = new MessengerContact();
            OnContactAvailabilityChanged(contact, new EventArgs());

            contact.AvailabilityChanged += new MessengerContact.AvailabilityChangedEventHandler(OnContactAvailabilityChanged);
#endif
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ResetAvailability();
        }

        private void ChangeAvailability(int availability)
        {
            String[] availabilityStrings = { "Unknown", "Available", "Busy", "Do Not Disturb", "Be Right Back", "Away", "Offline" };

            if ((availability < 0) || (availability >= availabilityStrings.Length))
            {
                availability = 0;
            }

            toolStripStatusLabelAvailability.Text = availabilityStrings[availability];
            toolStripStatusLabelAvailability.Image = availabilityBitmaps[availability];
        }

        private void ResetAvailability()
        {
#if OFFLINE
            ChangeAvailability(1);
#else
            contact.Availability = MessengerContactAvailability.Reset;
#endif
        }

        private void OnContactAvailabilityChanged(object sender, EventArgs e)
        {
#if !OFFLINE
            ChangeAvailability((int)Math.Round((double)contact.Availability / 3000));
#endif
        }

        private class GeneratorEventArgs
        {
            public GeneratorEventArgs(String text, Boolean forever)
            {
                this.text = text;
                this.forever = forever;
            }

            private String text;
            public String Text
            {
                get { return text; }
            }

            private Boolean forever;
            public Boolean Forever
            {
                get { return forever; }
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            GeneratorEventArgs generatorEventArgs = e.Argument as GeneratorEventArgs;
            if (null == generatorEventArgs)
            {
                throw new ArgumentException();
            }

            EnableControls(false);

            while (!backgroundWorker.CancellationPending)
            {
                generator.Generate(generatorEventArgs.Text);

                if (!generatorEventArgs.Forever)
                {
                    break;
                }

                for (int i = 0; i < 30; i++)
                {
                    Thread.Sleep(100);

                    if (backgroundWorker.CancellationPending)
                    {
                        break;
                    }
                }
            }

            ResetAvailability();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableControls(true);

            if (e.Error != null) // TODO: debug only?
            {
                MessageBox.Show(this, e.Error.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        void MorseGenerator_StateChanged(object sender, MorseGeneratorStateChangedEventArgs e)
        {
#if OFFLINE
            ChangeAvailability(e.NewState ? 2 : 1);
#else
            contact.Availability = e.NewState ? MessengerContactAvailability.Busy : MessengerContactAvailability.Available;
#endif

            e.Cancel = backgroundWorker.CancellationPending;
        }

        int selectionStart = 0;
        int selectionLength = 0;

        delegate void EnableControlsCallback(bool enable);
        private void EnableControls(bool enable)
        {
            if (this.textBoxText.InvokeRequired)
            {
                EnableControlsCallback enableControlsCallback = new EnableControlsCallback(EnableControls);
                Invoke(enableControlsCallback, new object[] { enable });
            }
            else
            {
                this.textBoxText.ReadOnly = !enable;
                this.toolStripButtonSendOnce.Enabled = enable;
                this.toolStripButtonSendForever.Enabled = enable;
                this.toolStripButtonCancel.Enabled = !enable;
                this.toolStripDropDownButtonConvert.Enabled = enable;
                this.toolStripButtonOptions.Enabled = enable;

                if (enable)
                {
                    this.textBoxText.SelectionStart = selectionStart;
                    this.textBoxText.SelectionLength = selectionLength;
                }
                else
                {
                    selectionStart = this.textBoxText.SelectionStart;
                    selectionLength = this.textBoxText.SelectionLength;
                }

                this.toolStripStatusLabelCurrent.Visible = !enable;
            }
        }

        void MorseGenerator_TextStarted(object sender, MorseGeneratorTextEventArgs e)
        {
//            EnableControls(false);
        }

        void MorseGenerator_TextFinished(object sender, MorseGeneratorTextEventArgs e)
        {
//            EnableControls(true);
        }

        delegate void SelectLetterCallback(int index);
        private void SelectLetter(int index)
        {
            if (this.textBoxText.InvokeRequired)
            {
                SelectLetterCallback selectLetterCallback = new SelectLetterCallback(SelectLetter);
                Invoke(selectLetterCallback, new object[] { index });
            }
            else
            {
                if ((index >= 0) && (index < this.textBoxText.Text.Length))
                {
                    this.textBoxText.SelectionStart = index;
                    this.textBoxText.SelectionLength = 1;
                }
                else
                {
                    this.textBoxText.SelectionStart = 0;
                    this.textBoxText.SelectionLength = 0;
                }

                this.textBoxText.ScrollToCaret();
            }
        }

        void MorseGenerator_LetterStarted(object sender, MorseGeneratorLetterEventArgs e)
        {
            SelectLetter(e.Index);

            char c = e.Letter;
            String text = ' ' == c ? "<word spacing>" : String.Format("{0}     {1}   ", c, generator.ConvertCharToMorse(c));
            this.toolStripStatusLabelCurrent.Text = "Now sending:     " + text;
        }

        void MorseGenerator_LetterFinished(object sender, MorseGeneratorLetterEventArgs e)
        {
//            SelectLetter(-1);
        }

        private Boolean CheckText()
        {
            if (0 == this.textBoxText.Text.Length)
            {
                MessageBox.Show(this, "Please type in some text first.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        private void Generate(bool forever)
        {
            if (!CheckText())
            {
                return;
            }

            generator.UnitDuration = configurationFile.GetSetting("unitDuration", 250);

            generator.DotDuration = configurationFile.GetSetting("dotDuration", 1);
            generator.DashDuration = configurationFile.GetSetting("dashDuration", 3);
            generator.GapDuration = configurationFile.GetSetting("gapDuration", 1);

            generator.LetterSpacing = configurationFile.GetSetting("letterSpacing", 3);
            generator.WordSpacing = configurationFile.GetSetting("wordSpacing", 7);


            GeneratorEventArgs generatorEventArgs = new GeneratorEventArgs(this.textBoxText.Text, forever);
            backgroundWorker.RunWorkerAsync(generatorEventArgs);
        }

        private void toolStripButtonSendOnce_Click(object sender, EventArgs e)
        {
            Generate(false);
        }

        private void toolStripButtonSendForever_Click(object sender, EventArgs e)
        {
            Generate(true);
        }

        private void toolStripButtonCancel_Click(object sender, EventArgs e)
        {
            backgroundWorker.CancelAsync();
        }

        private String convertUndoString = "";

        private void textToMorseCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckText())
            {
                return;
            }

            convertUndoString = this.textBoxText.Text;

            this.textBoxText.Text = generator.ConvertTextToMorse(convertUndoString);

            this.undoToolStripMenuItem.Enabled = convertUndoString.Length > 0;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.textBoxText.Text = convertUndoString;
            convertUndoString = "";
            this.undoToolStripMenuItem.Enabled = false;
        }

        private void toolStripButtonOptions_Click(object sender, EventArgs e)
        {
            OptionsForm optionsForm = new OptionsForm(configurationFile);
            if (DialogResult.OK == optionsForm.ShowDialog(this))
            {
                this.textBoxText.Font = optionsForm.Font;
            }
        }

        private void toolStripButtonAbout_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog(this);
        }

        private void textBoxText_TextChanged(object sender, EventArgs e)
        {
            this.undoToolStripMenuItem.Enabled = false;
        }
    }
}
