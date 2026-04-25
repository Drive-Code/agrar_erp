using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class ReportsForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblTable;
        private ComboBox cmbTables;
        private Button btnWord;
        private Button btnExcel;
        private Button btnCreate;

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
            this.lblTable = new System.Windows.Forms.Label();
            this.cmbTables = new System.Windows.Forms.ComboBox();
            this.btnWord = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.SuspendLayout();

            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 32F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(255)))));
            this.lblTitle.Location = new System.Drawing.Point(0, 40);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(720, 60);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Создание отчёта";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(220)))));
            this.lblSubtitle.Location = new System.Drawing.Point(0, 110);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(720, 40);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Выберите таблицу и формат экспорта";
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblTable.AutoSize = true;
            this.lblTable.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTable.Location = new System.Drawing.Point(100, 200);
            this.lblTable.Name = "lblTable";
            this.lblTable.Size = new System.Drawing.Size(93, 25);
            this.lblTable.TabIndex = 2;
            this.lblTable.Text = "Таблица:";

            this.cmbTables.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(60)))));
            this.cmbTables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTables.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbTables.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.cmbTables.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.cmbTables.Location = new System.Drawing.Point(220, 197);
            this.cmbTables.Name = "cmbTables";
            this.cmbTables.Size = new System.Drawing.Size(380, 33);
            this.cmbTables.TabIndex = 3;
 
            this.btnWord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnWord.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWord.FlatAppearance.BorderSize = 0;
            this.btnWord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWord.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.btnWord.ForeColor = System.Drawing.Color.White;
            this.btnWord.Location = new System.Drawing.Point(120, 280);
            this.btnWord.Name = "btnWord";
            this.btnWord.Size = new System.Drawing.Size(220, 80);
            this.btnWord.TabIndex = 4;
            this.btnWord.Text = "Экспорт в Word";
            this.btnWord.UseVisualStyleBackColor = false;
            this.btnWord.Click += new System.EventHandler(this.BtnWord_Click);

            this.btnExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(100)))));
            this.btnExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExcel.FlatAppearance.BorderSize = 0;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcel.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.btnExcel.ForeColor = System.Drawing.Color.White;
            this.btnExcel.Location = new System.Drawing.Point(380, 280);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(220, 80);
            this.btnExcel.TabIndex = 5;
            this.btnExcel.Text = "Экспорт в Excel";
            this.btnExcel.UseVisualStyleBackColor = false;
            this.btnExcel.Click += new System.EventHandler(this.BtnExcel_Click);

            this.btnCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(160)))), ((int)(((byte)(80)))));
            this.btnCreate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCreate.FlatAppearance.BorderSize = 0;
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreate.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.btnCreate.ForeColor = System.Drawing.Color.White;
            this.btnCreate.Location = new System.Drawing.Point(250, 400);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(220, 80);
            this.btnCreate.TabIndex = 6;
            this.btnCreate.Text = "Создать отчёт";
            this.btnCreate.UseVisualStyleBackColor = false;
            this.btnCreate.Click += new System.EventHandler(this.BtnCreateReport_Click);
 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(38)))));
            this.ClientSize = new System.Drawing.Size(720, 620);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.lblTable);
            this.Controls.Add(this.cmbTables);
            this.Controls.Add(this.btnWord);
            this.Controls.Add(this.btnExcel);
            this.Controls.Add(this.btnCreate);
            this.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ReportsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Создание отчёта";
            this.Load += new System.EventHandler(this.ReportsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}