namespace ServerManager
{
    partial class Main
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
            this.label1 = new System.Windows.Forms.Label();
            this.Connect = new System.Windows.Forms.Button();
            this.CreateNewUser = new System.Windows.Forms.Button();
            this.Users = new System.Windows.Forms.ListBox();
            this.CreateNewSpreadsheet = new System.Windows.Forms.Button();
            this.Spreadsheets = new System.Windows.Forms.ListBox();
            this.TurnOffServer = new System.Windows.Forms.Button();
            this.ServerStatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.allUsers = new System.Windows.Forms.ListBox();
            this.allSpreads = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(246, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Noble Team Spreadsheet Manager";
            // 
            // Connect
            // 
            this.Connect.Location = new System.Drawing.Point(258, 28);
            this.Connect.Margin = new System.Windows.Forms.Padding(2);
            this.Connect.Name = "Connect";
            this.Connect.Size = new System.Drawing.Size(152, 22);
            this.Connect.TabIndex = 6;
            this.Connect.Text = "Connect to Server";
            this.Connect.UseVisualStyleBackColor = true;
            this.Connect.Click += new System.EventHandler(this.Connect_Click);
            // 
            // CreateNewUser
            // 
            this.CreateNewUser.Location = new System.Drawing.Point(336, 254);
            this.CreateNewUser.Margin = new System.Windows.Forms.Padding(2);
            this.CreateNewUser.Name = "CreateNewUser";
            this.CreateNewUser.Size = new System.Drawing.Size(152, 43);
            this.CreateNewUser.TabIndex = 1;
            this.CreateNewUser.Text = "Create User";
            this.CreateNewUser.UseVisualStyleBackColor = true;
            this.CreateNewUser.Click += new System.EventHandler(this.CreateNewUser_Click);
            // 
            // Users
            // 
            this.Users.FormattingEnabled = true;
            this.Users.Location = new System.Drawing.Point(18, 77);
            this.Users.Margin = new System.Windows.Forms.Padding(2);
            this.Users.Name = "Users";
            this.Users.Size = new System.Drawing.Size(80, 147);
            this.Users.TabIndex = 3;
            this.Users.SelectedIndexChanged += new System.EventHandler(this.Users_SelectedIndexChanged);
            // 
            // CreateNewSpreadsheet
            // 
            this.CreateNewSpreadsheet.Location = new System.Drawing.Point(502, 254);
            this.CreateNewSpreadsheet.Margin = new System.Windows.Forms.Padding(2);
            this.CreateNewSpreadsheet.Name = "CreateNewSpreadsheet";
            this.CreateNewSpreadsheet.Size = new System.Drawing.Size(152, 43);
            this.CreateNewSpreadsheet.TabIndex = 2;
            this.CreateNewSpreadsheet.Text = "Create new Spreadsheet";
            this.CreateNewSpreadsheet.UseVisualStyleBackColor = true;
            this.CreateNewSpreadsheet.Click += new System.EventHandler(this.CreateNewSpreadsheet_Click);
            // 
            // Spreadsheets
            // 
            this.Spreadsheets.FormattingEnabled = true;
            this.Spreadsheets.Location = new System.Drawing.Point(102, 77);
            this.Spreadsheets.Margin = new System.Windows.Forms.Padding(2);
            this.Spreadsheets.Name = "Spreadsheets";
            this.Spreadsheets.Size = new System.Drawing.Size(119, 147);
            this.Spreadsheets.TabIndex = 4;
            // 
            // TurnOffServer
            // 
            this.TurnOffServer.Location = new System.Drawing.Point(258, 349);
            this.TurnOffServer.Margin = new System.Windows.Forms.Padding(2);
            this.TurnOffServer.Name = "TurnOffServer";
            this.TurnOffServer.Size = new System.Drawing.Size(152, 43);
            this.TurnOffServer.TabIndex = 0;
            this.TurnOffServer.Text = "Turn off Server";
            this.TurnOffServer.UseVisualStyleBackColor = true;
            this.TurnOffServer.Click += new System.EventHandler(this.TurnOffServer_Click);
            // 
            // ServerStatus
            // 
            this.ServerStatus.AutoSize = true;
            this.ServerStatus.Location = new System.Drawing.Point(262, 58);
            this.ServerStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ServerStatus.Name = "ServerStatus";
            this.ServerStatus.Size = new System.Drawing.Size(149, 13);
            this.ServerStatus.TabIndex = 7;
            this.ServerStatus.Text = "Server Status: Not Connected";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 63);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Active User List";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(100, 63);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Active Spreadsheet List";
            // 
            // allUsers
            // 
            this.allUsers.FormattingEnabled = true;
            this.allUsers.Location = new System.Drawing.Point(336, 93);
            this.allUsers.Margin = new System.Windows.Forms.Padding(2);
            this.allUsers.Name = "allUsers";
            this.allUsers.Size = new System.Drawing.Size(154, 147);
            this.allUsers.TabIndex = 10;
            this.allUsers.SelectedIndexChanged += new System.EventHandler(this.allUsers_SelectedIndexChanged);
            // 
            // allSpreads
            // 
            this.allSpreads.FormattingEnabled = true;
            this.allSpreads.Location = new System.Drawing.Point(502, 93);
            this.allSpreads.Margin = new System.Windows.Forms.Padding(2);
            this.allSpreads.Name = "allSpreads";
            this.allSpreads.Size = new System.Drawing.Size(154, 147);
            this.allSpreads.TabIndex = 11;
            this.allSpreads.SelectedIndexChanged += new System.EventHandler(this.allSpreads_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(499, 77);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Spreadsheet List";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(335, 77);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "User List";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 406);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.allSpreads);
            this.Controls.Add(this.allUsers);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ServerStatus);
            this.Controls.Add(this.Connect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Spreadsheets);
            this.Controls.Add(this.Users);
            this.Controls.Add(this.CreateNewSpreadsheet);
            this.Controls.Add(this.CreateNewUser);
            this.Controls.Add(this.TurnOffServer);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Main";
            this.Text = "Server Manager";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Connect;
        private System.Windows.Forms.Button CreateNewUser;
        private System.Windows.Forms.ListBox Users;
        private System.Windows.Forms.Button CreateNewSpreadsheet;
        private System.Windows.Forms.ListBox Spreadsheets;
        private System.Windows.Forms.Button TurnOffServer;
        private System.Windows.Forms.Label ServerStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox allUsers;
        private System.Windows.Forms.ListBox allSpreads;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}

