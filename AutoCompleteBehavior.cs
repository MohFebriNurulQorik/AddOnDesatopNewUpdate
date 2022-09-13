using System;
using System.Linq;
using System.Windows.Forms;

namespace ScaleAddon
{
    public class AutoCompleteBehavior
    {
        private readonly ComboBox comboBox;
        private string previousSearchterm;

        private object[] originalList;

        public AutoCompleteBehavior(ComboBox comboBox)
        {
            this.comboBox = comboBox;
            this.comboBox.AutoCompleteMode = AutoCompleteMode.Suggest; // crucial otherwise exceptions occur when the user types in text which is not found in the autocompletion list
            this.comboBox.TextChanged += this.OnTextChanged;
            this.comboBox.KeyPress += this.OnKeyPress;
            this.comboBox.SelectionChangeCommitted += this.OnSelectionChangeCommitted;
        }

        private void OnSelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.comboBox.SelectedItem == null)
            {
                return;
            }

            var sel = this.comboBox.SelectedItem;
            this.ResetCompletionList();
            this.comboBox.SelectedItem = sel;
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.comboBox.Text) || !this.comboBox.Visible || !this.comboBox.Enabled)
            {
                return;
            }

            this.ResetCompletionList();
        }

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r' || e.KeyChar == '\n')
            {
                e.Handled = true;
                if (this.comboBox.SelectedIndex == -1 && this.comboBox.Items.Count > 0
                    && this.comboBox.Items[0].ToString().ToLowerInvariant().StartsWith(this.comboBox.Text.ToLowerInvariant()))
                {
                    this.comboBox.Text = this.comboBox.Items[0].ToString();
                }

                this.comboBox.DroppedDown = false;

                // Guardclause when detecting any enter keypresses to avoid a glitch which was selecting an item by means of down arrow key followed by enter to wipe out the text within
                return;
            }

            // Its crucial that we use begininvoke because we need the changes to sink into the textfield  Omitting begininvoke would cause the searchterm to lag behind by one character  That is the last character that got typed in
            this.comboBox.BeginInvoke(new Action(this.ReevaluateCompletionList));
        }

        private void ResetCompletionList()
        {
            this.previousSearchterm = null;
            try
            {
                this.comboBox.SuspendLayout();

                if (this.originalList == null)
                {
                    this.originalList = this.comboBox.Items.Cast<object>().ToArray();
                }

                if (this.comboBox.Items.Count == this.originalList.Length)
                {
                    return;
                }

                while (this.comboBox.Items.Count > 0)
                {
                    this.comboBox.Items.RemoveAt(0);
                }

                this.comboBox.Items.AddRange(this.originalList);
            }
            finally
            {
                this.comboBox.ResumeLayout(true);
            }
        }

        private void ReevaluateCompletionList()
        {
            var currentSearchterm = this.comboBox.Text.ToLowerInvariant();
            if (currentSearchterm == this.previousSearchterm)
            {
                return;
            }

            this.previousSearchterm = currentSearchterm;
            try
            {
                this.comboBox.SuspendLayout();

                if (this.originalList == null)
                {
                    this.originalList = this.comboBox.Items.Cast<object>().ToArray(); // backup original list
                }

                object[] newList;
                if (string.IsNullOrEmpty(currentSearchterm))
                {
                    if (this.comboBox.Items.Count == this.originalList.Length)
                    {
                        return;
                    }

                    newList = this.originalList;
                }
                else
                {
                    newList = this.originalList.Where(x => x.ToString().ToLowerInvariant().Contains(currentSearchterm)).ToArray();
                }

                try
                {
                    // clear list by loop through it otherwise the cursor would move to the beginning of the textbox
                    while (this.comboBox.Items.Count > 0)
                    {
                        this.comboBox.Items.RemoveAt(0);
                    }
                }
                catch
                {
                    try
                    {
                        this.comboBox.Items.Clear();
                    }
                    catch (Exception ex)
                    {
                        //Debug.WriteLine(ex.Message);
                    }
                }

                this.comboBox.Items.AddRange(newList.ToArray()); // reset list
            }
            finally
            {
                if (currentSearchterm.Length >= 1 && !this.comboBox.DroppedDown)
                {
                    this.comboBox.DroppedDown = true; // if the current searchterm is empty we leave the dropdown list to whatever state it already had
                    Cursor.Current = Cursors.Default; // workaround for the fact the cursor disappears due to droppeddown=true  This is a known bu.g plaguing combobox which microsoft denies to fix for years now
                    this.comboBox.Text = currentSearchterm; // Another workaround for a glitch which causes all text to be selected when there is a matching entry which starts with the exact text being typed in
                    this.comboBox.Select(currentSearchterm.Length, 0);
                }

                this.comboBox.ResumeLayout(true);
            }
        }
    }
}