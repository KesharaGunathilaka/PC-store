using Guna.UI2.WinForms;
using System;
using System.Collections;
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
    public partial class frmProductAdd : SampleView
    {
        public frmProductAdd()
        {
            InitializeComponent();
        }

        public int id = 0;
        public int cID = 0;

        private void frmProductAdd_Load(object sender, EventArgs e)
        {
            string qry = "Select catID 'id',catName 'name' from category";

            MainClass.CBFill(qry, cbCat);

            if(cID > 0)
            {
                cbCat.SelectedValue = cID;
            }

            if(id >0)
            {
                ForUpdateLoadData();
            }
        }

        string filePath;
        Byte[] imageByteArray;
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images(.jpg,.png)|* .png; *.jpg";
            if (ofd.ShowDialog() == DialogResult.OK )
            {
                filePath = ofd.FileName;
                txtImage.Image = new Bitmap(filePath);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                guna2MessageDialog1.Show("Please enter a Product Name.", "Validation Error");
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                return;
            }

            if (string.IsNullOrEmpty(txtPrice.Text.Trim()))
            {
                guna2MessageDialog1.Show("Please enter a Price.", "Validation Error");
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                return;
            }

            if (string.IsNullOrEmpty(txtBrand.Text.Trim()))
            {
                guna2MessageDialog1.Show("Please enter a product Brand.", "Validation Error");
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                return;
            }

            if (string.IsNullOrEmpty(txtDetails.Text.Trim()))
            {
                guna2MessageDialog1.Show("Please enter Product Details.", "Validation Error");
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                return;
            }

            if (cbCat.SelectedIndex == -1)
            {
                guna2MessageDialog1.Show("Please Select a Category.", "Validation Error");
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                return;
            }

            if (cbAvailability.SelectedItem == null)
            {
                guna2MessageDialog1.Show("Please Select a Availability.", "Validation Error");
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                return;
            }

            string qry = "";

                if (id == 0) //Insert
                {
                    qry = "Insert into products Values(@Name,@price,@cat,@brand,@detail,@ava,@img)";
                }
                else //Update
                {
                    qry = "Update products Set pName=@Name, pPrice=@price,CategoryID=@cat,pBrand=@brand,pDetail=@detail,pAvailability=@ava, pImage=@img where pID = @id ";
                }

            Image temp = new Bitmap(txtImage.Image);
            MemoryStream ms = new MemoryStream();
            temp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            imageByteArray = ms.ToArray();



                Hashtable ht = new Hashtable();
                ht.Add("@id", id);
                ht.Add("@Name", txtName.Text);
                
                ht.Add("@price", txtPrice.Text);
                ht.Add("@cat", Convert.ToInt32(cbCat.SelectedValue));
            ht.Add("@brand", txtBrand.Text);
            ht.Add("@detail", txtDetails.Text);
            ht.Add("@ava", cbAvailability.Text);
            ht.Add("@img", imageByteArray);


            if (MainClass.SQl(qry, ht) > 0)
                {
                    guna2MessageDialog1.Show("Saved Successfully","Done");
                    id = 0;
                cID = 0;
                    txtName.Text = "";
                
                txtPrice.Text = "";
                
                cbCat.SelectedIndex = 0;
                cbCat.SelectedIndex = -1;
                txtBrand.Text = "";
                txtDetails.Text = "";
                cbAvailability.SelectedIndex = -1;
                txtImage.Image = PCstore.Properties.Resources.productPic;



                    txtName.Focus();
                    this.Close();
                }
            
        }

        private void ForUpdateLoadData()
        {
            string qry = @"Select * from products where pid=" + id + "";
            SqlCommand cmd = new SqlCommand(qry,MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if(dt.Rows.Count > 0)
            {
                txtName.Text = dt.Rows[0]["pName"].ToString();
                txtPrice.Text = dt.Rows[0]["pPrice"].ToString();
                txtBrand.Text = dt.Rows[0]["pBrand"].ToString();
                txtDetails.Text = dt.Rows[0]["pDetail"].ToString();
                cbAvailability.Text = dt.Rows[0]["pAvailability"].ToString();

                Byte[] imageArray = (byte[])(dt.Rows[0]["pImage"]);
                byte[] imageByteArray = imageArray;
                txtImage.Image = Image.FromStream(new MemoryStream(imageArray));
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
