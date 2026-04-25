using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class ConfirmDeleteForm : Form
    {
        public ConfirmDeleteForm(string itemName)
        {
            InitializeComponent();
            lblMessage.Text = $"Удалить запись '{itemName}'?";
        }

        private void BtnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void BtnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void ConfirmDeleteForm_Load(object sender, EventArgs e)
        {

        }
    }
}