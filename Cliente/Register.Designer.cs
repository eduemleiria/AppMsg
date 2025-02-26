namespace Cliente
{
    partial class Register
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
            this.txtRUsername = new System.Windows.Forms.TextBox();
            this.btnRegistar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRPass = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRPass2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnIrLogin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(102, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Criar conta";
            // 
            // txtRUsername
            // 
            this.txtRUsername.Location = new System.Drawing.Point(81, 84);
            this.txtRUsername.Name = "txtRUsername";
            this.txtRUsername.Size = new System.Drawing.Size(193, 22);
            this.txtRUsername.TabIndex = 1;
            // 
            // btnRegistar
            // 
            this.btnRegistar.Location = new System.Drawing.Point(124, 238);
            this.btnRegistar.Name = "btnRegistar";
            this.btnRegistar.Size = new System.Drawing.Size(94, 38);
            this.btnRegistar.TabIndex = 2;
            this.btnRegistar.Text = "Registar";
            this.btnRegistar.UseVisualStyleBackColor = true;
            this.btnRegistar.Click += new System.EventHandler(this.btnRegistar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(81, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Username:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(81, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Password:";
            // 
            // txtRPass
            // 
            this.txtRPass.Location = new System.Drawing.Point(81, 138);
            this.txtRPass.Name = "txtRPass";
            this.txtRPass.Size = new System.Drawing.Size(193, 22);
            this.txtRPass.TabIndex = 4;
            this.txtRPass.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(81, 177);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 16);
            this.label4.TabIndex = 7;
            this.label4.Text = "Repita a password:";
            // 
            // txtRPass2
            // 
            this.txtRPass2.Location = new System.Drawing.Point(81, 199);
            this.txtRPass2.Name = "txtRPass2";
            this.txtRPass2.Size = new System.Drawing.Size(193, 22);
            this.txtRPass2.TabIndex = 6;
            this.txtRPass2.UseSystemPasswordChar = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(114, 295);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(119, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = "Já tem uma conta?";
            // 
            // btnIrLogin
            // 
            this.btnIrLogin.Location = new System.Drawing.Point(93, 314);
            this.btnIrLogin.Name = "btnIrLogin";
            this.btnIrLogin.Size = new System.Drawing.Size(162, 38);
            this.btnIrLogin.TabIndex = 8;
            this.btnIrLogin.Text = "Fazer Login";
            this.btnIrLogin.UseVisualStyleBackColor = true;
            this.btnIrLogin.Click += new System.EventHandler(this.btnIrLogin_Click);
            // 
            // Register
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 378);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnIrLogin);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtRPass2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtRPass);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnRegistar);
            this.Controls.Add(this.txtRUsername);
            this.Controls.Add(this.label1);
            this.Name = "Register";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Register";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRUsername;
        private System.Windows.Forms.Button btnRegistar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRPass;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtRPass2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnIrLogin;
    }
}