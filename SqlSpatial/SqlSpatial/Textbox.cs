namespace SqlSpatial
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class Textbox : TextBox
    {
        private IContainer components;

        public Textbox()
        {
            base.AcceptsReturn = true;
            this.Font = new Font("Courier New", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.Multiline = true;
            base.ScrollBars = ScrollBars.Vertical;
            this.MaxLength = 0x40000;
            this.InitializeComponent();
        }

        public void Delete()
        {
            int selectionLength = this.SelectionLength;
            int selectionStart = base.SelectionStart;
            string text = this.Text;
            this.Text = text.Substring(0, selectionStart) + text.Substring(selectionStart + selectionLength);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void IndentDecrease(string prefix)
        {
            this.prefixSelection(prefix, false, true);
        }

        public void IndentIncrease(string prefix, bool allowRepeat)
        {
            this.prefixSelection(prefix, true, allowRepeat);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        private void prefixSelection(string prefix, bool add, bool allowRepeat)
        {
            int selectionLength = this.SelectionLength;
            int selectionStart = base.SelectionStart;
            int lineFromCharIndex = this.GetLineFromCharIndex(selectionStart);
            int num4 = this.GetLineFromCharIndex(selectionStart + selectionLength);
            string text = this.Text;
            string[] lines = base.Lines;
            bool flag = false;
            int length = prefix.Length;
            for (int i = lineFromCharIndex; (i <= num4) && (i < lines.Length); i++)
            {
                if ((allowRepeat && add) || (lines[i].StartsWith(prefix) != add))
                {
                    if (add)
                    {
                        lines[i] = prefix + lines[i];
                    }
                    else
                    {
                        lines[i] = lines[i].Substring(length);
                    }
                    if ((i == lineFromCharIndex) && (base.GetFirstCharIndexFromLine(i) < selectionStart))
                    {
                        selectionStart += length * (add ? 1 : -1);
                    }
                    else if (selectionLength > 0)
                    {
                        selectionLength += length * (add ? 1 : -1);
                    }
                    flag = true;
                }
            }
            if (flag)
            {
                base.Lines = lines;
                base.SelectionStart = selectionStart;
                if (selectionLength > 0)
                {
                    this.SelectionLength = selectionLength;
                }
                this.OnTextChanged(new EventArgs());
            }
        }
    }
}

