/*
 * Source Code From: https://archive.codeplex.com/?p=xmlrichtextbox
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CustomXmlViewer
{
    public partial class ucXmlRichTextBox : RichTextBox
    {
        #region Constructor
        public ucXmlRichTextBox()
        {
            InitializeComponent();

            this.Font = new System.Drawing.Font("Consolas", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            miCopyRtf.Click += new EventHandler(miCopyRtf_Click);
            miCopyText.Click += new EventHandler(miCopyText_Click);
            miSelectAll.Click += new EventHandler(miSelectAll_Click);
        }
        #endregion Constructor

        #region Properties
        private string xml = "";
        public string Xml
        {
            get { return xml; }
            set 
            {
                this.Text = "";
                xml = value;
                SetXml(xml); 
            }
        }
        #endregion Properties

        #region Private Methods
        private void SetXml(string s)
        {
            if (String.IsNullOrEmpty(s)) return;

            XDocument xdoc = XDocument.Parse(s);

            string formattedText = xdoc.ToString().Trim();

            if (String.IsNullOrEmpty(formattedText)) return;

            XmlStateMachine machine = new XmlStateMachine();
            
            if (s.StartsWith("<?"))
            {
                string xmlDeclaration = machine.GetXmlDeclaration(s);
                if(xmlDeclaration != String.Empty) formattedText =  xmlDeclaration + Environment.NewLine + formattedText;
            }

            int location = 0;
            int failCount = 0;
            int tokenTryCount = 0;
            XmlTokenType ttype = XmlTokenType.Unknown;
            while (location < formattedText.Length)
            {
                string token = machine.GetNextToken(formattedText, location, out ttype);
                Color color = machine.GetTokenColor(ttype);
                this.AppendText(token, color);
                location += token.Length;
                tokenTryCount++;

                // Check for ongoing failure
                if (token.Length == 0) failCount++;
                if (failCount > 10 || tokenTryCount > formattedText.Length)
                {
                    string theRestOfIt = formattedText.Substring(location, formattedText.Length - location);
                    //this.AppendText(Environment.NewLine + Environment.NewLine + theRestOfIt); // DEBUG
                    this.AppendText(theRestOfIt);
                    break;
                }
            }
        }
        #endregion Private Methods

        #region Context Menu
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(this.SelectedText))
            {
                miCopyText.Enabled = false;
                miCopyRtf.Enabled = false;
            }
            else
            {
                miCopyText.Enabled = true;
                miCopyRtf.Enabled = true;
            }
        }

        void miCopyText_Click(object sender, EventArgs e)
        {
            string s = this.SelectedText;
            try
            {
                XDocument doc = XDocument.Parse(s);
                s = doc.ToString();
            }
            catch { }
            Clipboard.SetText(s);
        }

        void miCopyRtf_Click(object sender, EventArgs e)
        {
            DataObject dto = new DataObject();
            dto.SetText(this.SelectedRtf, TextDataFormat.Rtf);
            dto.SetText(this.SelectedText, TextDataFormat.UnicodeText);
            Clipboard.Clear();
            Clipboard.SetDataObject(dto);
        }

        private void miSelectAll_Click(object sender, EventArgs e)
        {
            this.SelectAll();
        }
        #endregion Context Menu
    }

    #region Extension Methods
    static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
    #endregion Extension Methods
}
