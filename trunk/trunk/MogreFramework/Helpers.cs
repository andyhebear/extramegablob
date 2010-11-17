using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MogreFramework
{
    class Helpers
    {
    }
    public class GUI_helper : Form
    {
        #region GUI object helpers
        //misc
        public static class MyTreeNodeCollection
        {
            public static TreeNodeCollection getNewTNC()
            {
                TreeView tv1 = new TreeView();
                return tv1.Nodes;
            }
        }

        //form
        public delegate void CloseFormDelegate();
        public virtual void CloseForm()
        {
            if (this.InvokeRequired)
            {
                CloseFormDelegate dele = new CloseFormDelegate(CloseForm2);
                this.Invoke(dele);
            }
            else
            {
                CloseForm2();
            }
        }
        private void CloseForm2()
        {
            this.Close();
        }

        //control
        public delegate void SetControlVisibleDelegate(Control Control, Boolean Visible);
        public virtual void setControlVisible(Control Control, Boolean Visible)
        {
            if (this.InvokeRequired)
            {
                SetControlVisibleDelegate dele = new SetControlVisibleDelegate(setControlVisible2);
                this.Invoke(dele, new object[] { Control, Visible });
            }
            else
            {
                setControlVisible2(Control, Visible);
            }
        }
        private void setControlVisible2(Control Control, Boolean Visible)
        {
            Control.Visible = Visible;
        }
        public delegate void SetControlEnabledDelegate(Control Control, Boolean Enabled);
        public virtual void setControlEnabled(Control Control, Boolean Enabled)
        {
            if (this.InvokeRequired)
            {
                SetControlEnabledDelegate dele = new SetControlEnabledDelegate(setControlEnabled2);
                this.Invoke(dele, new object[] { Control, Enabled });
            }
            else
            {
                setControlEnabled2(Control, Enabled);
            }
        }
        private void setControlEnabled2(Control Control, Boolean Enabled)
        {
            Control.Enabled = Enabled;
        }
        public delegate void SetControlTextDelegate(Control Control, String Text);
        public virtual void setControlText(Control Control, String Text)
        {
            if (this.InvokeRequired)
            {
                SetControlTextDelegate dele = new SetControlTextDelegate(setControlText2);
                this.Invoke(dele, new object[] { Control, Text });
            }
            else
            {
                setControlText2(Control, Text);
            }
        }
        private void setControlText2(Control Control, String Text)
        {
            Control.Text = Text;
        }
        public delegate String GetControlTextDelegate(Control Control);
        public virtual String getControlText(Control Control)
        {
            if (this.InvokeRequired)
            {
                GetControlTextDelegate dele = new GetControlTextDelegate(getControlText2);
                return (String)this.Invoke(dele, new object[] { Control });
            }
            else
            {
                return getControlText2(Control);
            }
        }
        private String getControlText2(Control Control)
        {
            return Control.Text;
        }
        public delegate void SetControlTagDelegate(Control Control, Object Text);
        public virtual void SetControlTag(Control Control, Object TagObject)
        {
            if (this.InvokeRequired)
            {
                SetControlTagDelegate dele = new SetControlTagDelegate(SetControlTag2);
                this.Invoke(dele, new object[] { Control, TagObject });
            }
            else
            {
                SetControlTag2(Control, TagObject);
            }
        }
        private void SetControlTag2(Control Control, Object TagObject)
        {
            Control.Tag = TagObject;
        }
        private delegate void setControlBoxEnabledDelegate(bool enabled);
        public virtual void setControlBoxEnabled(bool enabled)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new setControlBoxEnabledDelegate(setControlBoxEnabled2), new object[] { enabled });
            }
            else
            {
                setControlBoxEnabled2(enabled);
            }
        }
        private void setControlBoxEnabled2(bool enabled)
        {
            this.ControlBox = enabled;
        }


        //picturebox
        public delegate void SetPictureBoxImageLocationDelegate(PictureBox Object, String Location);
        public virtual void SetPictureBoxImageLocation(PictureBox Object, String Location)
        {
            if (this.InvokeRequired)
            {
                SetPictureBoxImageLocationDelegate dele = new SetPictureBoxImageLocationDelegate(SetPictureBoxImageLocation2);
                this.Invoke(dele, new object[] { Object, Location });
            }
            else
            {
                SetPictureBoxImageLocation2(Object, Location);
            }
        }
        private void SetPictureBoxImageLocation2(PictureBox Object, String Location)
        {
            Object.ImageLocation = Location;
        }
        public delegate void SetPictureBoxImageDelegate(PictureBox Object, Image Image);
        public virtual void SetPictureBoxImage(PictureBox Object, Image Image)
        {
            if (this.InvokeRequired)
            {
                SetPictureBoxImageDelegate dele = new SetPictureBoxImageDelegate(SetPictureBoxImage2);
                this.Invoke(dele, new object[] { Object, Image });
            }
            else
            {
                SetPictureBoxImage2(Object, Image);
            }
        }
        private void SetPictureBoxImage2(PictureBox Object, Image Image)
        {
            Object.Image = Image;
        }

        //status bar panel
        public delegate void SetStatusBarPanelTextDelegate(StatusBarPanel Object, String Text);
        public virtual void SetStatusBarPanelText(StatusBarPanel Object, String Text)
        {
            if (this.InvokeRequired)
            {
                SetStatusBarPanelTextDelegate dele = new SetStatusBarPanelTextDelegate(SetStatusBarPanelText2);
                this.Invoke(dele, new object[] { Object, Text });
            }
            else
            {
                SetStatusBarPanelText2(Object, Text);
            }
        }
        private void SetStatusBarPanelText2(StatusBarPanel Object, String Text)
        {
            Object.Text = Text;
        }

        //progress bar
        public delegate void InvokeProgressBarUpdateDelegate(ProgressBar Object);
        public virtual void InvokeProgressBarUpdate(ProgressBar Object)
        {
            if (this.InvokeRequired)
            {
                InvokeProgressBarUpdateDelegate dele = new InvokeProgressBarUpdateDelegate(InvokeProgressBarUpdate2);
                this.Invoke(dele, new object[] { Object });
            }
            else
            {
                InvokeProgressBarUpdate2(Object);
            }
        }
        private void InvokeProgressBarUpdate2(ProgressBar Object)
        {
            Object.Update();
        }
        public delegate void SetProgressBarValueDelegate(ProgressBar Object, Int32 Value);
        public virtual void SetProgressBarValue(ProgressBar Object, Int32 Value)
        {
            if (this.InvokeRequired)
            {
                SetProgressBarValueDelegate dele = new SetProgressBarValueDelegate(SetProgressBarValue2);
                this.Invoke(dele, new object[] { Object, Value });
            }
            else
            {
                SetProgressBarValue2(Object, Value);
            }
        }
        private void SetProgressBarValue2(ProgressBar Object, Int32 Value)
        {
            if (Value >= 0 && Value >= Object.Minimum && Value <= Object.Maximum)
                Object.Value = Value;
        }
        public delegate void SetProgressBarMaximumDelegate(ProgressBar Object, Int32 Maximum);
        public virtual void SetProgressBarMaximum(ProgressBar Object, Int32 Maximum)
        {
            if (this.InvokeRequired)
            {
                SetProgressBarMaximumDelegate dele = new SetProgressBarMaximumDelegate(SetProgressBarMaximum2);
                this.Invoke(dele, new object[] { Object, Maximum });
            }
            else
            {
                SetProgressBarMaximum2(Object, Maximum);
            }
        }
        private void SetProgressBarMaximum2(ProgressBar Object, Int32 Maximum)
        {
            if (Maximum > 0 && Maximum > Object.Minimum)
                Object.Maximum = Maximum;
        }
        public delegate Int32 GetProgressBarValueDelegate(ProgressBar Object);
        public virtual Int32 GetProgressBarValue(ProgressBar Object)
        {
            if (this.InvokeRequired)
            {
                GetProgressBarValueDelegate dele = new GetProgressBarValueDelegate(GetProgressBarValue2);
                return (Int32)this.Invoke(dele, new object[] { Object });
            }
            else
            {
                return GetProgressBarValue2(Object);
            }
        }
        private Int32 GetProgressBarValue2(ProgressBar Object)
        {
            return Object.Value;
        }
        public delegate Int32 GetProgressBarMaximumDelegate(ProgressBar Object);
        public virtual Int32 GetProgressBarMaximum(ProgressBar Object)
        {
            if (this.InvokeRequired)
            {
                GetProgressBarMaximumDelegate dele = new GetProgressBarMaximumDelegate(GetProgressBarMaximum2);
                return (Int32)this.Invoke(dele, new object[] { Object });
            }
            else
            {
                return GetProgressBarMaximum2(Object);
            }
        }
        private Int32 GetProgressBarMaximum2(ProgressBar Object)
        {
            return Object.Maximum;
        }

        //textbox
        public delegate void TextBoxClearDelegate(TextBox Object);
        public virtual void TextBoxClear(TextBox Object)
        {
            if (this.InvokeRequired)
            {
                TextBoxClearDelegate dele = new TextBoxClearDelegate(TextBoxClear2);
                this.Invoke(dele, new object[] { Object });
            }
            else
            {
                TextBoxClear2(Object);
            }
        }
        private void TextBoxClear2(TextBox Object)
        {
            Object.Clear();
        }
        public delegate void TextBoxAppendTextDelegate(TextBox Object, String Text);
        public virtual void TextBoxAppendText(TextBox Object, String Text)
        {
            if (this.InvokeRequired)
            {
                TextBoxAppendTextDelegate dele = new TextBoxAppendTextDelegate(TextBoxAppendText2);
                try { this.Invoke(dele, new object[] { Object, Text }); }
                catch { }
            }
            else
            {
                TextBoxAppendText2(Object, Text);
            }
        }
        private void TextBoxAppendText2(TextBox Object, String Text)
        {
            try { Object.AppendText(Text); }
            catch { }
        }

        //label
        public delegate void LabelClearDelegate(Label Object);
        public virtual void LabelClear(Label Object)
        {
            if (this.InvokeRequired)
            {
                LabelClearDelegate dele = new LabelClearDelegate(LabelClear2);
                this.Invoke(dele, new object[] { Object });
            }
            else
            {
                LabelClear2(Object);
            }
        }
        private void LabelClear2(Label Object)
        {
            Object.Text = "";
        }
        public delegate void LabelAppendTextDelegate(Label Object, String Text);
        public virtual void LabelSetText(Label Object, String Text)
        {
            if (this.InvokeRequired)
            {
                LabelAppendTextDelegate dele = new LabelAppendTextDelegate(LabelSetText2);
                this.Invoke(dele, new object[] { Object, Text });
            }
            else
            {
                LabelSetText2(Object, Text);
            }
        }
        private void LabelSetText2(Label Object, String Text)
        {
            Object.Text = Text;
        }

        //listbox
        public delegate void ListBoxItemAddDelegate(ListBox Object, Object Item);
        public virtual void ListBoxItemAdd(ListBox Object, Object Item)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    ListBoxItemAddDelegate dele = new ListBoxItemAddDelegate(ListBoxItemAdd2);
                    this.Invoke(dele, new object[] { Object, Item });
                }
                else
                {
                    ListBoxItemAdd2(Object, Item);
                }
            }
            catch { }
        }
        private void ListBoxItemAdd2(ListBox Object, Object Item)
        {
            Object.Items.Add(Item);
        }
        public delegate void ListBoxItemAddAndScrollDownDelegate(ListBox Object, Object Item);
        public virtual void ListBoxItemAddAndScrollDown(ListBox Object, Object Item)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    ListBoxItemAddAndScrollDownDelegate dele = new ListBoxItemAddAndScrollDownDelegate(ListBoxItemAddAndScrollDown2);
                    this.Invoke(dele, new object[] { Object, Item });
                }
                else
                {
                    ListBoxItemAddAndScrollDown2(Object, Item);
                }
            }
            catch { }
        }
        private void ListBoxItemAddAndScrollDown2(ListBox Object, Object Item)
        {
            Object.Items.Add(Item);
            ListBoxScrollDown2(Object);
        }


        public delegate void ListBoxScrollDownDelegate(ListBox Object);
        public virtual void ListBoxScrollDown(ListBox Object)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    ListBoxScrollDownDelegate dele = new ListBoxScrollDownDelegate(ListBoxScrollDown2);
                    this.Invoke(dele, new object[] { Object });
                }
                else
                {
                    ListBoxScrollDown2(Object);
                }
            }
            catch { }
        }
        private void ListBoxScrollDown2(ListBox Object)
        {
            int itemsPerPage = (int)(Object.Height / Object.ItemHeight);
            Object.TopIndex = Object.Items.Count - itemsPerPage + 2;

            //Object.SelectedIndex = Object.Items.Count - 1;
            //Object.SelectedIndex = -1;
        }

        public delegate void SetListBoxSelectedIndexDelegate(ListBox Object, Int32 Value);
        public virtual void SetListBoxSelectedIndex(ListBox Object, Int32 Value)
        {
            if (this.InvokeRequired)
            {
                SetListBoxSelectedIndexDelegate dele = new SetListBoxSelectedIndexDelegate(SetListBoxSelectedIndex2);
                this.Invoke(dele, new object[] { Object, Value });
            }
            else
            {
                SetListBoxSelectedIndex2(Object, Value);
            }
        }
        private void SetListBoxSelectedIndex2(ListBox Object, Int32 Value)
        {
            Object.SelectedIndex = Value;
        }

        //treeview
        public delegate void TreeViewClearDelegate(TreeView Object);
        public virtual void TreeViewClear(TreeView Object)
        {
            if (this.InvokeRequired)
            {
                TreeViewClearDelegate dele = new TreeViewClearDelegate(TreeViewClear2);
                this.Invoke(dele, new object[] { Object });
            }
            else
            {
                TreeViewClear2(Object);
            }
        }
        private void TreeViewClear2(TreeView Object)
        {
            Object.Nodes.Clear();
        }

        //listview
        public delegate void ListViewClearDelegate(ListView Object);
        public virtual void ListViewClear(ListView Object)
        {
            if (this.InvokeRequired)
            {
                ListViewClearDelegate dele = new ListViewClearDelegate(ListViewClear2);
                this.Invoke(dele, new object[] { Object });
            }
            else
            {
                ListViewClear2(Object);
            }
        }
        private void ListViewClear2(ListView Object)
        {
            Object.Items.Clear();
            Object.Columns.Clear();
        }
        public delegate void ListViewItemAddDelegate(ListView Object, ListViewItem Item);
        public virtual void ListViewItemAdd(ListView Object, ListViewItem Item)
        {
            if (this.InvokeRequired)
            {
                ListViewItemAddDelegate dele = new ListViewItemAddDelegate(ListViewItemAdd2);
                this.Invoke(dele, new object[] { Object, Item });
            }
            else
            {
                ListViewItemAdd2(Object, Item);
            }
        }
        private void ListViewItemAdd2(ListView Object, ListViewItem Item)
        {
            Object.Items.Add(Item);
        }

        //listbox
        public delegate void ListBoxClearDelegate(ListBox Object);
        public virtual void ListBoxClear(ListBox Object)
        {
            if (this.InvokeRequired)
            {
                ListBoxClearDelegate dele = new ListBoxClearDelegate(ListBoxClear2);
                this.Invoke(dele, new object[] { Object });
            }
            else
            {
                ListBoxClear2(Object);
            }
        }
        private void ListBoxClear2(ListBox Object)
        {
            Object.Items.Clear();
        }

        //combobox
        public delegate void SetComboBoxSelectedIndexDelegate(ComboBox Object, Int32 Value);
        public virtual void SetComboBoxSelectedIndex(ComboBox Object, Int32 Value)
        {
            if (this.InvokeRequired)
            {
                SetComboBoxSelectedIndexDelegate dele = new SetComboBoxSelectedIndexDelegate(SetComboBoxSelectedIndex2);
                this.Invoke(dele, new object[] { Object, Value });
            }
            else
            {
                SetComboBoxSelectedIndex2(Object, Value);
            }
        }
        private void SetComboBoxSelectedIndex2(ComboBox Object, Int32 Value)
        {
            Object.SelectedIndex = Value;
        }
        public delegate void SetComboBoxSelectedItemDelegate(ComboBox Object, Object Value);
        public virtual void SetComboBoxSelectedItem(ComboBox Object, Object Value)
        {
            if (this.InvokeRequired)
            {
                SetComboBoxSelectedItemDelegate dele = new SetComboBoxSelectedItemDelegate(SetComboBoxSelectedItem2);
                this.Invoke(dele, new object[] { Object, Value });
            }
            else
            {
                SetComboBoxSelectedItem2(Object, Value);
            }
        }
        private void SetComboBoxSelectedItem2(ComboBox Object, Object Value)
        {
            Object.SelectedItem = Value;
        }

        //tab control
        public delegate void TabControlSwitchTabDelegate(TabControl Object, Int32 Value);
        public virtual void TabControlSwitchTab(TabControl Object, Int32 Value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new TabControlSwitchTabDelegate(TabControlSwitchTab2), new object[] { Object, Value });
            }
            else
            {
                TabControlSwitchTab2(Object, Value);
            }
        }
        private void TabControlSwitchTab2(TabControl Object, Int32 Value)
        {
            Object.SelectedIndex = Value;
        }


        public virtual System.Windows.Forms.Control getControlRecursive(System.Windows.Forms.Control c, String ControlName)
        {
            if (c.HasChildren)
            {
                foreach (System.Windows.Forms.Control C in c.Controls)
                {
                    if (C.Name == ControlName)
                    {
                        return C;
                    }
                    else
                    {
                        System.Windows.Forms.Control d = getControlRecursive(C, ControlName);
                        if (!object.Equals(null, d))
                        {
                            return d;
                        }
                    }
                }
            }
            return null;
        }
        #endregion
    }
}
