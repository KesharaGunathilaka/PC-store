using PCstore.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCstore.View
{
    public partial class frmSaleView : SampleView
    {
        public frmSaleView()
        {
            InitializeComponent();
        }

        private void btnSale_Click(object sender, EventArgs e)
        {
            frmSale frm = new frmSale();
            frm.ShowDialog();
            GetData();
        }

        public void GetData()
        {
            
            string qry = "select saleID,adata,CustomerName,CustomerContact,store,paymeth,pID,Price from sale where CustomerName like '%" + txtSearch.Text + "%'";

            ListBox lb = new ListBox();
            lb.Items.Add(dgvid);
            lb.Items.Add(dgvDate);
            lb.Items.Add(dgvName);
            lb.Items.Add(dgvContact);
            lb.Items.Add(dgvStore);
            lb.Items.Add(dgvPay);
            lb.Items.Add(dgvItem);
            lb.Items.Add(dgvPrice);
            
            MainClass.LoadData(qry, guna2DataGridView1, lb);

        }

        private void frmSaleView_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            GetData();
        }


    }
}
