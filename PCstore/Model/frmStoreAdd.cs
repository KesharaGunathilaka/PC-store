using Guna.UI2.WinForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCstore.Model
{
    public partial class frmStoreAdd : SampleView
    {
        public frmStoreAdd()
        {
            InitializeComponent();
        }

        public int id = 0;

        
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                guna2MessageDialog1.Show("Please enter a Store.", "Validation Error");
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                return; 
            }

            string qry = "";

            if (id == 0) //Insert
            {
                qry = "Insert into stores Values(@Name)";
            }
            else //Update
            {
                qry = "Update stores Set sname = @Name where sid = @id ";
            }

            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@Name", txtName.Text);

            if (MainClass.SQl(qry, ht) > 0)
            {
                guna2MessageDialog1.Show("Saved Successfully");
                id = 0;
                txtName.Text = "";
                txtName.Focus();
                this.Close();
            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
