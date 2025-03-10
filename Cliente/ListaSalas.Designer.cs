namespace Cliente
{
    partial class ListaSalas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListaSalas));
            this.lbSalas = new System.Windows.Forms.ListBox();
            this.lbMsgs = new System.Windows.Forms.ListBox();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.btnEnviarMsg = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.criarUmaSalaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iniciarConversaDiretaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDetalhes = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbSalas
            // 
            this.lbSalas.FormattingEnabled = true;
            this.lbSalas.ItemHeight = 16;
            this.lbSalas.Location = new System.Drawing.Point(1, 51);
            this.lbSalas.Name = "lbSalas";
            this.lbSalas.Size = new System.Drawing.Size(171, 388);
            this.lbSalas.TabIndex = 0;
            this.lbSalas.SelectedIndexChanged += new System.EventHandler(this.lbSalas_SelectedIndexChanged);
            // 
            // lbMsgs
            // 
            this.lbMsgs.FormattingEnabled = true;
            this.lbMsgs.ItemHeight = 16;
            this.lbMsgs.Location = new System.Drawing.Point(178, 51);
            this.lbMsgs.Name = "lbMsgs";
            this.lbMsgs.Size = new System.Drawing.Size(619, 356);
            this.lbMsgs.TabIndex = 1;
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(178, 412);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(546, 22);
            this.txtMsg.TabIndex = 2;
            this.txtMsg.Visible = false;
            // 
            // btnEnviarMsg
            // 
            this.btnEnviarMsg.Location = new System.Drawing.Point(722, 412);
            this.btnEnviarMsg.Name = "btnEnviarMsg";
            this.btnEnviarMsg.Size = new System.Drawing.Size(75, 23);
            this.btnEnviarMsg.TabIndex = 3;
            this.btnEnviarMsg.Text = "Enviar";
            this.btnEnviarMsg.UseVisualStyleBackColor = true;
            this.btnEnviarMsg.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.logoutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 30);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.criarUmaSalaToolStripMenuItem,
            this.iniciarConversaDiretaToolStripMenuItem});
            this.settingsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("settingsToolStripMenuItem.Image")));
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(96, 26);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // criarUmaSalaToolStripMenuItem
            // 
            this.criarUmaSalaToolStripMenuItem.Name = "criarUmaSalaToolStripMenuItem";
            this.criarUmaSalaToolStripMenuItem.Size = new System.Drawing.Size(237, 26);
            this.criarUmaSalaToolStripMenuItem.Text = "Criar uma sala";
            this.criarUmaSalaToolStripMenuItem.Click += new System.EventHandler(this.criarUmaSalaToolStripMenuItem_Click);
            // 
            // iniciarConversaDiretaToolStripMenuItem
            // 
            this.iniciarConversaDiretaToolStripMenuItem.Name = "iniciarConversaDiretaToolStripMenuItem";
            this.iniciarConversaDiretaToolStripMenuItem.Size = new System.Drawing.Size(237, 26);
            this.iniciarConversaDiretaToolStripMenuItem.Text = "Iniciar conversa direta";
            this.iniciarConversaDiretaToolStripMenuItem.Click += new System.EventHandler(this.iniciarConversaDiretaToolStripMenuItem_Click);
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.logoutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("logoutToolStripMenuItem.Image")));
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(90, 26);
            this.logoutToolStripMenuItem.Text = "Logout";
            this.logoutToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // btnDetalhes
            // 
            this.btnDetalhes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDetalhes.Location = new System.Drawing.Point(178, 22);
            this.btnDetalhes.Name = "btnDetalhes";
            this.btnDetalhes.Size = new System.Drawing.Size(114, 28);
            this.btnDetalhes.TabIndex = 5;
            this.btnDetalhes.Text = "Detalhes";
            this.btnDetalhes.UseVisualStyleBackColor = true;
            this.btnDetalhes.Visible = false;
            this.btnDetalhes.Click += new System.EventHandler(this.btnDetalhes_Click);
            // 
            // ListaSalas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 446);
            this.Controls.Add(this.btnDetalhes);
            this.Controls.Add(this.btnEnviarMsg);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.lbMsgs);
            this.Controls.Add(this.lbSalas);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ListaSalas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ListaSalas";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbSalas;
        private System.Windows.Forms.ListBox lbMsgs;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.Button btnEnviarMsg;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem criarUmaSalaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iniciarConversaDiretaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
        private System.Windows.Forms.Button btnDetalhes;
    }
}