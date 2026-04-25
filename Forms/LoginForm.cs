using BCrypt.Net;
using MySqlConnector;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class LoginForm : Form
    {
        private string connectionString = Repositories.DatabaseConnection.ConnectionString;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "Заполните все поля";
                return;
            }

            lblError.Text = "Проверка...";
            lblError.ForeColor = Color.Yellow;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT password_hash, role FROM users WHERE login = @login";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@login", login);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedHash = reader.GetString("password_hash");
                                bool isValid = BCrypt.Net.BCrypt.Verify(password, storedHash);
                                if (isValid)
                                {
                                    this.Hide();
                                    MainWorkingForm mainForm = new MainWorkingForm();
                                    mainForm.ShowDialog();
                                    this.Close();
                                }
                                else
                                {
                                    lblError.Text = "Неверный пароль";
                                    lblError.ForeColor = Color.Red;
                                }
                            }
                            else
                            {
                                lblError.Text = "Пользователь не найден";
                                lblError.ForeColor = Color.Red;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Ошибка: " + ex.Message;
                lblError.ForeColor = Color.Red;
                MessageBox.Show("Детали ошибки:\n" + ex.ToString(), "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
        }
    }
}