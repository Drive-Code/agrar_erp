using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtLogin;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblError;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.BackColor = Color.FromArgb(30, 30, 40);
            this.ClientSize = new Size(404, 381);
            this.Font = new Font("Segoe UI", 11F);
            this.ForeColor = Color.WhiteSmoke;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Авторизация";
            this.Load += new System.EventHandler(this.LoginForm_Load);

            Label lblTitle = new Label();
            lblTitle.Text = "Вход в систему";
            lblTitle.Font = new Font("Segoe UI Semibold", 24, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(100, 180, 255);
            lblTitle.Location = new Point(0, 40);
            lblTitle.Size = new Size(420, 50);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitle);

            Label lblLogin = new Label();
            lblLogin.Text = "Логин:";
            lblLogin.Location = new Point(50, 120);
            lblLogin.AutoSize = true;
            this.Controls.Add(lblLogin);

            txtLogin = new TextBox();
            txtLogin.Location = new Point(120, 117);
            txtLogin.Size = new Size(220, 30);
            txtLogin.BackColor = Color.FromArgb(50, 50, 60);
            txtLogin.ForeColor = Color.WhiteSmoke;
            this.Controls.Add(txtLogin);

            Label lblPass = new Label();
            lblPass.Text = "Пароль:";
            lblPass.Location = new Point(50, 170);
            lblPass.AutoSize = true;
            this.Controls.Add(lblPass);

            txtPassword = new TextBox();
            txtPassword.Location = new Point(120, 167);
            txtPassword.Size = new Size(220, 30);
            txtPassword.PasswordChar = '*';
            txtPassword.BackColor = Color.FromArgb(50, 50, 60);
            txtPassword.ForeColor = Color.WhiteSmoke;
            this.Controls.Add(txtPassword);

            btnLogin = new Button();
            btnLogin.Text = "Войти";
            btnLogin.Size = new Size(180, 50);
            btnLogin.Location = new Point(120, 230);
            btnLogin.BackColor = Color.FromArgb(70, 130, 180);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            this.Controls.Add(btnLogin);

            lblError = new Label();
            lblError.Text = "";
            lblError.ForeColor = Color.Red;
            lblError.Location = new Point(50, 290);
            lblError.Size = new Size(320, 30);
            lblError.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblError);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}