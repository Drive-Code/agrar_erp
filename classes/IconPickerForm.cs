using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class IconPickerForm : Form
    {
        private ListBox lstIcons;
        private Button btnOK, btnCancel;
        public string SelectedIconName { get; private set; } = "Book";

        public IconPickerForm()
        {
            this.Text = "Выберите иконку";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(32, 32, 40);
            this.ForeColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 11);

            lstIcons = new ListBox();
            lstIcons.Location = new Point(20, 20);
            lstIcons.Size = new Size(340, 380);
            lstIcons.BackColor = Color.FromArgb(50, 50, 60);
            lstIcons.ForeColor = Color.WhiteSmoke;

            var icons = new List<string>
            {
                "Book", "Boxes", "BoxOpen", "Cubes", "Industry",
                "Truck", "Users", "ChartLine", "Building", "Cog",
                "ClipboardList", "Warehouse", "Wrench", "Tools", "Box"
            };
            foreach (var icon in icons)
                lstIcons.Items.Add(icon);

            this.Controls.Add(lstIcons);

            btnOK = new Button
            {
                Text = "OK",
                Size = new Size(100, 35),
                Location = new Point(120, 410),
                BackColor = Color.FromArgb(70, 130, 180),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.Click += (s, e) =>
            {
                if (lstIcons.SelectedItem != null)
                    SelectedIconName = lstIcons.SelectedItem.ToString();
                DialogResult = DialogResult.OK;
                Close();
            };
            this.Controls.Add(btnOK);

            btnCancel = new Button
            {
                Text = "Отмена",
                Size = new Size(100, 35),
                Location = new Point(240, 410),
                BackColor = Color.FromArgb(100, 100, 120),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };
            this.Controls.Add(btnCancel);
        }
    }
}