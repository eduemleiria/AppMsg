namespace Cliente
{
    partial class ConversaDireta
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConversaDireta));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnEnviarMsgD = new System.Windows.Forms.Button();
            this.txtMsgD = new System.Windows.Forms.TextBox();
            this.lbMsgsD = new System.Windows.Forms.ListBox();
            this.lblAfalarCom = new System.Windows.Forms.Label();
            this.btnDesconectar = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logoutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(643, 28);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.logoutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("logoutToolStripMenuItem.Image")));
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(90, 24);
            this.logoutToolStripMenuItem.Text = "Logout";
            this.logoutToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // btnEnviarMsgD
            // 
            this.btnEnviarMsgD.Location = new System.Drawing.Point(556, 438);
            this.btnEnviarMsgD.Name = "btnEnviarMsgD";
            this.btnEnviarMsgD.Size = new System.Drawing.Size(75, 23);
            this.btnEnviarMsgD.TabIndex = 8;
            this.btnEnviarMsgD.Text = "Enviar";
            this.btnEnviarMsgD.UseVisualStyleBackColor = true;
            // 
            // txtMsgD
            // 
            this.txtMsgD.Location = new System.Drawing.Point(12, 438);
            this.txtMsgD.Name = "txtMsgD";
            this.txtMsgD.Size = new System.Drawing.Size(546, 22);
            this.txtMsgD.TabIndex = 7;
            // 
            // lbMsgsD
            // 
            this.lbMsgsD.FormattingEnabled = true;
            this.lbMsgsD.ItemHeight = 16;
            this.lbMsgsD.Location = new System.Drawing.Point(12, 61);
            this.lbMsgsD.Name = "lbMsgsD";
            this.lbMsgsD.Size = new System.Drawing.Size(619, 372);
            this.lbMsgsD.TabIndex = 6;
            // 
            // lblAfalarCom
            // 
            this.lblAfalarCom.AutoSize = true;
            this.lblAfalarCom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAfalarCom.Location = new System.Drawing.Point(126, 39);
            this.lblAfalarCom.Name = "lblAfalarCom";
            this.lblAfalarCom.Size = new System.Drawing.Size(103, 18);
            this.lblAfalarCom.TabIndex = 9;
            this.lblAfalarCom.Text = "Chat vazio...";
            // 
            // btnDesconectar
            // 
            this.btnDesconectar.Location = new System.Drawing.Point(12, 34);
            this.btnDesconectar.Name = "btnDesconectar";
            this.btnDesconectar.Size = new System.Drawing.Size(99, 23);
            this.btnDesconectar.TabIndex = 11;
            this.btnDesconectar.Text = "Desconectar";
            this.btnDesconectar.UseVisualStyleBackColor = true;
            this.btnDesconectar.Click += new System.EventHandler(this.btnDesconectar_Click);
            // 
            // ConversaDireta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 470);
            this.Controls.Add(this.btnDesconectar);
            this.Controls.Add(this.lblAfalarCom);
            this.Controls.Add(this.btnEnviarMsgD);
            this.Controls.Add(this.txtMsgD);
            this.Controls.Add(this.lbMsgsD);
            this.Controls.Add(this.menuStrip1);
            this.Name = "ConversaDireta";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ConversaDireta";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
        private System.Windows.Forms.Button btnEnviarMsgD;
        private System.Windows.Forms.TextBox txtMsgD;
        private System.Windows.Forms.ListBox lbMsgsD;
        private System.Windows.Forms.Label lblAfalarCom;
        private System.Windows.Forms.Button btnDesconectar;
    }
}