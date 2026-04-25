using FontAwesome.Sharp;
using MySqlConnector;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Xceed.Words.NET;
using WindowsFormsApp1.Repositories;
using Xceed.Document.NET;

namespace WindowsFormsApp1
{
    public partial class MainWorkingForm : Form
    {
        private bool isMenuVisible = false;
        private Timer animationTimer;
        private int targetSidebarWidth = 0;
        private const int SIDEBAR_OPEN_WIDTH = 260;
        private const int SIDEBAR_CLOSED_WIDTH = 70;
        private const int ANIMATION_STEP = 20;
        private string connectionString = DatabaseConnection.ConnectionString;
        private string currentTab = "Справочники";

        private Control[] menuItems;

        private int? selectedDirectoryId = null;

        private string currentDataTable = null;
        private int? selectedDataId = null;

        private int currentPage = 1;
        private int totalPages = 1;
        private string lastSearch = "";
        private const int PageSize = 50;

        private int currentDirPage = 1;
        private int totalDirPages = 1;
        private const int DirPageSize = 10;

        private WorkshopRepository workshopRepo = new WorkshopRepository();

        public MainWorkingForm()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                HideAllContent();
                SetupUI();
                RefreshMenu();
                this.DoubleBuffered = true;
                animationTimer = new Timer { Interval = 10 };
                animationTimer.Tick += AnimationTimer_Tick;
                sidebar.Width = SIDEBAR_CLOSED_WIDTH;
                btnToggleMenu.Text = "→";
                UpdateMenuItemsVisibility();
                ShowTab("Справочники");
            }

            this.Text = "Учёт материалов и продукции";
            this.Size = new Size(1500, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(20, 25, 35);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(1000, 700);

            // Настройка прокрутки сайдбара – только вертикальная
            sidebar.AutoScroll = false;
            sidebar.HorizontalScroll.Enabled = false;
            sidebar.HorizontalScroll.Visible = false;
            sidebar.VerticalScroll.Enabled = true;
            sidebar.VerticalScroll.Visible = true;
        }

        private void MainWorkingForm_Paint(object sender, PaintEventArgs e)
        {
            using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(20, 25, 35),
                Color.FromArgb(35, 40, 55),
                System.Drawing.Drawing2D.LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private void SetupUI()
        {
            menuItems = new Control[8];
            menuItems[0] = CreateMenuIconButton("Справочники", IconChar.Book, 100, Color.FromArgb(70, 130, 180));
            menuItems[1] = CreateMenuIconButton("Готовая продукция", IconChar.Boxes, 170, Color.FromArgb(100, 180, 100));
            menuItems[2] = CreateMenuIconButton("Материалы", IconChar.Cubes, 240, Color.FromArgb(50, 150, 200));
            menuItems[3] = CreateMenuIconButton("Операции", IconChar.Industry, 310, Color.FromArgb(220, 160, 80));
            menuItems[4] = CreateMenuIconButton("Поставщики", IconChar.Truck, 380, Color.FromArgb(150, 100, 200));
            menuItems[5] = CreateMenuIconButton("Сотрудники", IconChar.Users, 450, Color.FromArgb(80, 180, 150));
            menuItems[6] = CreateMenuIconButton("Цеха", IconChar.Building, 520, Color.FromArgb(130, 100, 200));
            menuItems[7] = CreateMenuIconButton("Отчёты", IconChar.ChartLine, 590, Color.FromArgb(180, 100, 200));

            foreach (var item in menuItems)
            {
                sidebar.Controls.Add(item);
                ((IconButton)item).Click += MenuItem_Click;
            }
        }

        private void RefreshMenu()
        {
            var toRemove = new List<Control>();
            foreach (Control ctrl in sidebar.Controls)
            {
                if (ctrl is IconButton btn && btn.Tag != null)
                {
                    string tag = btn.Tag.ToString();
                    bool isSystem = (tag == "Справочники" || tag == "Готовая продукция" ||
                                     tag == "Материалы" || tag == "Операции" ||
                                     tag == "Поставщики" || tag == "Сотрудники" ||
                                     tag == "Цеха" || tag == "Отчёты");
                    if (!isSystem)
                        toRemove.Add(ctrl);
                }
            }
            foreach (var ctrl in toRemove)
            {
                sidebar.Controls.Remove(ctrl);
                ctrl.Dispose();
            }

            var dirRepo = new DirectoryRepository(connectionString);
            var allDirs = dirRepo.GetAll();

            int top = 660;
            foreach (var dir in allDirs)
            {
                if (dir.TableName == "products" || dir.TableName == "materials" ||
                    dir.TableName == "operations" || dir.TableName == "suppliers" ||
                    dir.TableName == "employees" || dir.TableName == "workshops" ||
                    dir.TableName == "users" || dir.TableName == "directories")
                    continue;

                IconChar icon = IconChar.Book;
                try { icon = (IconChar)Enum.Parse(typeof(IconChar), dir.IconName ?? "Book"); } catch { }

                var btn = CreateMenuIconButton(dir.Name, icon, top, Color.FromArgb(100, 150, 200));
                sidebar.Controls.Add(btn);
                btn.Click += MenuItem_Click;
                top += 70;
            }
        }

        private IconButton CreateMenuIconButton(string text, IconChar icon, int top, Color accentColor)
        {
            IconButton btn = new IconButton
            {
                IconChar = icon,
                IconColor = accentColor,
                IconSize = 28,
                IconFont = IconFont.Auto,
                Text = text,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                TextAlign = ContentAlignment.MiddleLeft,
                ImageAlign = ContentAlignment.MiddleRight,
                Padding = new Padding(40, 0, 30, 0),
                Size = new Size(240, 60),
                Location = new Point(10, top),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.WhiteSmoke,
                Font = new System.Drawing.Font("Segoe UI Semibold", 13F)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Tag = text;
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(50, 50, 65);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.Transparent;
            return btn;
        }

        private void HideAllContent()
        {
            foreach (Control ctrl in contentArea.Controls)
                ctrl.Visible = false;
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            var btn = sender as IconButton;
            if (btn != null)
            {
                string tabName = (string)btn.Tag;
                ShowTab(tabName);
            }
        }

        private void ShowTab(string tabName)
        {
            currentTab = tabName;
            HideAllContent();

            if (tabName == "Справочники")
            {
                dgvDirectories.Visible = true;
                btnDirAdd.Visible = true;
                btnDirUpdate.Visible = true;
                btnDirDelete.Visible = true;
                currentDirPage = 1;
                LoadDirectories();
            }
            else if (tabName == "Отчёты")
            {
                using (var form = new ReportsForm(connectionString))
                {
                    form.ShowDialog();
                }
            }
            else
            {
                lblDataTitle.Visible = true;
                txtSearch.Visible = true;
                btnSearch.Visible = true;
                dgvData.Visible = true;
                btnDataAdd.Visible = true;
                btnDataUpdate.Visible = true;
                btnDataDelete.Visible = true;

                var dirRepo = new DirectoryRepository(connectionString);
                var dir = dirRepo.GetAll().FirstOrDefault(d => d.Name == tabName);
                if (dir != null)
                    currentDataTable = dir.TableName;
                else if (tabName == "Готовая продукция") currentDataTable = "products";
                else if (tabName == "Материалы") currentDataTable = "materials";
                else if (tabName == "Операции") currentDataTable = "operations";
                else if (tabName == "Поставщики") currentDataTable = "suppliers";
                else if (tabName == "Сотрудники") currentDataTable = "employees";
                else if (tabName == "Цеха") currentDataTable = "workshops";

                lblDataTitle.Text = tabName;
                currentPage = 1;
                LoadDataPage();
            }
        }

        private void LoadDirectories()
        {
            dgvDirectories.Rows.Clear();
            dgvDirectories.Columns.Clear();
            dgvDirectories.Columns.Add("id", "ID");
            dgvDirectories.Columns.Add("name", "Название");
            dgvDirectories.Columns.Add("description", "Описание");
            dgvDirectories.Columns.Add("icon_name", "Иконка");
            dgvDirectories.Columns.Add("table_name", "Таблица");

            try
            {
                var dirRepo = new DirectoryRepository(connectionString);
                var allDirs = dirRepo.GetAll();
                int total = allDirs.Count;
                totalDirPages = (int)Math.Ceiling(total / (double)DirPageSize);

                var pageDirs = allDirs.Skip((currentDirPage - 1) * DirPageSize).Take(DirPageSize);

                foreach (var d in pageDirs)
                {
                    dgvDirectories.Rows.Add(d.Id, d.Name, d.Description, d.IconName, d.TableName);
                }

                UpdateDirPaginationControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки справочников: " + ex.Message);
            }
            selectedDirectoryId = null;
            btnDirUpdate.Enabled = false;
            btnDirDelete.Enabled = false;
        }

        private void UpdateDirPaginationControls()
        {
            btnDirPrevPage.Enabled = currentDirPage > 1;
            btnDirNextPage.Enabled = currentDirPage < totalDirPages;
            lblDirPageInfo.Text = $"Страница {currentDirPage} из {(totalDirPages > 0 ? totalDirPages : 1)}";
            btnDirPrevPage.Visible = true;
            btnDirNextPage.Visible = true;
            lblDirPageInfo.Visible = true;
        }

        private void BtnDirPrevPage_Click(object sender, EventArgs e)
        {
            if (currentDirPage > 1)
            {
                currentDirPage--;
                LoadDirectories();
            }
        }

        private void BtnDirNextPage_Click(object sender, EventArgs e)
        {
            if (currentDirPage < totalDirPages)
            {
                currentDirPage++;
                LoadDirectories();
            }
        }

        private void DgvDirectories_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDirectories.SelectedRows.Count > 0)
            {
                selectedDirectoryId = Convert.ToInt32(dgvDirectories.SelectedRows[0].Cells["id"].Value);
                btnDirUpdate.Enabled = true;
                btnDirDelete.Enabled = true;
            }
            else
            {
                selectedDirectoryId = null;
                btnDirUpdate.Enabled = false;
                btnDirDelete.Enabled = false;
            }
        }

        private void BtnDirAdd_Click(object sender, EventArgs e)
        {
            using (var form = new AddDirectoryForm(connectionString))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadDirectories();
                    RefreshMenu();
                }
            }
        }

        private void BtnDirUpdate_Click(object sender, EventArgs e)
        {
            if (selectedDirectoryId == null) return;
            using (var form = new AddDirectoryForm(connectionString, selectedDirectoryId.Value))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadDirectories();
                    RefreshMenu();
                }
            }
        }

        private void BtnDirDelete_Click(object sender, EventArgs e)
        {
            if (selectedDirectoryId == null) return;
            var dirRepo = new DirectoryRepository(connectionString);
            var item = dirRepo.GetById(selectedDirectoryId.Value);
            if (item == null) return;

            using (var form = new DeleteDirectoryForm(connectionString, selectedDirectoryId.Value, item.Name))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadDirectories();
                    RefreshMenu();
                }
            }
        }

        private void LoadDataPage(string search = "")
        {
            try
            {
                if (currentDataTable == "workshops")
                {
                    LoadWorkshopsPage(search);
                    return;
                }

                if (currentDataTable == "materials")
                {
                    LoadGroupedMaterialsPage(search);
                    return;
                }

                var dynRepo = new DynamicTableRepository(connectionString);
                System.Data.DataTable dt;
                int total;

                if (string.IsNullOrEmpty(search))
                {
                    dt = dynRepo.GetPage(currentDataTable, currentPage, PageSize);
                    total = dynRepo.GetTotalCount(currentDataTable);
                }
                else
                {
                    var columns = dynRepo.GetColumns(currentDataTable);
                    dt = dynRepo.SearchPage(currentDataTable, search, currentPage, PageSize, columns);
                    total = dynRepo.GetSearchTotalCount(currentDataTable, search, columns);
                }

                lastSearch = search;
                totalPages = (int)Math.Ceiling(total / (double)PageSize);
                dgvData.DataSource = dt;

                if (dgvData.Columns.Contains("id"))
                    dgvData.Columns["id"].Visible = false;

                // Для каждой колонки получаем красивое название из COMMENT
                foreach (DataGridViewColumn col in dgvData.Columns)
                {
                    if (col.Name == "id") continue;

                    string comment = GetColumnComment(currentDataTable, col.Name);
                    string headerText;

                    if (!string.IsNullOrEmpty(comment))
                    {
                        headerText = comment;
                    }
                    else
                    {
                        // Убираем _id из названия если есть
                        string cleanName = col.Name;
                        if (cleanName.EndsWith("_id", StringComparison.OrdinalIgnoreCase))
                            cleanName = cleanName.Substring(0, cleanName.Length - 3);
                        headerText = cleanName.Replace("_", " ");
                    }

                    col.HeaderText = headerText;
                }

                UpdatePaginationControls();
                selectedDataId = null;
                btnDataUpdate.Enabled = false;
                btnDataDelete.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }
        private string GetColumnComment(string tableName, string columnName)
        {
            try
            {
                using (var conn = new MySqlConnector.MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = @"SELECT COLUMN_COMMENT FROM INFORMATION_SCHEMA.COLUMNS 
                           WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @table AND COLUMN_NAME = @column";
                    using (var cmd = new MySqlConnector.MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@table", tableName);
                        cmd.Parameters.AddWithValue("@column", columnName);
                        var result = cmd.ExecuteScalar();
                        return result?.ToString() ?? "";
                    }
                }
            }
            catch { return ""; }
        }

        // Добавь этот новый метод в класс MainWorkingForm
        private string GetColumnCommentFromDb(string tableName, string columnName)
        {
            try
            {
                using (var conn = new MySqlConnector.MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = @"SELECT COLUMN_COMMENT FROM INFORMATION_SCHEMA.COLUMNS 
                           WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @table AND COLUMN_NAME = @column";
                    using (var cmd = new MySqlConnector.MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@table", tableName);
                        cmd.Parameters.AddWithValue("@column", columnName);
                        var result = cmd.ExecuteScalar();
                        return result?.ToString() ?? "";
                    }
                }
            }
            catch { return ""; }
        }
        // Добавь этот метод в класс MainWorkingForm
        //private string GetColumnComment(string tableName, string columnName)
        //{
        //    try
        //    {
        //        using (var conn = new MySqlConnector.MySqlConnection(connectionString))
        //        {
        //            conn.Open();
        //            string sql = @"SELECT COLUMN_COMMENT FROM INFORMATION_SCHEMA.COLUMNS 
        //                   WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @table AND COLUMN_NAME = @column";
        //            using (var cmd = new MySqlConnector.MySqlCommand(sql, conn))
        //            {
        //                cmd.Parameters.AddWithValue("@table", tableName);
        //                cmd.Parameters.AddWithValue("@column", columnName);
        //                var result = cmd.ExecuteScalar();
        //                return result?.ToString() ?? "";
        //            }
        //        }
        //    }
        //    catch { return ""; }
        //}

        private void LoadGroupedMaterialsPage(string search = "")
        {
            try
            {
                var materialRepo = new MaterialRepository();
                List<Material> list;

                if (string.IsNullOrEmpty(search))
                    list = materialRepo.GetPage(currentPage, PageSize);
                else
                    list = materialRepo.SearchPage(search, currentPage, PageSize);

                int total = string.IsNullOrEmpty(search) ? materialRepo.GetTotalCount() : materialRepo.GetSearchTotalCount(search);
                lastSearch = search;
                totalPages = (int)Math.Ceiling(total / (double)PageSize);

                var displayList = list.Select(m => new Material
                {
                    Id = m.Id,
                    Name = m.Name,
                    BatchNumber = m.BatchNumber,
                    ArrivalFrom = m.ArrivalFrom,
                    ConsumptionTo = m.ConsumptionTo,
                    FullBatchMass = m.FullBatchMass,
                    TotalMass = m.TotalMass,
                    Length = m.Length,
                    Quantity = m.Quantity,
                    ArrivalDate = m.ArrivalDate
                }).ToList();

                for (int i = 1; i < displayList.Count; i++)
                {
                    if (displayList[i].Name == displayList[i - 1].Name)
                        displayList[i].Name = "";
                    if (displayList[i].BatchNumber == displayList[i - 1].BatchNumber)
                        displayList[i].BatchNumber = "";
                    if (displayList[i].ArrivalFrom == displayList[i - 1].ArrivalFrom)
                        displayList[i].ArrivalFrom = "";
                    if (displayList[i].ConsumptionTo == displayList[i - 1].ConsumptionTo)
                        displayList[i].ConsumptionTo = "";
                }

                dgvData.DataSource = displayList;

                if (dgvData.Columns.Contains("Id")) dgvData.Columns["Id"].Visible = false;
                dgvData.Columns["Name"].HeaderText = "Название";
                dgvData.Columns["BatchNumber"].HeaderText = "Номер партии";
                dgvData.Columns["ArrivalFrom"].HeaderText = "Поставщик";
                dgvData.Columns["ConsumptionTo"].HeaderText = "Расход в";
                if (dgvData.Columns.Contains("FullBatchMass")) dgvData.Columns["FullBatchMass"].HeaderText = "Полная масса (кг)";
                if (dgvData.Columns.Contains("TotalMass")) dgvData.Columns["TotalMass"].HeaderText = "Общая масса (кг)";
                if (dgvData.Columns.Contains("Length")) dgvData.Columns["Length"].HeaderText = "Длина (м)";
                if (dgvData.Columns.Contains("Quantity")) dgvData.Columns["Quantity"].HeaderText = "Количество";
                if (dgvData.Columns.Contains("ArrivalDate")) dgvData.Columns["ArrivalDate"].HeaderText = "Дата поступления";

                UpdatePaginationControls();
                selectedDataId = null;
                btnDataUpdate.Enabled = false;
                btnDataDelete.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void LoadWorkshopsPage(string search = "")
        {
            List<Workshop> list;
            int total;
            if (string.IsNullOrEmpty(search))
            {
                list = workshopRepo.GetPage(currentPage, PageSize);
                total = workshopRepo.GetTotalCount();
            }
            else
            {
                list = workshopRepo.SearchPage(search, currentPage, PageSize);
                total = workshopRepo.GetSearchTotalCount(search);
            }
            lastSearch = search;
            totalPages = (int)Math.Ceiling(total / (double)PageSize);
            dgvData.DataSource = list;

            if (dgvData.Columns.Contains("Id")) dgvData.Columns["Id"].Visible = false;
            dgvData.Columns["Name"].HeaderText = "Название цеха";
            dgvData.Columns["Description"].HeaderText = "Описание";
            UpdatePaginationControls();
        }

        private void UpdatePaginationControls()
        {
            btnPrevPage.Enabled = currentPage > 1;
            btnNextPage.Enabled = currentPage < totalPages;
            lblPageInfo.Text = $"Страница {currentPage} из {(totalPages > 0 ? totalPages : 1)}";
            btnPrevPage.Visible = true;
            btnNextPage.Visible = true;
            lblPageInfo.Visible = true;
        }

        private void BtnPrevPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadDataPage(lastSearch);
            }
        }

        private void BtnNextPage_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadDataPage(lastSearch);
            }
        }

        private void DgvData_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count > 0)
            {
                selectedDataId = Convert.ToInt32(dgvData.SelectedRows[0].Cells["id"].Value);
                btnDataUpdate.Enabled = true;
                btnDataDelete.Enabled = true;
            }
            else
            {
                selectedDataId = null;
                btnDataUpdate.Enabled = false;
                btnDataDelete.Enabled = false;
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadDataPage(txtSearch.Text.Trim());
        }

        private void BtnDataAdd_Click(object sender, EventArgs e)
        {
            if (currentDataTable == "workshops")
            {
                using (var form = new AddEditWorkshopForm())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        currentPage = 1;
                        LoadDataPage(lastSearch);
                    }
                }
            }
            else
            {
                using (var form = new AddEditItemForm(connectionString, currentDataTable))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        currentPage = 1;
                        LoadDataPage(lastSearch);
                    }
                }
            }
        }

        private void BtnDataUpdate_Click(object sender, EventArgs e)
        {
            if (selectedDataId == null) return;
            if (currentDataTable == "workshops")
            {
                using (var form = new AddEditWorkshopForm(selectedDataId.Value))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                        LoadDataPage(lastSearch);
                }
            }
            else
            {
                using (var form = new AddEditItemForm(connectionString, currentDataTable, selectedDataId))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                        LoadDataPage(lastSearch);
                }
            }
        }

        private void BtnDataDelete_Click(object sender, EventArgs e)
        {
            if (selectedDataId == null) return;
            if (MessageBox.Show("Удалить выбранную запись?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    if (currentDataTable == "workshops")
                    {
                        workshopRepo.Delete(selectedDataId.Value);
                    }
                    else
                    {
                        var dynRepo = new DynamicTableRepository(connectionString);
                        dynRepo.Delete(currentDataTable, selectedDataId.Value);
                    }
                    LoadDataPage(lastSearch);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления: " + ex.Message);
                }
            }
        }

        private void MainWorkingForm_Load(object sender, EventArgs e)
        {
        }

        private void LoadReportTables()
        {
            cmbReportTables.Items.Clear();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(
                        "SELECT table_name FROM information_schema.tables " +
                        "WHERE table_schema = 'auto_production' " +
                        "AND table_name NOT IN ('users', 'directories') " +
                        "ORDER BY table_name", conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbReportTables.Items.Add(reader.GetString(0));
                        }
                    }
                }
                if (cmbReportTables.Items.Count > 0)
                    cmbReportTables.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки таблиц: " + ex.Message);
            }
        }

        private void BtnWord_Click(object sender, EventArgs e)
        {
            if (cmbReportTables.SelectedItem == null)
            {
                MessageBox.Show("Выберите таблицу");
                return;
            }
            string table = cmbReportTables.SelectedItem.ToString();
            ExportToWord(table);
        }

        private void BtnExcel_Click(object sender, EventArgs e)
        {
            if (cmbReportTables.SelectedItem == null)
            {
                MessageBox.Show("Выберите таблицу");
                return;
            }
            string table = cmbReportTables.SelectedItem.ToString();
            ExportToExcel(table);
        }

        private void ExportToWord(string tableName)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Word файлы (*.docx)|*.docx";
                saveDialog.Title = "Сохранить отчёт в Word";
                saveDialog.FileName = $"Отчёт_{tableName}_{DateTime.Now:yyyy-MM-dd}.docx";
                if (saveDialog.ShowDialog() != DialogResult.OK) return;

                try
                {
                    using (var document = DocX.Create(saveDialog.FileName))
                    {
                        document.InsertParagraph($"Отчёт по таблице: {tableName}")
                                .FontSize(20).Bold().Alignment = Alignment.center;
                        document.InsertParagraph($"Дата создания: {DateTime.Now:dd.MM.yyyy HH:mm}")
                                .Alignment = Alignment.center;
                        document.InsertParagraph("");

                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();
                            using (MySqlCommand cmd = new MySqlCommand($"SELECT * FROM `{tableName}`", conn))
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    int colCount = reader.FieldCount;
                                    var wordTable = document.AddTable(1, colCount);
                                    for (int i = 0; i < colCount; i++)
                                        wordTable.Rows[0].Cells[i].Paragraphs.First().Append(reader.GetName(i)).Bold();
                                    while (reader.Read())
                                    {
                                        var row = wordTable.InsertRow();
                                        for (int i = 0; i < colCount; i++)
                                            row.Cells[i].Paragraphs.First().Append(reader[i]?.ToString() ?? "");
                                    }
                                    document.InsertTable(wordTable);
                                }
                                else
                                {
                                    document.InsertParagraph("Таблица пуста");
                                }
                            }
                        }
                        document.Save();
                    }
                    MessageBox.Show("Отчёт успешно сохранён в Word!");
                    System.Diagnostics.Process.Start(saveDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка экспорта в Word: " + ex.Message);
                }
            }
        }

        private void ExportToExcel(string tableName)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel файлы (*.xlsx)|*.xlsx";
                saveDialog.Title = "Сохранить отчёт в Excel";
                saveDialog.FileName = $"Отчёт_{tableName}_{DateTime.Now:yyyy-MM-dd}.xlsx";
                if (saveDialog.ShowDialog() != DialogResult.OK) return;

                try
                {
                    using (var package = new ExcelPackage(new FileInfo(saveDialog.FileName)))
                    {
                        var ws = package.Workbook.Worksheets.Add(tableName);
                        ws.Cells[1, 1].Value = $"Отчёт по таблице: {tableName}";
                        ws.Cells[1, 1, 1, 10].Merge = true;
                        ws.Cells[1, 1].Style.Font.Size = 20;
                        ws.Cells[1, 1].Style.Font.Bold = true;
                        ws.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[2, 1].Value = $"Дата создания: {DateTime.Now:dd.MM.yyyy HH:mm}";
                        ws.Cells[2, 1, 2, 10].Merge = true;
                        ws.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();
                            using (MySqlCommand cmd = new MySqlCommand($"SELECT * FROM `{tableName}`", conn))
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        ws.Cells[4, i + 1].Value = reader.GetName(i);
                                        ws.Cells[4, i + 1].Style.Font.Bold = true;
                                        ws.Cells[4, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[4, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(70, 130, 180));
                                        ws.Cells[4, i + 1].Style.Font.Color.SetColor(Color.White);
                                    }
                                    int row = 5;
                                    while (reader.Read())
                                    {
                                        for (int i = 0; i < reader.FieldCount; i++)
                                            ws.Cells[row, i + 1].Value = reader[i]?.ToString();
                                        row++;
                                    }
                                }
                                else
                                {
                                    ws.Cells[4, 1].Value = "Таблица пуста";
                                }
                            }
                        }
                        ws.Cells[ws.Dimension.Address].AutoFitColumns();
                        package.Save();
                    }
                    MessageBox.Show("Отчёт успешно сохранён в Excel!");
                    System.Diagnostics.Process.Start(saveDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка экспорта в Excel: " + ex.Message);
                }
            }
        }

        private void BtnToggleMenu_Click(object sender, EventArgs e)
        {
            if (isMenuVisible)
            {
                targetSidebarWidth = SIDEBAR_CLOSED_WIDTH;
                btnToggleMenu.Text = "→";
            }
            else
            {
                targetSidebarWidth = SIDEBAR_OPEN_WIDTH;
                btnToggleMenu.Text = "≡";
            }
            isMenuVisible = !isMenuVisible;
            animationTimer.Start();
            UpdateMenuItemsVisibility();
            contentArea.Invalidate(true);
            this.PerformLayout();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            int currentWidth = sidebar.Width;
            int step = (targetSidebarWidth > currentWidth) ? ANIMATION_STEP : -ANIMATION_STEP;
            if (Math.Abs(targetSidebarWidth - currentWidth) <= Math.Abs(step))
            {
                sidebar.Width = targetSidebarWidth;
                animationTimer.Stop();
                contentArea.Invalidate(true);
                this.PerformLayout();
            }
            else
            {
                sidebar.Width += step;
            }
            if (sidebar.Controls.Count > 1)
                sidebar.Controls[1].Visible = isMenuVisible;
        }

        private void UpdateMenuItemsVisibility()
        {
            foreach (var item in menuItems)
            {
                var btn = item as IconButton;
                if (btn != null)
                {
                    if (isMenuVisible)
                    {
                        btn.Text = (string)btn.Tag;
                        btn.Size = new Size(240, 60);
                        btn.Padding = new Padding(40, 0, 30, 0);
                        btn.ImageAlign = ContentAlignment.MiddleRight;
                    }
                    else
                    {
                        btn.Text = "";
                        btn.Size = new Size(50, 50);
                        btn.Location = new Point(10, btn.Location.Y);
                        btn.Padding = new Padding(0);
                        btn.ImageAlign = ContentAlignment.MiddleCenter;
                    }
                }
            }
        }

        private void contentArea_Paint(object sender, PaintEventArgs e) { }
        private void contentArea_Paint_1(object sender, PaintEventArgs e) { }
    }
}