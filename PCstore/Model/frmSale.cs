using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PCstore.Model
{
    public partial class frmSale : SampleView
    {
        public frmSale()
        {
            InitializeComponent();
        }

        public int sID = 0;

        
        private void frmSale_Load(object sender, EventArgs e)
        {
            string qry = "Select sid 'id',sname 'name' from stores";

            MainClass.CBFill(qry, cmbStore);

            if (sID > 0)
            {
                cmbStore.SelectedValue = sID;
            }

            {
                ProductPanel.Controls.Clear();
                LoadProducts();
            }
        }



            private void LoadProducts()
            {
                ProductPanel.Controls.Clear();
                string qry = @"Select * from products p inner join category c on c.catID = p.CategoryID where pName like '%" + txtSearch.Text + "%'";
                //string qry = @"Select * from products inner join category on catID = CategoryID";
                SqlCommand cmd = new SqlCommand(qry, MainClass.con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow item in dt.Rows)
                {
                    Byte[] imagearray = (byte[])item["pImage"];
                    byte[] imagebytearray = imagearray;


                    AddItems(item["pID"].ToString(), item["pBrand"].ToString() + " " + item["pName"].ToString(), item["catName"].ToString(),
                        item["pPrice"].ToString(), Image.FromStream(new MemoryStream(imagearray)), item["pAvailability"].ToString());
                }
            }

        private void AddItems(string id, string name, string cat, string price, Image pImage, string ava)
        {
            var w = new ucProduct()
            {
                PName = name,
                PPrice = price,
                PCategory = cat,
                PImage = pImage,
                PAva = ava,

                //PAva = availability,
                id = Convert.ToInt32(id)
            };

            ProductPanel.Controls.Add(w);
            w.onSelect += (ss, ee) =>
            {
                var wdg = (ucProduct)ss;

                foreach (DataGridViewRow item in guna2DataGridView1.Rows)
                {
                    if (Convert.ToInt32(item.Cells["dgvid"].Value) == wdg.id)
                    {
                        item.Cells["dgvQty"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) + 1;
                        item.Cells["dgvAmount"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) *
                        double.Parse(item.Cells["dgvPrice"].Value.ToString());
                        // item.Cells["dgvCost"].Value = (int.Parse(item.Cells["dgvQty"].Value.ToString())) *
                        // double.Parse(wdg.pCost);
                        GetTotal();
                        return;
                    }  
                }
                guna2DataGridView1.Rows.Add(new object[] { 0, wdg.id, wdg.PName, 1, wdg.PPrice, wdg.PPrice });
                GetTotal();
            };
        }

            private void GetTotal()
            {
                 Double tot = 0;
                lblTotal.Text = "00";
                foreach (DataGridViewRow row in guna2DataGridView1.Rows) 
                {
                    tot += double.Parse(row.Cells["dgvAmount"].Value.ToString());
                }
                lblTotal.Text = tot.ToString("N0");
            }

        

        

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.PName.ToLower().Contains(txtSearch.Text.Trim().ToLower());
            }
        }

        private void guna2DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int count = 0;

            foreach(DataGridViewRow row in guna2DataGridView1.Rows) 
            {
                count++;
                row.Cells[0].Value = count;
            }
        }

        private void txtRec_TextChanged(object sender, EventArgs e)
        {
            double amt = 0;
            double rec = 0;
            double.TryParse(lblTotal.Text, out amt);
            double.TryParse(txtRec.Text, out rec);

            double change = rec - amt;
            txtChange.Text = change.ToString("N0");
        }

       


        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCustomer.Text.Trim()))
            {
                guna2MessageDialog1.Show("Please enter a Customer Name.", "Validation Error");
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                return;
            }

            if (string.IsNullOrEmpty(txtPhone.Text.Trim()))
            {
                guna2MessageDialog1.Show("Please enter a Customer Contact.", "Validation Error");
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                return;
            }

            if (cmbStore.SelectedIndex == -1)
            {
                guna2MessageDialog1.Show("Please Select a Store.", "Validation Error");
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                return;
            }

            if (cmbPay.SelectedItem == null)
            {
                guna2MessageDialog1.Show("Please Select a Payment Method.", "Validation Error");
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                return;
            }

            string qrysale = "";
            //int r;

            MainClass.con.Open();

            //r = Convert.ToInt32(cmd.ExecuteScalar());


            qrysale = @"insert into sale values (@adata,@CustomerName,@CustomerContact,@atotal,@pID,@paymeth,@store,@qty,@price)";
            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                SqlCommand cmd = new SqlCommand(qrysale, MainClass.con);
                cmd.Parameters.AddWithValue("@adata", DateTime.Now.Date);
                cmd.Parameters.AddWithValue("@CustomerName", txtCustomer.Text);
                cmd.Parameters.AddWithValue("@CustomerContact", txtPhone.Text);
                cmd.Parameters.AddWithValue("@atotal", Convert.ToDouble(lblTotal.Text));
                cmd.Parameters.AddWithValue("@pID", row.Cells["dgvName"].Value.ToString());
                cmd.Parameters.AddWithValue("@paymeth", cmbPay.Text);
                cmd.Parameters.AddWithValue("@store", cmbStore.Text);
                cmd.Parameters.AddWithValue("@qty", Convert.ToInt32(row.Cells["dgvQty"].Value));
                cmd.Parameters.AddWithValue("@price", Convert.ToDouble(row.Cells["dgvPrice"].Value));

                cmd.ExecuteNonQuery();

            }
            MainClass.con.Close();
            guna2DataGridView1.Rows.Clear();
            lblTotal.Text = "00";
            txtChange.Text = "00";
            txtRec.Text = "00";
            txtCustomer.Text = "";
            txtPhone.Text = "";
            cmbPay.SelectedIndex = -1;
            cmbStore.SelectedIndex = 0;
            cmbStore.SelectedIndex = -1;

            guna2MessageDialog1.Show("Payment Successful", "Done");
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;

        }

        private void btnClear_Click_1(object sender, EventArgs e)
        {
            guna2DataGridView1.Rows.Clear();
            lblTotal.Text = "00";
            txtChange.Text = "00";
            txtRec.Text = "00";
            txtCustomer.Text = "";
            txtPhone.Text = "";
            cmbPay.SelectedIndex = -1;
            cmbStore.SelectedIndex = 0;
            cmbStore.SelectedIndex = -1;
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}