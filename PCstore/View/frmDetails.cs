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

namespace PCstore.View
{


    public partial class frmDetails : SampleView
    {
        public frmDetails()
        {
            InitializeComponent();
        }

        public int id = 0;
        private void frmDetails_Load(object sender, EventArgs e)
        {
            if (id > 0)
            {
                string qry = @"Select * from products where pid=" + id + "";
                
                DataTable dt = MainClass.GetData(qry);
               

                foreach (DataRow row in dt.Rows)
                {
                    lblName.Text = row["pName"].ToString();
                    lblPrice.Text = row["pPrice"].ToString();
                    lblBrand.Text = row["pBrand"].ToString();
                    lblDetail.Text = row["pDetail"].ToString();
                    lblAva.Text = row["pAvailability"].ToString();

                    Byte[] imageArray = (byte[])(dt.Rows[0]["pImage"]);
                    byte[] imageByteArray = imageArray;
                    txtPic.Image = Image.FromStream(new MemoryStream(imageArray));
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
