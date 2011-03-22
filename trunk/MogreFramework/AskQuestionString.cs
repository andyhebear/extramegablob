using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MogreFramework
{
    public partial class AskQuestionString : GUI_helper
    {
        public static string md5(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            return s.ToString();
        }
        public AskQuestionString(int secondsTimeout, string question, bool optionalAnswer, bool requiredAnswer)
        {
            InitializeComponent();
            tbQuestion.Text = question;
            this.requiredAnswer = requiredAnswer;
            this.optionalAnswer = optionalAnswer;
            if (!requiredAnswer && !optionalAnswer)
                tbAnswer.Visible = false;
            this.secondsTimeout = secondsTimeout;
        }
        private bool requiredAnswer = false;
        private bool optionalAnswer = false;
        private int secondsTimeout = -1;
        private void tbAnswer_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && e.Control)
            {
                this.tbAnswer.Text += Environment.NewLine;
            }
            else if (e.KeyCode == Keys.Enter && !e.Control)
            {
                if (Commit()) this.Close();
            }
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (Commit()) this.Close();
        }
        private string grabInput()
        {
            string s = this.tbAnswer.Text.Trim();
            if (s == "") return "";
            TextBoxClear(tbAnswer);
            return s;
        }
        public delegate void replyInputDelegate(string questionMD5, string text);
        public event replyInputDelegate onReplyInput;
        private void replyInput(string questionMD5, string text)
        {
            if (!object.Equals(null, this.onReplyInput))
            {
                onReplyInput(questionMD5, text);
            }
        }
        private bool Commit()
        {
            string s = grabInput();
            if (s == "" && this.requiredAnswer)
                return false;
            replyInput(AskQuestionString.md5(tbQuestion.Text), s);
            return true;
        }
    }
}
