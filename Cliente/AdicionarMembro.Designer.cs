namespace Cliente
{
    partial class AdicionarMembro
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
            this.btnAdicionar = new System.Windows.Forms.Button();
            this.txtUserAdd = new System.Windows.Forms.TextBox();
            this.btnVoltar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Utilizador a adicionar à sala:";
            // 
            // btnAdicionar
            // 
            this.btnAdicionar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdicionar.Location = new System.Drawing.Point(189, 131);
            this.btnAdicionar.Name = "btnAdicionar";
            this.btnAdicionar.Size = new System.Drawing.Size(94, 38);
            this.btnAdicionar.TabIndex = 1;
            this.btnAdicionar.Text = "Adicionar";
            this.btnAdicionar.UseVisualStyleBackColor = true;
            this.btnAdicionar.Click += new System.EventHandler(this.btnAdicionar_Click);
            // 
            // txtUserAdd
            // 
            this.txtUserAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserAdd.Location = new System.Drawing.Point(12, 101);
            this.txtUserAdd.Name = "txtUserAdd";
            this.txtUserAdd.Size = new System.Drawing.Size(271, 24);
            this.txtUserAdd.TabIndex = 2;
            // 
            // btnVoltar
            // 
            this.btnVoltar.Location = new System.Drawing.Point(12, 12);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(75, 23);
            this.btnVoltar.TabIndex = 3;
            this.btnVoltar.Text = "Voltar";
            this.btnVoltar.UseVisualStyleBackColor = true;
            this.btnVoltar.Click += new System.EventHandler(this.btnVoltar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(61, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "Convidar utilizadores";
            // 
            // AdicionarMembro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 181);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnVoltar);
            this.Controls.Add(this.txtUserAdd);
            this.Controls.Add(this.btnAdicionar);
            this.Controls.Add(this.label1);
            this.Name = "AdicionarMembro";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AdicionarMembro";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAdicionar;
        private System.Windows.Forms.TextBox txtUserAdd;
        private System.Windows.Forms.Button btnVoltar;
        private System.Windows.Forms.Label label2;
    }
}