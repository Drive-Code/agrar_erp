using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class MainWorkingForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel sidebar;
        private Panel contentArea;
        private Button btnToggleMenu;
        private Label lblLogo;
        private DataGridView dgvDirectories;
        private DataGridView dgvData;
        private Label lblDataTitle;
        private TextBox txtSearch;
        private Button btnSearch;
        private Label lblReportTitle;
        private Label lblSelectTable;
        private ComboBox cmbReportTables;
        private Button btnWord;
        private Button btnExcel;
        private Label lblReportInfo;
        private Button btnDirAdd;
        private Button btnDirUpdate;
        private Button btnDirDelete;
        private Button btnDataAdd;
        private Button btnDataUpdate;
        private Button btnDataDelete;

        // Пагинация для данных
        private Button btnPrevPage;
        private Button btnNextPage;
        private Label lblPageInfo;

        // Пагинация для справочников
        private Button btnDirPrevPage;
        private Button btnDirNextPage;
        private Label lblDirPageInfo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.sidebar = new System.Windows.Forms.Panel();
            this.btnToggleMenu = new System.Windows.Forms.Button();
            this.lblLogo = new System.Windows.Forms.Label();
            this.contentArea = new System.Windows.Forms.Panel();
            this.dgvDirectories = new System.Windows.Forms.DataGridView();
            this.btnDirAdd = new System.Windows.Forms.Button();
            this.btnDirUpdate = new System.Windows.Forms.Button();
            this.btnDirDelete = new System.Windows.Forms.Button();
            this.btnDirPrevPage = new System.Windows.Forms.Button();
            this.btnDirNextPage = new System.Windows.Forms.Button();
            this.lblDirPageInfo = new System.Windows.Forms.Label();
            this.lblDataTitle = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.btnDataAdd = new System.Windows.Forms.Button();
            this.btnDataUpdate = new System.Windows.Forms.Button();
            this.btnDataDelete = new System.Windows.Forms.Button();
            this.btnPrevPage = new System.Windows.Forms.Button();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.lblPageInfo = new System.Windows.Forms.Label();
            this.lblReportTitle = new System.Windows.Forms.Label();
            this.lblSelectTable = new System.Windows.Forms.Label();
            this.cmbReportTables = new System.Windows.Forms.ComboBox();
            this.btnWord = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
            this.lblReportInfo = new System.Windows.Forms.Label();
            this.sidebar.SuspendLayout();
            this.contentArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDirectories)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();

            this.sidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(45)))));
            this.sidebar.Controls.Add(this.btnToggleMenu);
            this.sidebar.Controls.Add(this.lblLogo);
            this.sidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebar.Location = new System.Drawing.Point(0, 0);
            this.sidebar.Name = "sidebar";
            this.sidebar.Size = new System.Drawing.Size(70, 761);
            this.sidebar.TabIndex = 0;

            this.btnToggleMenu.BackColor = System.Drawing.Color.Transparent;
            this.btnToggleMenu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToggleMenu.FlatAppearance.BorderSize = 0;
            this.btnToggleMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleMenu.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.btnToggleMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(220)))));
            this.btnToggleMenu.Location = new System.Drawing.Point(10, 10);
            this.btnToggleMenu.Name = "btnToggleMenu";
            this.btnToggleMenu.Size = new System.Drawing.Size(50, 50);
            this.btnToggleMenu.TabIndex = 0;
            this.btnToggleMenu.Text = "→";
            this.btnToggleMenu.UseVisualStyleBackColor = false;
            this.btnToggleMenu.Click += new System.EventHandler(this.BtnToggleMenu_Click);

            this.lblLogo.AutoSize = true;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(255)))));
            this.lblLogo.Location = new System.Drawing.Point(70, 30);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(77, 30);
            this.lblLogo.TabIndex = 1;
            this.lblLogo.Text = "Меню";
            this.lblLogo.Visible = false;

            this.contentArea.AutoScroll = true;
            this.contentArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(40)))));
            this.contentArea.Controls.Add(this.dgvDirectories);
            this.contentArea.Controls.Add(this.btnDirAdd);
            this.contentArea.Controls.Add(this.btnDirUpdate);
            this.contentArea.Controls.Add(this.btnDirDelete);
            this.contentArea.Controls.Add(this.btnDirPrevPage);
            this.contentArea.Controls.Add(this.btnDirNextPage);
            this.contentArea.Controls.Add(this.lblDirPageInfo);
            this.contentArea.Controls.Add(this.lblDataTitle);
            this.contentArea.Controls.Add(this.txtSearch);
            this.contentArea.Controls.Add(this.btnSearch);
            this.contentArea.Controls.Add(this.dgvData);
            this.contentArea.Controls.Add(this.btnDataAdd);
            this.contentArea.Controls.Add(this.btnDataUpdate);
            this.contentArea.Controls.Add(this.btnDataDelete);
            this.contentArea.Controls.Add(this.btnPrevPage);
            this.contentArea.Controls.Add(this.btnNextPage);
            this.contentArea.Controls.Add(this.lblPageInfo);
            this.contentArea.Controls.Add(this.lblReportTitle);
            this.contentArea.Controls.Add(this.lblSelectTable);
            this.contentArea.Controls.Add(this.cmbReportTables);
            this.contentArea.Controls.Add(this.btnWord);
            this.contentArea.Controls.Add(this.btnExcel);
            this.contentArea.Controls.Add(this.lblReportInfo);
            this.contentArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentArea.Location = new System.Drawing.Point(0, 0);
            this.contentArea.Name = "contentArea";
            this.contentArea.Size = new System.Drawing.Size(1264, 761);
            this.contentArea.TabIndex = 1;
            this.contentArea.Paint += new System.Windows.Forms.PaintEventHandler(this.contentArea_Paint_1);
            this.contentArea.AutoScroll = true;
           

            this.dgvDirectories.AllowUserToAddRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(45)))));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.dgvDirectories.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvDirectories.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDirectories.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))));
            this.dgvDirectories.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(60)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.dgvDirectories.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDirectories.DefaultCellStyle = dataGridViewCellStyle9;
            this.dgvDirectories.EnableHeadersVisualStyles = false;
            this.dgvDirectories.Location = new System.Drawing.Point(94, 30);
            this.dgvDirectories.MultiSelect = false;
            this.dgvDirectories.Name = "dgvDirectories";
            this.dgvDirectories.ReadOnly = true;
            this.dgvDirectories.RowHeadersVisible = false;
            this.dgvDirectories.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDirectories.Size = new System.Drawing.Size(1000, 300);
            this.dgvDirectories.TabIndex = 0;
            this.dgvDirectories.Tag = "DirsGrid";
            this.dgvDirectories.Visible = false;
            this.dgvDirectories.SelectionChanged += new System.EventHandler(this.DgvDirectories_SelectionChanged);

            this.btnDirAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnDirAdd.FlatAppearance.BorderSize = 0;
            this.btnDirAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDirAdd.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnDirAdd.ForeColor = System.Drawing.Color.White;
            this.btnDirAdd.Location = new System.Drawing.Point(144, 340);
            this.btnDirAdd.Name = "btnDirAdd";
            this.btnDirAdd.Size = new System.Drawing.Size(140, 40);
            this.btnDirAdd.TabIndex = 1;
            this.btnDirAdd.Tag = "DirAddBtn";
            this.btnDirAdd.Text = "Добавить";
            this.btnDirAdd.UseVisualStyleBackColor = false;
            this.btnDirAdd.Visible = false;
            this.btnDirAdd.Click += new System.EventHandler(this.BtnDirAdd_Click);

            this.btnDirUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(160)))), ((int)(((byte)(80)))));
            this.btnDirUpdate.Enabled = false;
            this.btnDirUpdate.FlatAppearance.BorderSize = 0;
            this.btnDirUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDirUpdate.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnDirUpdate.ForeColor = System.Drawing.Color.White;
            this.btnDirUpdate.Location = new System.Drawing.Point(300, 340);
            this.btnDirUpdate.Name = "btnDirUpdate";
            this.btnDirUpdate.Size = new System.Drawing.Size(140, 40);
            this.btnDirUpdate.TabIndex = 2;
            this.btnDirUpdate.Tag = "DirUpdateBtn";
            this.btnDirUpdate.Text = "Обновить";
            this.btnDirUpdate.UseVisualStyleBackColor = false;
            this.btnDirUpdate.Visible = false;
            this.btnDirUpdate.Click += new System.EventHandler(this.BtnDirUpdate_Click);

            this.btnDirDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnDirDelete.Enabled = false;
            this.btnDirDelete.FlatAppearance.BorderSize = 0;
            this.btnDirDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDirDelete.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnDirDelete.ForeColor = System.Drawing.Color.White;
            this.btnDirDelete.Location = new System.Drawing.Point(460, 340);
            this.btnDirDelete.Name = "btnDirDelete";
            this.btnDirDelete.Size = new System.Drawing.Size(140, 40);
            this.btnDirDelete.TabIndex = 3;
            this.btnDirDelete.Tag = "DirDeleteBtn";
            this.btnDirDelete.Text = "Удалить";
            this.btnDirDelete.UseVisualStyleBackColor = false;
            this.btnDirDelete.Visible = false;
            this.btnDirDelete.Click += new System.EventHandler(this.BtnDirDelete_Click);

            // Пагинация для справочников — под кнопками действий
            this.btnDirPrevPage = new Button();
            this.btnDirPrevPage.Text = "← Назад";
            this.btnDirPrevPage.Size = new Size(100, 35);
            this.btnDirPrevPage.Location = new Point(144, 390);
            this.btnDirPrevPage.BackColor = Color.FromArgb(70, 130, 180);
            this.btnDirPrevPage.ForeColor = Color.White;
            this.btnDirPrevPage.FlatStyle = FlatStyle.Flat;
            this.btnDirPrevPage.FlatAppearance.BorderSize = 0;
            this.btnDirPrevPage.Visible = false;
            this.btnDirPrevPage.Click += new System.EventHandler(this.BtnDirPrevPage_Click);
            this.contentArea.Controls.Add(this.btnDirPrevPage);

            this.btnDirNextPage = new Button();
            this.btnDirNextPage.Text = "Вперёд →";
            this.btnDirNextPage.Size = new Size(100, 35);
            this.btnDirNextPage.Location = new Point(254, 390);
            this.btnDirNextPage.BackColor = Color.FromArgb(70, 130, 180);
            this.btnDirNextPage.ForeColor = Color.White;
            this.btnDirNextPage.FlatStyle = FlatStyle.Flat;
            this.btnDirNextPage.FlatAppearance.BorderSize = 0;
            this.btnDirNextPage.Visible = false;
            this.btnDirNextPage.Click += new System.EventHandler(this.BtnDirNextPage_Click);
            this.contentArea.Controls.Add(this.btnDirNextPage);

            this.lblDirPageInfo = new Label();
            this.lblDirPageInfo.Text = "Страница 1 из 1";
            this.lblDirPageInfo.Location = new Point(374, 397);
            this.lblDirPageInfo.AutoSize = true;
            this.lblDirPageInfo.ForeColor = Color.WhiteSmoke;
            this.lblDirPageInfo.Font = new Font("Segoe UI", 11);
            this.lblDirPageInfo.Visible = false;
            this.contentArea.Controls.Add(this.lblDirPageInfo);

            this.lblDataTitle.AutoSize = true;
            this.lblDataTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblDataTitle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblDataTitle.Location = new System.Drawing.Point(94, 20);
            this.lblDataTitle.Name = "lblDataTitle";
            this.lblDataTitle.Size = new System.Drawing.Size(0, 45);
            this.lblDataTitle.TabIndex = 4;
            this.lblDataTitle.Tag = "DataTitle";
            this.lblDataTitle.Visible = false;

            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(60)))));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtSearch.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtSearch.Location = new System.Drawing.Point(94, 69);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(500, 29);
            this.txtSearch.TabIndex = 5;
            this.txtSearch.Tag = "DataSearch";
            this.txtSearch.Visible = false;

            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(621, 64);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(120, 40);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Tag = "DataSearchBtn";
            this.btnSearch.Text = "Найти";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Visible = false;
            this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);

            this.dgvData.AllowUserToAddRows = false;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(45)))));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.dgvData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvData.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))));
            this.dgvData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(60)))));
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.dgvData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvData.DefaultCellStyle = dataGridViewCellStyle12;
            this.dgvData.EnableHeadersVisualStyles = false;
            this.dgvData.Location = new System.Drawing.Point(94, 140);
            this.dgvData.MultiSelect = false;
            this.dgvData.Name = "dgvData";
            this.dgvData.ReadOnly = true;
            this.dgvData.RowHeadersVisible = false;
            this.dgvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvData.Size = new System.Drawing.Size(1000, 300);
            this.dgvData.TabIndex = 7;
            this.dgvData.Tag = "DataGrid";
            this.dgvData.Visible = false;
            this.dgvData.SelectionChanged += new System.EventHandler(this.DgvData_SelectionChanged);

            this.btnDataAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnDataAdd.FlatAppearance.BorderSize = 0;
            this.btnDataAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDataAdd.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnDataAdd.ForeColor = System.Drawing.Color.White;
            this.btnDataAdd.Location = new System.Drawing.Point(144, 458);
            this.btnDataAdd.Name = "btnDataAdd";
            this.btnDataAdd.Size = new System.Drawing.Size(140, 40);
            this.btnDataAdd.TabIndex = 8;
            this.btnDataAdd.Tag = "DataAddBtn";
            this.btnDataAdd.Text = "Добавить";
            this.btnDataAdd.UseVisualStyleBackColor = false;
            this.btnDataAdd.Visible = false;
            this.btnDataAdd.Click += new System.EventHandler(this.BtnDataAdd_Click);

            this.btnDataUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(160)))), ((int)(((byte)(80)))));
            this.btnDataUpdate.Enabled = false;
            this.btnDataUpdate.FlatAppearance.BorderSize = 0;
            this.btnDataUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDataUpdate.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnDataUpdate.ForeColor = System.Drawing.Color.White;
            this.btnDataUpdate.Location = new System.Drawing.Point(300, 458);
            this.btnDataUpdate.Name = "btnDataUpdate";
            this.btnDataUpdate.Size = new System.Drawing.Size(140, 40);
            this.btnDataUpdate.TabIndex = 9;
            this.btnDataUpdate.Tag = "DataUpdateBtn";
            this.btnDataUpdate.Text = "Обновить";
            this.btnDataUpdate.UseVisualStyleBackColor = false;
            this.btnDataUpdate.Visible = false;
            this.btnDataUpdate.Click += new System.EventHandler(this.BtnDataUpdate_Click);

            this.btnDataDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnDataDelete.Enabled = false;
            this.btnDataDelete.FlatAppearance.BorderSize = 0;
            this.btnDataDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDataDelete.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnDataDelete.ForeColor = System.Drawing.Color.White;
            this.btnDataDelete.Location = new System.Drawing.Point(460, 458);
            this.btnDataDelete.Name = "btnDataDelete";
            this.btnDataDelete.Size = new System.Drawing.Size(140, 40);
            this.btnDataDelete.TabIndex = 10;
            this.btnDataDelete.Tag = "DataDeleteBtn";
            this.btnDataDelete.Text = "Удалить";
            this.btnDataDelete.UseVisualStyleBackColor = false;
            this.btnDataDelete.Visible = false;
            this.btnDataDelete.Click += new System.EventHandler(this.BtnDataDelete_Click);

            // Пагинация для данных — под кнопками действий
            this.btnPrevPage = new Button();
            this.btnPrevPage.Text = "← Назад";
            this.btnPrevPage.Size = new Size(100, 35);
            this.btnPrevPage.Location = new Point(144, 508);
            this.btnPrevPage.BackColor = Color.FromArgb(70, 130, 180);
            this.btnPrevPage.ForeColor = Color.White;
            this.btnPrevPage.FlatStyle = FlatStyle.Flat;
            this.btnPrevPage.FlatAppearance.BorderSize = 0;
            this.btnPrevPage.Visible = false;
            this.btnPrevPage.Click += new System.EventHandler(this.BtnPrevPage_Click);
            this.contentArea.Controls.Add(this.btnPrevPage);

            this.btnNextPage = new Button();
            this.btnNextPage.Text = "Вперёд →";
            this.btnNextPage.Size = new Size(100, 35);
            this.btnNextPage.Location = new Point(254, 508);
            this.btnNextPage.BackColor = Color.FromArgb(70, 130, 180);
            this.btnNextPage.ForeColor = Color.White;
            this.btnNextPage.FlatStyle = FlatStyle.Flat;
            this.btnNextPage.FlatAppearance.BorderSize = 0;
            this.btnNextPage.Visible = false;
            this.btnNextPage.Click += new System.EventHandler(this.BtnNextPage_Click);
            this.contentArea.Controls.Add(this.btnNextPage);

            this.lblPageInfo = new Label();
            this.lblPageInfo.Text = "Страница 1 из 1";
            this.lblPageInfo.Location = new Point(374, 515);
            this.lblPageInfo.AutoSize = true;
            this.lblPageInfo.ForeColor = Color.WhiteSmoke;
            this.lblPageInfo.Font = new Font("Segoe UI", 11);
            this.lblPageInfo.Visible = false;
            this.contentArea.Controls.Add(this.lblPageInfo);

            this.lblReportTitle.AutoSize = true;
            this.lblReportTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblReportTitle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblReportTitle.Location = new System.Drawing.Point(94, 20);
            this.lblReportTitle.Name = "lblReportTitle";
            this.lblReportTitle.Size = new System.Drawing.Size(337, 37);
            this.lblReportTitle.TabIndex = 11;
            this.lblReportTitle.Tag = "ReportTitle";
            this.lblReportTitle.Text = "Экспорт данных в отчёт";
            this.lblReportTitle.Visible = false;

            this.lblSelectTable.AutoSize = true;
            this.lblSelectTable.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblSelectTable.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblSelectTable.Location = new System.Drawing.Point(94, 77);
            this.lblSelectTable.Name = "lblSelectTable";
            this.lblSelectTable.Size = new System.Drawing.Size(146, 21);
            this.lblSelectTable.TabIndex = 12;
            this.lblSelectTable.Tag = "ReportLabel";
            this.lblSelectTable.Text = "Выберите таблицу:";
            this.lblSelectTable.Visible = false;

            this.cmbReportTables.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(60)))));
            this.cmbReportTables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReportTables.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbReportTables.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.cmbReportTables.Location = new System.Drawing.Point(250, 77);
            this.cmbReportTables.Name = "cmbReportTables";
            this.cmbReportTables.Size = new System.Drawing.Size(300, 21);
            this.cmbReportTables.TabIndex = 13;
            this.cmbReportTables.Tag = "ReportCombo";
            this.cmbReportTables.Visible = false;

            this.btnWord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnWord.FlatAppearance.BorderSize = 0;
            this.btnWord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWord.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnWord.ForeColor = System.Drawing.Color.White;
            this.btnWord.Location = new System.Drawing.Point(94, 140);
            this.btnWord.Name = "btnWord";
            this.btnWord.Size = new System.Drawing.Size(200, 50);
            this.btnWord.TabIndex = 14;
            this.btnWord.Tag = "ReportWord";
            this.btnWord.Text = "Экспорт в Word";
            this.btnWord.UseVisualStyleBackColor = false;
            this.btnWord.Visible = false;
            this.btnWord.Click += new System.EventHandler(this.BtnWord_Click);

            this.btnExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(100)))));
            this.btnExcel.FlatAppearance.BorderSize = 0;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnExcel.ForeColor = System.Drawing.Color.White;
            this.btnExcel.Location = new System.Drawing.Point(310, 140);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(200, 50);
            this.btnExcel.TabIndex = 15;
            this.btnExcel.Tag = "ReportExcel";
            this.btnExcel.Text = "Экспорт в Excel";
            this.btnExcel.UseVisualStyleBackColor = false;
            this.btnExcel.Visible = false;
            this.btnExcel.Click += new System.EventHandler(this.BtnExcel_Click);

            this.lblReportInfo.ForeColor = System.Drawing.Color.LightGray;
            this.lblReportInfo.Location = new System.Drawing.Point(94, 210);
            this.lblReportInfo.Name = "lblReportInfo";
            this.lblReportInfo.Size = new System.Drawing.Size(600, 60);
            this.lblReportInfo.TabIndex = 16;
            this.lblReportInfo.Tag = "ReportInfo";
            this.lblReportInfo.Visible = false;

            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(25)))), ((int)(((byte)(35)))));
            this.ClientSize = new System.Drawing.Size(1264, 761);
            this.Controls.Add(this.contentArea);
            this.Controls.Add(this.sidebar);
            this.MinimumSize = new System.Drawing.Size(1000, 700);
            this.Name = "MainWorkingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Учёт материалов и продукции — Главная";
            this.Load += new System.EventHandler(this.MainWorkingForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainWorkingForm_Paint);
            this.sidebar.ResumeLayout(false);
            this.sidebar.PerformLayout();
            this.contentArea.ResumeLayout(false);
            this.contentArea.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDirectories)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);
        }
    }
}