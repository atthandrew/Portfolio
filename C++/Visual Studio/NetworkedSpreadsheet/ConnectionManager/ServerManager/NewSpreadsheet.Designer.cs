namespace ServerManager
{
    partial class NewSpreadsheet
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
            this.label2 = new System.Windows.Forms.Label();
            this.NameSp = new System.Windows.Forms.TextBox();
            this.CreateNew = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(184, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(196, 25);
            this.label2.TabIndex = 23;
            this.label2.Text = "Spreadsheet Name";
            // 
            // NameSp
            // 
            this.NameSp.Location = new System.Drawing.Point(180, 104);
            this.NameSp.Name = "NameSp";
            this.NameSp.Size = new System.Drawing.Size(200, 31);
            this.NameSp.TabIndex = 22;
            // 
            // CreateNew
            // 
            this.CreateNew.Location = new System.Drawing.Point(128, 164);
            this.CreateNew.Name = "CreateNew";
            this.CreateNew.Size = new System.Drawing.Size(303, 42);
            this.CreateNew.TabIndex = 21;
            this.CreateNew.Text = "Create";
            this.CreateNew.UseVisualStyleBackColor = true;
            this.CreateNew.Click += new System.EventHandler(this.CreateNew_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(193, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 25);
            this.label1.TabIndex = 20;
            this.label1.Text = "New Spreadsheet";
            // 
            // NewSpreadsheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 246);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NameSp);
            this.Controls.Add(this.CreateNew);
            this.Controls.Add(this.label1);
            this.Name = "NewSpreadsheet";
            this.Text = "New Spreadsheet";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox NameSp;
        private System.Windows.Forms.Button CreateNew;
        private System.Windows.Forms.Label label1;
    }
}