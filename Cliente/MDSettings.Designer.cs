namespace Cliente
{
    partial class MDSettings
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
            this.btnEntrarMD = new System.Windows.Forms.Button();
            this.btnCriarMD = new System.Windows.Forms.Button();
            this.btnVoltar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(104, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mensagens Diretas";
            // 
            // btnEntrarMD
            // 
            this.btnEntrarMD.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEntrarMD.Location = new System.Drawing.Point(13, 84);
            this.btnEntrarMD.Name = "btnEntrarMD";
            this.btnEntrarMD.Size = new System.Drawing.Size(159, 66);
            this.btnEntrarMD.TabIndex = 1;
            this.btnEntrarMD.Text = "Entrar";
            this.btnEntrarMD.UseVisualStyleBackColor = true;
            this.btnEntrarMD.Click += new System.EventHandler(this.btnEntrarMD_Click);
            // 
            // btnCriarMD
            // 
            this.btnCriarMD.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCriarMD.Location = new System.Drawing.Point(200, 84);
            this.btnCriarMD.Name = "btnCriarMD";
            this.btnCriarMD.Size = new System.Drawing.Size(159, 66);
            this.btnCriarMD.TabIndex = 2;
            this.btnCriarMD.Text = "Criar";
            this.btnCriarMD.UseVisualStyleBackColor = true;
            this.btnCriarMD.Click += new System.EventHandler(this.btnCriarMD_Click);
            // 
            // btnVoltar
            // 
            this.btnVoltar.Location = new System.Drawing.Point(13, 13);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(75, 23);
            this.btnVoltar.TabIndex = 3;
            this.btnVoltar.Text = "Voltar";
            this.btnVoltar.UseVisualStyleBackColor = true;
            this.btnVoltar.Click += new System.EventHandler(this.btnVoltar_Click);
            // 
            // MDSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 182);
            this.Controls.Add(this.btnVoltar);
            this.Controls.Add(this.btnCriarMD);
            this.Controls.Add(this.btnEntrarMD);
            this.Controls.Add(this.label1);
            this.Name = "MDSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MDSettings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEntrarMD;
        private System.Windows.Forms.Button btnCriarMD;
        private System.Windows.Forms.Button btnVoltar;
    }
}