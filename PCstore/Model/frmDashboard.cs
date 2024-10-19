using Guna.UI2.WinForms;
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

namespace PCstore.Model
{
    public partial class frmDashboard : SampleView
    {
        public frmDashboard()
        {
            InitializeComponent();
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            //guna2DataGridView1.BorderStyle = BorderStyle.FixedSingle;
            
            AddCategory();

            ProductPanel.Controls.Clear();
           

            //price range
            string qry = @"Select max(pPrice) 'Max' from products";
            DataTable dt = MainClass.GetData(qry);


            lblMax.Text = Convert.ToDouble(dt.Rows[0][0].ToString()).ToString("N0");



            TrackBar1.Maximum = Convert.ToInt32(dt.Rows[0][0].ToString()) / 2;
            TrackBar2.Maximum = Convert.ToInt32(dt.Rows[0][0].ToString());
            TrackBar2.Minimum = Convert.ToInt32(dt.Rows[0][0].ToString()) / 2;


            TrackBar2.Value = Convert.ToInt32(dt.Rows[0][0].ToString());
            LoadProducts();
        }
        

        private void AddCategory()
        {
            string qry = "Select * from Category";
            SqlCommand cmd = new SqlCommand(qry,MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);


            BrandPanel.Controls.Clear();

            if(dt.Rows.Count > 0 )
            {
                foreach (DataRow row in dt.Rows)
                {
                    Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                    b.FillColor = Color.FromArgb(3, 1, 32);
                    b.Size = new Size(150, 50);
                    b.BorderThickness = 2;
                    b.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                    b.BorderColor = Color.Gold;
                    b.BorderRadius = 2;
                    b.Font = new Font("Calibri", 11, FontStyle.Bold);
                    // b.AutoSize = true;
                    b.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                    b.Text = row["catName"].ToString();
                    b.ForeColor = Color.Gold;
                    b.Click += new EventHandler(b_Click);

                    BrandPanel.Controls.Add(b);
                }
            }
        }

        private void b_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;
            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.PCategory.ToLower().Contains(b.Text.Trim().ToLower());
            }
        }

        private void LoadProducts()
        {
            ProductPanel.Controls.Clear();
            string qry = @"Select * from products p inner join category c on c.catID = p.CategoryID where pName like '%" + txtSearch.Text + "%' and pPrice" + " between " + Convert.ToDouble(lblMin.Text)+ " and " + Convert.ToDouble(lblMax.Text) + "  ";
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

        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            foreach (var item in ProductPanel.Controls) 
            {
            var pro = (ucProduct)item;
                pro.Visible = pro.PName.ToLower().Contains(txtSearch.Text.Trim().ToLower());
            }
        }

        private void TrackBar1_Scroll(object sender, ScrollEventArgs e)
        {
            lblMin.Text = Convert.ToDouble(TrackBar1.Value).ToString("N0");
            LoadProducts();
        }

        private void TrackBar2_Scroll(object sender, ScrollEventArgs e)
        {
            lblMax.Text = Convert.ToDouble(TrackBar2.Value).ToString("N0");
            LoadProducts();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
           foreach(Control cc in BrandPanel.Controls)
            {
                Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)cc;
                b.Checked = false;
            }
            
           TrackBar1.Value = 0;
           TrackBar2.Value = TrackBar2.Maximum;
            lblMin.Text = "0";
            lblMax.Text = TrackBar2.Maximum.ToString();
            LoadProducts();
        }
    }
}
