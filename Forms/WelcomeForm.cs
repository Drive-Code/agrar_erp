using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class WelcomeForm : Form
    {
        public WelcomeForm()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm login = new LoginForm();
            login.ShowDialog();
            this.Close();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void WelcomeForm_Paint(object sender, PaintEventArgs e)
        {
            using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(25, 25, 40),
                Color.FromArgb(50, 50, 70),
                System.Drawing.Drawing2D.LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private void PanelCenter_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddArc(0, 0, 20, 20, 180, 90);
                path.AddArc(panel.Width - 20, 0, 20, 20, 270, 90);
                path.AddArc(panel.Width - 20, panel.Height - 20, 20, 20, 0, 90);
                path.AddArc(0, panel.Height - 20, 20, 20, 90, 90);
                path.CloseFigure();
                panel.Region = new Region(path);
            }
            using (var brush = new SolidBrush(Color.FromArgb(55, 55, 70)))
            {
                e.Graphics.FillRectangle(brush, panel.ClientRectangle);
            }
        }

        //обработчики для кнопок
        private void BtnLogin_MouseEnter(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.FromArgb(90, 150, 220);
        }

        private void BtnLogin_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.FromArgb(70, 130, 180);
        }

        private void BtnExit_MouseEnter(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.FromArgb(220, 80, 80);
        }

        private void BtnExit_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.FromArgb(180, 60, 60);
        }

        private void WelcomeForm_Load(object sender, EventArgs e)
        {

        }
    }
}