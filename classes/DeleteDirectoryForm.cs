using System;
using System.Windows.Forms;
using WindowsFormsApp1.Repositories;

namespace WindowsFormsApp1
{
    public partial class DeleteDirectoryForm : Form
    {
        private readonly string connectionString;
        private readonly int directoryId;
        private readonly string directoryName;
        private DirectoryRepository dirRepo;

        public DeleteDirectoryForm(string connString, int id, string name)
        {
            connectionString = connString;
            directoryId = id;
            directoryName = name;
            dirRepo = new DirectoryRepository(connectionString);
            InitializeComponent();
            lblMessage.Text = $"Вы уверены, что хотите удалить справочник \"{directoryName}\"?";
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var item = dirRepo.GetById(directoryId);
                if (item != null && !string.IsNullOrEmpty(item.TableName))
                {
                    using (var conn = new MySqlConnector.MySqlConnection(connectionString))
                    {
                        conn.Open();
                        using (var cmd = new MySqlConnector.MySqlCommand($"DROP TABLE IF EXISTS `{item.TableName}`", conn))
                            cmd.ExecuteNonQuery();
                    }
                }
                dirRepo.Delete(directoryId);
                MessageBox.Show("Справочник удалён!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления:\n" + ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void DeleteDirectoryForm_Load(object sender, EventArgs e)
        {

        }

        private void lblMessage_Click(object sender, EventArgs e)
        {

        }
    }
}