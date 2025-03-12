namespace Cliente
{
    partial class ChatSala
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
            this.labelNomeSala = new System.Windows.Forms.Label();
            this.btnDetalhes = new System.Windows.Forms.Button();
            this.btnEnviarMsg = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.lbMsgs = new System.Windows.Forms.ListBox();
            this.btnVoltar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelNomeSala
            // 
            this.labelNomeSala.AutoSize = true;
            this.labelNomeSala.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNomeSala.Location = new System.Drawing.Point(12, 41);
            this.labelNomeSala.Name = "labelNomeSala";
            this.labelNomeSala.Size = new System.Drawing.Size(59, 20);
            this.labelNomeSala.TabIndex = 0;
            this.labelNomeSala.Text = "label1";
            // 
            // btnDetalhes
            // 
            this.btnDetalhes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDetalhes.Location = new System.Drawing.Point(579, 64);
            this.btnDetalhes.Name = "btnDetalhes";
            this.btnDetalhes.Size = new System.Drawing.Size(114, 28);
            this.btnDetalhes.TabIndex = 9;
            this.btnDetalhes.Text = "Detalhes";
            this.btnDetalhes.UseVisualStyleBackColor = true;
            this.btnDetalhes.Click += new System.EventHandler(this.btnDetalhes_Click);
            // 
            // btnEnviarMsg
            // 
            this.btnEnviarMsg.Location = new System.Drawing.Point(618, 406);
            this.btnEnviarMsg.Name = "btnEnviarMsg";
            this.btnEnviarMsg.Size = new System.Drawing.Size(75, 28);
            this.btnEnviarMsg.TabIndex = 8;
            this.btnEnviarMsg.Text = "Enviar";
            this.btnEnviarMsg.UseVisualStyleBackColor = true;
            this.btnEnviarMsg.Click += new System.EventHandler(this.btnEnviarMsg_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMsg.Location = new System.Drawing.Point(12, 406);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(600, 28);
            this.txtMsg.TabIndex = 7;
            // 
            // lbMsgs
            // 
            this.lbMsgs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMsgs.FormattingEnabled = true;
            this.lbMsgs.ItemHeight = 18;
            this.lbMsgs.Location = new System.Drawing.Point(12, 64);
            this.lbMsgs.Name = "lbMsgs";
            this.lbMsgs.Size = new System.Drawing.Size(681, 346);
            this.lbMsgs.TabIndex = 6;
            // 
            // btnVoltar
            // 
            this.btnVoltar.Location = new System.Drawing.Point(1, 1);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(98, 33);
            this.btnVoltar.TabIndex = 10;
            this.btnVoltar.Text = "Voltar";
            this.btnVoltar.UseVisualStyleBackColor = true;
            this.btnVoltar.Click += new System.EventHandler(this.btnVoltar_Click);
            // 
            // ChatSala
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 444);
            this.Controls.Add(this.btnVoltar);
            this.Controls.Add(this.btnDetalhes);
            this.Controls.Add(this.btnEnviarMsg);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.lbMsgs);
            this.Controls.Add(this.labelNomeSala);
            this.Name = "ChatSala";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ChatSala";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelNomeSala;
        private System.Windows.Forms.Button btnDetalhes;
        private System.Windows.Forms.Button btnEnviarMsg;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.ListBox lbMsgs;
        private System.Windows.Forms.Button btnVoltar;
    }
}