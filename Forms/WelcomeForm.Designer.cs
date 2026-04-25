using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class WelcomeForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.panelCenter = new System.Windows.Forms.Panel();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblFooter = new System.Windows.Forms.Label();
            this.panelCenter.SuspendLayout();
            this.SuspendLayout();
 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 36F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(255)))));
            this.lblTitle.Location = new System.Drawing.Point(21, 97);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(497, 65);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Автоматный участок";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(200)))));
            this.lblSubtitle.Location = new System.Drawing.Point(27, 162);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(509, 30);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Система учёта материалов и готовой продукции";
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.panelCenter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(60)))));
            this.panelCenter.Controls.Add(this.btnLogin);
            this.panelCenter.Controls.Add(this.btnExit);
            this.panelCenter.Location = new System.Drawing.Point(238, 238);
            this.panelCenter.Name = "panelCenter";
            this.panelCenter.Size = new System.Drawing.Size(420, 260);
            this.panelCenter.TabIndex = 2;
            this.panelCenter.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelCenter_Paint);

            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(50, 60);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(320, 70);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "Войти в систему";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            this.btnLogin.MouseEnter += new System.EventHandler(this.BtnLogin_MouseEnter);
            this.btnLogin.MouseLeave += new System.EventHandler(this.BtnLogin_MouseLeave);

            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(50, 150);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(320, 70);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "Выход";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);
            this.btnExit.MouseEnter += new System.EventHandler(this.BtnExit_MouseEnter);
            this.btnExit.MouseLeave += new System.EventHandler(this.BtnExit_MouseLeave);

            this.lblFooter.AutoSize = true;
            this.lblFooter.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFooter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(140)))));
            this.lblFooter.Location = new System.Drawing.Point(0, 531);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.Size = new System.Drawing.Size(203, 15);
            this.lblFooter.TabIndex = 3;
            this.lblFooter.Text = "© 2025 • Автоматный участок • v0.1";
            this.lblFooter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(904, 581);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.panelCenter);
            this.Controls.Add(this.lblFooter);
            this.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WelcomeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Учёт материалов и продукции";
            this.Load += new System.EventHandler(this.WelcomeForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.WelcomeForm_Paint);
            this.panelCenter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Label lblTitle;
        private Label lblSubtitle;
        private Panel panelCenter;
        private Button btnLogin;
        private Button btnExit;
        private Label lblFooter;
    }
}