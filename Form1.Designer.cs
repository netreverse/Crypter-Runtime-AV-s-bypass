namespace Server_Crypter
{
    partial class cryptNj
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.boosterButton2 = new BoosterButton();
            this.boosterButton1 = new BoosterButton();
            this.boosterTextBox1 = new BoosterTextBox();
            this.stubRicheText = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.xylosNotice1 = new XylosNotice();
            this.resourcesRichText = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Path of Server:";
            // 
            // boosterButton2
            // 
            this.boosterButton2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.boosterButton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(182)))), ((int)(((byte)(182)))));
            this.boosterButton2.Location = new System.Drawing.Point(148, 109);
            this.boosterButton2.Name = "boosterButton2";
            this.boosterButton2.Size = new System.Drawing.Size(75, 38);
            this.boosterButton2.TabIndex = 3;
            this.boosterButton2.Text = "Build";
            this.boosterButton2.UseVisualStyleBackColor = true;
            this.boosterButton2.Click += new System.EventHandler(this.boosterButton2_Click);
            // 
            // boosterButton1
            // 
            this.boosterButton1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.boosterButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(182)))), ((int)(((byte)(182)))));
            this.boosterButton1.Location = new System.Drawing.Point(332, 54);
            this.boosterButton1.Name = "boosterButton1";
            this.boosterButton1.Size = new System.Drawing.Size(42, 23);
            this.boosterButton1.TabIndex = 1;
            this.boosterButton1.Text = "....";
            this.boosterButton1.UseVisualStyleBackColor = true;
            this.boosterButton1.Click += new System.EventHandler(this.boosterButton1_Click);
            // 
            // boosterTextBox1
            // 
            this.boosterTextBox1.Location = new System.Drawing.Point(44, 57);
            this.boosterTextBox1.MultiLine = true;
            this.boosterTextBox1.Name = "boosterTextBox1";
            this.boosterTextBox1.ReadOnly = false;
            this.boosterTextBox1.Size = new System.Drawing.Size(272, 20);
            this.boosterTextBox1.TabIndex = 0;
            this.boosterTextBox1.UseSystemPasswordChar = false;
            // 
            // stubRicheText
            // 
            this.stubRicheText.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.stubRicheText.Location = new System.Drawing.Point(536, 41);
            this.stubRicheText.Name = "stubRicheText";
            this.stubRicheText.Size = new System.Drawing.Size(583, 137);
            this.stubRicheText.TabIndex = 4;
            this.stubRicheText.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(533, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Stub:";
            // 
            // xylosNotice1
            // 
            this.xylosNotice1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.xylosNotice1.Cursor = System.Windows.Forms.Cursors.Default;
            this.xylosNotice1.Enabled = false;
            this.xylosNotice1.Location = new System.Drawing.Point(312, 409);
            this.xylosNotice1.Multiline = true;
            this.xylosNotice1.Name = "xylosNotice1";
            this.xylosNotice1.ReadOnly = true;
            this.xylosNotice1.Size = new System.Drawing.Size(425, 36);
            this.xylosNotice1.TabIndex = 6;
            this.xylosNotice1.Text = "Add some junk code for FUDn and you can use upx for low detection";
            // 
            // resourcesRichText
            // 
            this.resourcesRichText.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.resourcesRichText.Location = new System.Drawing.Point(536, 230);
            this.resourcesRichText.Name = "resourcesRichText";
            this.resourcesRichText.Size = new System.Drawing.Size(583, 137);
            this.resourcesRichText.TabIndex = 7;
            this.resourcesRichText.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(536, 211);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = ".res encodés en AES256";
            // 
            // cryptNj
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.ClientSize = new System.Drawing.Size(1148, 457);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.resourcesRichText);
            this.Controls.Add(this.xylosNotice1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.stubRicheText);
            this.Controls.Add(this.boosterButton2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.boosterButton1);
            this.Controls.Add(this.boosterTextBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "cryptNj";
            this.Text = "Server Crypter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public BoosterTextBox boosterTextBox1;
        private BoosterButton boosterButton1;
        private System.Windows.Forms.Label label1;
        private BoosterButton boosterButton2;
        public System.Windows.Forms.RichTextBox stubRicheText;
        private System.Windows.Forms.Label label2;
        private XylosNotice xylosNotice1;
        private System.Windows.Forms.RichTextBox resourcesRichText;
        private System.Windows.Forms.Label label3;
    }
}

