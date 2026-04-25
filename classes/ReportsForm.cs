using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MySqlConnector;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class ReportsForm : Form
    {
        private string connectionString;

        public ReportsForm(string connString)
        {
            connectionString = connString;
            InitializeComponent();
            LoadTables();
        }

        private void LoadTables()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT table_name FROM information_schema.tables 
                                     WHERE table_schema = 'auto_production' 
                                     AND table_name NOT IN ('users', 'directories') 
                                     ORDER BY table_name";
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbTables.Items.Add(reader.GetString(0));
                        }
                    }
                }
                if (cmbTables.Items.Count > 0)
                    cmbTables.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки таблиц:\n" + ex.Message);
            }
        }

        private void BtnCreateReport_Click(object sender, EventArgs e)
        {
            if (cmbTables.SelectedItem == null)
            {
                MessageBox.Show("Выберите таблицу");
                return;
            }
            string table = cmbTables.SelectedItem.ToString();

            DialogResult result = MessageBox.Show(
                $"Создать отчёт по таблице '{table}'?\n\nДа = Excel\nНет = Word\nОтмена = выход",
                "Формат отчёта", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
                ExportToExcel(table);
            else if (result == DialogResult.No)
                ExportToWord(table);
        }

        private void BtnWord_Click(object sender, EventArgs e)
        {
            if (cmbTables.SelectedItem == null) return;
            ExportToWord(cmbTables.SelectedItem.ToString());
        }

        private void BtnExcel_Click(object sender, EventArgs e)
        {
            if (cmbTables.SelectedItem == null) return;
            ExportToExcel(cmbTables.SelectedItem.ToString());
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
                    using (var workbook = new XLWorkbook())
                    {
                        var ws = workbook.Worksheets.Add(tableName);

                        ws.Cell(1, 1).Value = $"Отчёт по таблице: {tableName}";
                        ws.Range(1, 1, 1, 10).Merge().Style
                          .Font.SetBold().Font.SetFontSize(20)
                          .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        ws.Cell(2, 1).Value = $"Дата создания: {DateTime.Now:dd.MM.yyyy HH:mm}";
                        ws.Range(2, 1, 2, 10).Merge().Style
                          .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        DataTable dt = new DataTable();
                        using (var conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();
                            using (var cmd = new MySqlCommand($"SELECT * FROM `{tableName}` ORDER BY id", conn))
                            using (var reader = cmd.ExecuteReader())
                            {
                                dt.Load(reader);
                            }
                        }

                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                var cell = ws.Cell(4, i + 1);
                                cell.Value = dt.Columns[i].ColumnName;
                                cell.Style.Font.SetBold()
                                    .Fill.SetBackgroundColor(XLColor.FromArgb(70, 130, 180))
                                    .Font.SetFontColor(XLColor.White);
                            }

                            ws.Cell(5, 1).InsertData(dt.AsEnumerable());
                        }
                        else
                        {
                            ws.Cell(4, 1).Value = "Таблица пуста";
                        }

                        ws.Columns().AdjustToContents();
                        workbook.SaveAs(saveDialog.FileName);
                    }

                    MessageBox.Show("Отчёт успешно сохранён в Excel!");
                    System.Diagnostics.Process.Start(saveDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка экспорта в Excel:\n" + ex.Message);
                }
            }
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
                    using (WordprocessingDocument doc = WordprocessingDocument.Create(saveDialog.FileName, WordprocessingDocumentType.Document))
                    {
                        MainDocumentPart mainPart = doc.AddMainDocumentPart();
                        mainPart.Document = new Document();
                        Body body = mainPart.Document.AppendChild(new Body());

                        Paragraph titlePara = body.AppendChild(new Paragraph());
                        Run titleRun = titlePara.AppendChild(new Run());
                        titleRun.AppendChild(new Text($"Отчёт по таблице: {tableName}"));
                        titleRun.RunProperties = new RunProperties(
                            new Bold(),
                            new FontSize() { Val = "48" }
                        );
                        titlePara.ParagraphProperties = new ParagraphProperties(
                            new Justification() { Val = JustificationValues.Center }
                        );

                        Paragraph datePara = body.AppendChild(new Paragraph());
                        Run dateRun = datePara.AppendChild(new Run());
                        dateRun.AppendChild(new Text($"Дата создания: {DateTime.Now:dd.MM.yyyy HH:mm}"));
                        datePara.ParagraphProperties = new ParagraphProperties(
                            new Justification() { Val = JustificationValues.Center }
                        );
                        body.AppendChild(new Paragraph()); // пустая строка

                        DataTable dt = new DataTable();
                        using (var conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();
                            using (var cmd = new MySqlCommand($"SELECT * FROM `{tableName}` ORDER BY id", conn))
                            using (var reader = cmd.ExecuteReader())
                            {
                                dt.Load(reader);
                            }
                        }

                        if (dt.Rows.Count > 0)
                        {
                            Table wordTable = body.AppendChild(new Table());
                            TableProperties tblProps = new TableProperties(
                                new TableBorders(
                                    new TopBorder() { Val = BorderValues.Single, Size = 1 },
                                    new BottomBorder() { Val = BorderValues.Single, Size = 1 },
                                    new LeftBorder() { Val = BorderValues.Single, Size = 1 },
                                    new RightBorder() { Val = BorderValues.Single, Size = 1 },
                                    new InsideHorizontalBorder() { Val = BorderValues.Single, Size = 1 },
                                    new InsideVerticalBorder() { Val = BorderValues.Single, Size = 1 }
                                )
                            );
                            wordTable.AppendChild(tblProps);

                            TableRow headerRow = wordTable.AppendChild(new TableRow());
                            foreach (DataColumn col in dt.Columns)
                            {
                                TableCell cell = headerRow.AppendChild(new TableCell());
                                Paragraph para = cell.AppendChild(new Paragraph());
                                Run run = para.AppendChild(new Run());
                                run.AppendChild(new Text(col.ColumnName));
                                run.RunProperties = new RunProperties(new Bold());
                            }

                           
                            foreach (DataRow row in dt.Rows)
                            {
                                TableRow dataRow = wordTable.AppendChild(new TableRow());
                                foreach (DataColumn col in dt.Columns)
                                {
                                    TableCell cell = dataRow.AppendChild(new TableCell());
                                    Paragraph para = cell.AppendChild(new Paragraph());
                                    para.AppendChild(new Run(new Text(row[col]?.ToString() ?? "")));
                                }
                            }
                        }
                        else
                        {
                            body.AppendChild(new Paragraph(new Run(new Text("Таблица пуста"))));
                        }
                    }

                    MessageBox.Show("Отчёт успешно сохранён в Word!");
                    System.Diagnostics.Process.Start(saveDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка экспорта в Word:\n" + ex.Message);
                }
            }
        }

        private void ReportsForm_Load(object sender, EventArgs e)
        {

        }
    }
}