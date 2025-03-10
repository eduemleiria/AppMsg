namespace Cliente
{
    partial class detalhesSala
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
            this.btnVoltar = new System.Windows.Forms.Button();
            this.labelNomeSala = new System.Windows.Forms.Label();
            this.lbMembros = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelDescricaoSala = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelData = new System.Windows.Forms.Label();
            this.btnAdicionar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnVoltar
            // 
            this.btnVoltar.Location = new System.Drawing.Point(12, 12);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(75, 23);
            this.btnVoltar.TabIndex = 0;
            this.btnVoltar.Text = "Voltar";
            this.btnVoltar.UseVisualStyleBackColor = true;
            this.btnVoltar.Click += new System.EventHandler(this.btnVoltar_Click);
            // 
            // labelNomeSala
            // 
            this.labelNomeSala.AutoSize = true;
            this.labelNomeSala.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNomeSala.Location = new System.Drawing.Point(126, 49);
            this.labelNomeSala.Name = "labelNomeSala";
            this.labelNomeSala.Size = new System.Drawing.Size(50, 16);
            this.labelNomeSala.TabIndex = 1;
            this.labelNomeSala.Text = "label1";
            // 
            // lbMembros
            // 
            this.lbMembros.FormattingEnabled = true;
            this.lbMembros.ItemHeight = 16;
            this.lbMembros.Location = new System.Drawing.Point(12, 201);
            this.lbMembros.Name = "lbMembros";
            this.lbMembros.Size = new System.Drawing.Size(257, 164);
            this.lbMembros.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Descrição:";
            // 
            // labelDescricaoSala
            // 
            this.labelDescricaoSala.AutoSize = true;
            this.labelDescricaoSala.Location = new System.Drawing.Point(12, 103);
            this.labelDescricaoSala.Name = "labelDescricaoSala";
            this.labelDescricaoSala.Size = new System.Drawing.Size(44, 16);
            this.labelDescricaoSala.TabIndex = 4;
            this.labelDescricaoSala.Text = "label2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 182);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Membros:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Data de criação: ";
            // 
            // labelData
            // 
            this.labelData.AutoSize = true;
            this.labelData.Location = new System.Drawing.Point(141, 152);
            this.labelData.Name = "labelData";
            this.labelData.Size = new System.Drawing.Size(44, 16);
            this.labelData.TabIndex = 7;
            this.labelData.Text = "label4";
            // 
            // btnAdicionar
            // 
            this.btnAdicionar.Location = new System.Drawing.Point(179, 174);
            this.btnAdicionar.Name = "btnAdicionar";
            this.btnAdicionar.Size = new System.Drawing.Size(89, 23);
            this.btnAdicionar.TabIndex = 8;
            this.btnAdicionar.Text = "Adicionar";
            this.btnAdicionar.UseVisualStyleBackColor = true;
            this.btnAdicionar.Visible = false;
            this.btnAdicionar.Click += new System.EventHandler(this.btnAdicionar_Click);
            // 
            // detalhesSala
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 377);
            this.Controls.Add(this.btnAdicionar);
            this.Controls.Add(this.labelData);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelDescricaoSala);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbMembros);
            this.Controls.Add(this.labelNomeSala);
            this.Controls.Add(this.btnVoltar);
            this.Name = "detalhesSala";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "detalhesSala";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnVoltar;
        private System.Windows.Forms.Label labelNomeSala;
        private System.Windows.Forms.ListBox lbMembros;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelDescricaoSala;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelData;
        private System.Windows.Forms.Button btnAdicionar;
    }
}