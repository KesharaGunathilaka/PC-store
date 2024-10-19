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

namespace PCstore.Model
{
    
    public partial class ucProduct : UserControl
    {
        public ucProduct()
        {
            InitializeComponent();
        }

        public EventHandler onSelect = null;

        public int id {  get; set; }

        

        public string PCategory { get; set; }

        public string PName
        {
            get { return btnProduct.Text; }
            set { btnProduct.Text = value; }
        }

        public Image PImage
        {
            get { return txtImage.Image; }
            set { txtImage.Image = value; }
        }

        
        public string PAva
        {

            get { return lblAva.Text; }
            set
            {
                lblAva.Text = value;

                // Change label color based on text
                if (value.Contains("In Stock"))
                {
                    lblAva.ForeColor = Color.Green;
                }
                
                else
                {
                    
                    lblAva.ForeColor = Color.Red;
                }
            }

        }

        public string PPrice
        {
            get { return lblBrand.Text; }
            set { lblBrand.Text = value; }
        }

        private void txtImage_Click(object sender, EventArgs e)
        {
            onSelect?.Invoke(this, e);
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            MainClass.BlurBackground(new frmDetails() { id = id });
          
            
        }
    }

}
