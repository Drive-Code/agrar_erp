using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class ConfirmDeleteForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblMessage;
        private Button btnYes;
        private Button btnNo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.SuspendLayout();

            this.lblMessage.Location = new System.Drawing.Point(20, 30);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(360, 50);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Удалить запись?";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.btnYes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnYes.FlatAppearance.BorderSize = 0;
            this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYes.ForeColor = System.Drawing.Color.White;
            this.btnYes.Location = new System.Drawing.Point(80, 100);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(100, 40);
            this.btnYes.TabIndex = 1;
            this.btnYes.Text = "Да";
            this.btnYes.UseVisualStyleBackColor = false;
            this.btnYes.Click += new System.EventHandler(this.BtnYes_Click);
 
            this.btnNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnNo.FlatAppearance.BorderSize = 0;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNo.ForeColor = System.Drawing.Color.White;
            this.btnNo.Location = new System.Drawing.Point(220, 100);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(100, 40);
            this.btnNo.TabIndex = 2;
            this.btnNo.Text = "Нет";
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Click += new System.EventHandler(this.BtnNo_Click);

            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(400, 180);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.btnNo);
            this.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfirmDeleteForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Подтверждение удаления";
            this.Load += new System.EventHandler(this.ConfirmDeleteForm_Load);
            this.ResumeLayout(false);

        }
    }
}