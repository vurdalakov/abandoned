using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace Vurdalakov.GooglePlusAlbum
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text = String.Format("Google+ Album Download {0}", Application.ProductVersion);

            this.buttonGetImageUrls.Enabled = false;
            this.buttonCopyImageUrls.Enabled = false;

            PasteUrl();
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            PasteUrl();
        }

        private void PasteUrl()
        {
            if (String.IsNullOrEmpty(this.textBoxAlbumUrl.Text) && Clipboard.ContainsText())
            {
                String url = Clipboard.GetText();

                if (url.StartsWith("http") && url.Contains("plus.google.com") && url.Contains("/photos/"))
                {
                    this.textBoxAlbumUrl.Text = url;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void homePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://code.google.com/p/gpalbum/");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(String.Format("{0} v{1}\n\nCopyright  (c) 2012 Vurdalakov\n\nhttp://code.google.com/p/gpalbum/", Application.ProductName, Application.ProductVersion),
                "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowException(Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void textBoxAlbumUrl_TextChanged(object sender, EventArgs e)
        {
            this.buttonGetImageUrls.Enabled = !String.IsNullOrEmpty(this.textBoxAlbumUrl.Text);
        }

        private void ApplyAlternativeMethod()
        {
            WebClient webClient = new WebClient();
            String xmlText = webClient.DownloadString(this.textBoxAlbumUrl.Text);

            MatchCollection matchCollection = Regex.Matches(xmlText, @"""(https?://[\w\.\-]*?googleusercontent.com/.*?)""", RegexOptions.Singleline);
            foreach (Match match2 in matchCollection)
            {
                if (match2.Success && (2 == match2.Groups.Count))
                {
                    String url = match2.Groups[1].Value;

                    if (!url.EndsWith("/photo.jpg", StringComparison.InvariantCultureIgnoreCase) && (null == this.listViewImages.FindItemWithText(url)))
                    {
                        this.listViewImages.Items.Add(url);
                    }
                }
            }
        }

        private void buttonGetImageUrls_Click(object sender, EventArgs e)
        {
            try
            {
                this.listViewImages.Items.Clear();
                this.toolStripStatusLabelImageCount.Text = "";
                this.buttonCopyImageUrls.Enabled = false;

                Match match = Regex.Match(this.textBoxAlbumUrl.Text, @"/photos/(\d+)/albums/(\d+)");

                if (!match.Success || (match.Groups.Count != 3))
                {
                    ApplyAlternativeMethod();
                    //                    throw new Exception("This URL does not belong to a Google+ photo album.");
                }
                else
                {
                    String requestUrl = String.Format("https://picasaweb.google.com/data/feed/api/user/{0}/albumid/{1}?v=2&kind=photo&imgmax=d", match.Groups[1].Value, match.Groups[2].Value);

                    String responseXml = "";

                    bool useAlternativeMethod;
                    try
                    {
                        WebClient webClient = new WebClient();
                        responseXml = webClient.DownloadString(requestUrl);

                        useAlternativeMethod = false;
                    }
                    catch (WebException ex)
                    {
                        if (WebExceptionStatus.ProtocolError == ex.Status)
                        {
                            useAlternativeMethod = true;
                        }
                        else
                        {
                            throw ex;
                        }
                    }

                    if (useAlternativeMethod)
                    {
                        ApplyAlternativeMethod();
                    }
                    else
                    {
                        StringReader stringReader = new StringReader(responseXml);
                        XmlTextReader xmlTextReader = new XmlTextReader(stringReader);

                        bool inEntry = false;
                        while (true)
                        {
                            String name = xmlTextReader.Name;

                            if (name.Equals("entry"))
                            {
                                if (XmlNodeType.Element == xmlTextReader.NodeType)
                                {
                                    inEntry = true;
                                }
                                else if (XmlNodeType.EndElement == xmlTextReader.NodeType)
                                {
                                    inEntry = false;
                                }
                            }

                            if (inEntry && (XmlNodeType.Element == xmlTextReader.NodeType))
                            {
                                switch (name)
                                {
                                    case "title":
                                        break;
                                    case "content":
                                        String url = xmlTextReader.GetAttribute("src");

                                        this.listViewImages.Items.Add(url);
                                        break;
                                }
                            }

                            if (!xmlTextReader.Read())
                            {
                                break;
                            }
                        }
                    }
                }

                this.toolStripStatusLabelImageCount.Text = String.Format("{0} image(s)", this.listViewImages.Items.Count);
                this.buttonCopyImageUrls.Enabled = this.listViewImages.Items.Count > 0;
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }

        private void buttonCopyImageUrls_Click(object sender, EventArgs e)
        {
            String text = "";

            foreach (ListViewItem listViewItem in this.listViewImages.Items)
            {
                text += listViewItem.Text + Environment.NewLine;
            }

            Clipboard.SetText(text);
        }
    }
}
