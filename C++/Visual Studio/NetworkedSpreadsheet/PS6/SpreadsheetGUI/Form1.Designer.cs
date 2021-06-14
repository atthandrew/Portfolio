namespace SpreadsheetGUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ContentsBox = new System.Windows.Forms.TextBox();
            this.SetButton = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.File = new System.Windows.Forms.ToolStripDropDownButton();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Save = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ThemeToolStripButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.lightModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.darkModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenuButton = new System.Windows.Forms.ToolStripButton();
            this.EditDropdown = new System.Windows.Forms.ToolStripDropDownButton();
            this.UndoButton = new System.Windows.Forms.ToolStripMenuItem();
            this.RevertButton = new System.Windows.Forms.ToolStripMenuItem();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.ValueBox = new System.Windows.Forms.TextBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this.ContentsLabel = new System.Windows.Forms.Label();
            this.Value = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.UsernameBox = new System.Windows.Forms.TextBox();
            this.PasswordBox = new System.Windows.Forms.TextBox();
            this.ServerBox = new System.Windows.Forms.TextBox();
            this.FilenameBox = new System.Windows.Forms.TextBox();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.ssListBox = new System.Windows.Forms.ListBox();
            this.openButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.userLabel = new System.Windows.Forms.Label();
            this.passLabel = new System.Windows.Forms.Label();
            this.fileLabel = new System.Windows.Forms.Label();
            this.existingLabel = new System.Windows.Forms.Label();
            this.spreadsheetPanel1 = new SS.CustomSpreadsheetPanel();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ContentsBox
            // 
            this.ContentsBox.Location = new System.Drawing.Point(314, 57);
            this.ContentsBox.Name = "ContentsBox";
            this.ContentsBox.Size = new System.Drawing.Size(335, 20);
            this.ContentsBox.TabIndex = 1;
            this.ContentsBox.Visible = false;
            this.ContentsBox.TextChanged += new System.EventHandler(this.ContentsBox_TextChanged);
            // 
            // SetButton
            // 
            this.SetButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SetButton.Location = new System.Drawing.Point(656, 57);
            this.SetButton.Name = "SetButton";
            this.SetButton.Size = new System.Drawing.Size(136, 37);
            this.SetButton.TabIndex = 2;
            this.SetButton.Text = "Set";
            this.SetButton.UseVisualStyleBackColor = true;
            this.SetButton.Visible = false;
            this.SetButton.Click += new System.EventHandler(this.SetButton_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File,
            this.ThemeToolStripButton,
            this.HelpMenuButton,
            this.EditDropdown});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip1.Size = new System.Drawing.Size(821, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // File
            // 
            this.File.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.Save,
            this.closeToolStripMenuItem});
            this.File.Image = ((System.Drawing.Image)(resources.GetObject("File.Image")));
            this.File.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.File.Name = "File";
            this.File.ShowDropDownArrow = false;
            this.File.Size = new System.Drawing.Size(29, 22);
            this.File.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // Save
            // 
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(107, 22);
            this.Save.Text = "Save...";
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // ThemeToolStripButton
            // 
            this.ThemeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ThemeToolStripButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lightModeToolStripMenuItem,
            this.darkModeToolStripMenuItem});
            this.ThemeToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("ThemeToolStripButton.Image")));
            this.ThemeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ThemeToolStripButton.Name = "ThemeToolStripButton";
            this.ThemeToolStripButton.ShowDropDownArrow = false;
            this.ThemeToolStripButton.Size = new System.Drawing.Size(48, 22);
            this.ThemeToolStripButton.Text = "Theme";
            // 
            // lightModeToolStripMenuItem
            // 
            this.lightModeToolStripMenuItem.Name = "lightModeToolStripMenuItem";
            this.lightModeToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.lightModeToolStripMenuItem.Text = "Light Mode";
            this.lightModeToolStripMenuItem.Click += new System.EventHandler(this.lightModeToolStripMenuItem_Click);
            // 
            // darkModeToolStripMenuItem
            // 
            this.darkModeToolStripMenuItem.Name = "darkModeToolStripMenuItem";
            this.darkModeToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.darkModeToolStripMenuItem.Text = "Dark Mode";
            this.darkModeToolStripMenuItem.Click += new System.EventHandler(this.darkModeToolStripMenuItem_Click);
            // 
            // HelpMenuButton
            // 
            this.HelpMenuButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.HelpMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("HelpMenuButton.Image")));
            this.HelpMenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.HelpMenuButton.Name = "HelpMenuButton";
            this.HelpMenuButton.Size = new System.Drawing.Size(36, 22);
            this.HelpMenuButton.Text = "Help";
            this.HelpMenuButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // EditDropdown
            // 
            this.EditDropdown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.EditDropdown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UndoButton,
            this.RevertButton});
            this.EditDropdown.Image = ((System.Drawing.Image)(resources.GetObject("EditDropdown.Image")));
            this.EditDropdown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditDropdown.Name = "EditDropdown";
            this.EditDropdown.ShowDropDownArrow = false;
            this.EditDropdown.Size = new System.Drawing.Size(31, 22);
            this.EditDropdown.Text = "Edit";
            this.EditDropdown.Visible = false;
            // 
            // UndoButton
            // 
            this.UndoButton.Name = "UndoButton";
            this.UndoButton.Size = new System.Drawing.Size(180, 22);
            this.UndoButton.Text = "Undo";
            this.UndoButton.Click += new System.EventHandler(this.UndoButton_Click);
            // 
            // RevertButton
            // 
            this.RevertButton.Name = "RevertButton";
            this.RevertButton.Size = new System.Drawing.Size(180, 22);
            this.RevertButton.Text = "Revert";
            this.RevertButton.Click += new System.EventHandler(this.RevertButton_Click);
            // 
            // NameBox
            // 
            this.NameBox.Location = new System.Drawing.Point(8, 57);
            this.NameBox.Name = "NameBox";
            this.NameBox.ReadOnly = true;
            this.NameBox.Size = new System.Drawing.Size(144, 20);
            this.NameBox.TabIndex = 4;
            this.NameBox.Visible = false;
            // 
            // ValueBox
            // 
            this.ValueBox.Location = new System.Drawing.Point(164, 58);
            this.ValueBox.Name = "ValueBox";
            this.ValueBox.ReadOnly = true;
            this.ValueBox.Size = new System.Drawing.Size(144, 20);
            this.ValueBox.TabIndex = 5;
            this.ValueBox.Visible = false;
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameLabel.Location = new System.Drawing.Point(46, 80);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(49, 17);
            this.NameLabel.TabIndex = 6;
            this.NameLabel.Text = "Name";
            this.NameLabel.Visible = false;
            // 
            // ContentsLabel
            // 
            this.ContentsLabel.AutoSize = true;
            this.ContentsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.ContentsLabel.Location = new System.Drawing.Point(448, 79);
            this.ContentsLabel.Name = "ContentsLabel";
            this.ContentsLabel.Size = new System.Drawing.Size(82, 20);
            this.ContentsLabel.TabIndex = 7;
            this.ContentsLabel.Text = "Contents";
            this.ContentsLabel.Visible = false;
            // 
            // Value
            // 
            this.Value.AutoSize = true;
            this.Value.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Value.Location = new System.Drawing.Point(188, 81);
            this.Value.Name = "Value";
            this.Value.Size = new System.Drawing.Size(49, 17);
            this.Value.TabIndex = 8;
            this.Value.Text = "Value";
            this.Value.Visible = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // UsernameBox
            // 
            this.UsernameBox.Location = new System.Drawing.Point(405, 27);
            this.UsernameBox.Margin = new System.Windows.Forms.Padding(2);
            this.UsernameBox.Name = "UsernameBox";
            this.UsernameBox.Size = new System.Drawing.Size(68, 20);
            this.UsernameBox.TabIndex = 9;
            this.UsernameBox.Visible = false;
            // 
            // PasswordBox
            // 
            this.PasswordBox.Location = new System.Drawing.Point(482, 26);
            this.PasswordBox.Margin = new System.Windows.Forms.Padding(2);
            this.PasswordBox.Name = "PasswordBox";
            this.PasswordBox.Size = new System.Drawing.Size(68, 20);
            this.PasswordBox.TabIndex = 10;
            this.PasswordBox.Visible = false;
            // 
            // ServerBox
            // 
            this.ServerBox.Location = new System.Drawing.Point(291, 27);
            this.ServerBox.Margin = new System.Windows.Forms.Padding(2);
            this.ServerBox.Name = "ServerBox";
            this.ServerBox.Size = new System.Drawing.Size(99, 20);
            this.ServerBox.TabIndex = 11;
            // 
            // FilenameBox
            // 
            this.FilenameBox.Location = new System.Drawing.Point(561, 27);
            this.FilenameBox.Margin = new System.Windows.Forms.Padding(2);
            this.FilenameBox.Name = "FilenameBox";
            this.FilenameBox.Size = new System.Drawing.Size(68, 20);
            this.FilenameBox.TabIndex = 12;
            this.FilenameBox.Visible = false;
            // 
            // ConnectButton
            // 
            this.ConnectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectButton.Location = new System.Drawing.Point(403, 21);
            this.ConnectButton.Margin = new System.Windows.Forms.Padding(2);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(70, 31);
            this.ConnectButton.TabIndex = 13;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // ssListBox
            // 
            this.ssListBox.FormattingEnabled = true;
            this.ssListBox.Location = new System.Drawing.Point(267, 25);
            this.ssListBox.Margin = new System.Windows.Forms.Padding(2);
            this.ssListBox.Name = "ssListBox";
            this.ssListBox.Size = new System.Drawing.Size(123, 69);
            this.ssListBox.TabIndex = 14;
            this.ssListBox.Visible = false;
            // 
            // openButton
            // 
            this.openButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openButton.Location = new System.Drawing.Point(647, 21);
            this.openButton.Margin = new System.Windows.Forms.Padding(2);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(66, 31);
            this.openButton.TabIndex = 15;
            this.openButton.Text = "Open";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Visible = false;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(207, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Server Address";
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(402, 48);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(55, 13);
            this.userLabel.TabIndex = 17;
            this.userLabel.Text = "Username";
            this.userLabel.Visible = false;
            // 
            // passLabel
            // 
            this.passLabel.AutoSize = true;
            this.passLabel.Location = new System.Drawing.Point(497, 48);
            this.passLabel.Name = "passLabel";
            this.passLabel.Size = new System.Drawing.Size(53, 13);
            this.passLabel.TabIndex = 18;
            this.passLabel.Text = "Password";
            this.passLabel.Visible = false;
            // 
            // fileLabel
            // 
            this.fileLabel.AutoSize = true;
            this.fileLabel.Location = new System.Drawing.Point(558, 49);
            this.fileLabel.Name = "fileLabel";
            this.fileLabel.Size = new System.Drawing.Size(49, 13);
            this.fileLabel.TabIndex = 19;
            this.fileLabel.Text = "Filename";
            this.fileLabel.Visible = false;
            // 
            // existingLabel
            // 
            this.existingLabel.AutoSize = true;
            this.existingLabel.Location = new System.Drawing.Point(267, 100);
            this.existingLabel.Name = "existingLabel";
            this.existingLabel.Size = new System.Drawing.Size(111, 13);
            this.existingLabel.TabIndex = 20;
            this.existingLabel.Text = "Existing Spreadsheets";
            this.existingLabel.Visible = false;
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel1.Location = new System.Drawing.Point(0, 102);
            this.spreadsheetPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(820, 425);
            this.spreadsheetPanel1.TabIndex = 0;
            this.spreadsheetPanel1.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 527);
            this.Controls.Add(this.existingLabel);
            this.Controls.Add(this.fileLabel);
            this.Controls.Add(this.passLabel);
            this.Controls.Add(this.userLabel);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.ssListBox);
            this.Controls.Add(this.FilenameBox);
            this.Controls.Add(this.UsernameBox);
            this.Controls.Add(this.Value);
            this.Controls.Add(this.ContentsLabel);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.ValueBox);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.SetButton);
            this.Controls.Add(this.ContentsBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PasswordBox);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Controls.Add(this.ServerBox);
            this.Controls.Add(this.NameBox);
            this.Controls.Add(this.ConnectButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SS.CustomSpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.TextBox ContentsBox;
        private System.Windows.Forms.Button SetButton;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton File;
        private System.Windows.Forms.ToolStripMenuItem Save;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.TextBox NameBox;
        private System.Windows.Forms.TextBox ValueBox;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Label ContentsLabel;
        private System.Windows.Forms.Label Value;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripDropDownButton ThemeToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem lightModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem darkModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton HelpMenuButton;
        private System.Windows.Forms.TextBox UsernameBox;
        private System.Windows.Forms.TextBox PasswordBox;
        private System.Windows.Forms.TextBox ServerBox;
        private System.Windows.Forms.TextBox FilenameBox;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.ListBox ssListBox;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.ToolStripDropDownButton EditDropdown;
        private System.Windows.Forms.ToolStripMenuItem UndoButton;
        private System.Windows.Forms.ToolStripMenuItem RevertButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label userLabel;
        private System.Windows.Forms.Label passLabel;
        private System.Windows.Forms.Label fileLabel;
        private System.Windows.Forms.Label existingLabel;
    }
}

