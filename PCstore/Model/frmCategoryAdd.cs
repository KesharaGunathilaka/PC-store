﻿using System;
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
    public partial class frmCategoryAdd : SampleView
    {
        public frmCategoryAdd()
        {
            InitializeComponent();
        }

        public int id = 0;
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                guna2MessageDialog1.Show("Please enter a Category.", "Validation Error");
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                return; 
            }

            string qry = "";

            if(id==0) //Insert
            {
                qry = "Insert into category Values(@Name)";
            }
            else //Update
            {
                qry = "Update category Set catName = @Name where catID = @id ";
            }

            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@Name", txtName.Text);

            if (MainClass.SQl(qry, ht)>0)
            {
                guna2MessageDialog1.Show("Saved Successfully");
                    id = 0;
                txtName.Text = "";
                txtName.Focus();
                this.Close();
            }
        }
    }
}