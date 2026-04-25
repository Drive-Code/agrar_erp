using System;
using System.Windows.Forms;
using WindowsFormsApp1.Repositories;

namespace WindowsFormsApp1
{
    public partial class AddEditWorkshopForm : Form
    {
        private int? workshopId;
        private WorkshopRepository repo;

        public AddEditWorkshopForm(int? id = null)
        {
            workshopId = id;
            repo = new WorkshopRepository();
            InitializeComponent();
            if (id.HasValue)
            {
                this.Text = "Редактирование цеха";
                LoadWorkshop();
            }
            else
            {
                this.Text = "Добавление цеха";
            }
        }

        private void LoadWorkshop()
        {
            var workshop = repo.GetById(workshopId.Value);
            if (workshop != null)
            {
                txtName.Text = workshop.Name;
                txtDescription.Text = workshop.Description ?? "";
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название цеха.");
                return;
            }

            var workshop = new Workshop
            {
                Id = workshopId ?? 0,
                Name = txtName.Text.Trim(),
                Description = txtDescription.Text.Trim()
            };

            try
            {
                if (workshopId.HasValue)
                    repo.Update(workshop);
                else
                    repo.Insert(workshop);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}