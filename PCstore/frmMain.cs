using PCstore.Model;
using PCstore.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCstore
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        public void AddControls(Form f)
        {
            CenterPanel.Controls.Clear();
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            CenterPanel.Controls.Add(f);
            f.Show();

        }

        private void guna2ControlBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lblUser.Text = MainClass.USER;
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            AddControls(new frmSaleView());
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            AddControls(new frmDashboard());
        }

        private void btnCatogeries_Click(object sender, EventArgs e)
        {
            AddControls(new frmCategoryView());
        }

        private void btnStores_Click(object sender, EventArgs e)
        {
            AddControls(new frmStoreView());
        }

        private void btnItems_Click(object sender, EventArgs e)
        {
            AddControls(new frmProductView());
        }
    }
}
