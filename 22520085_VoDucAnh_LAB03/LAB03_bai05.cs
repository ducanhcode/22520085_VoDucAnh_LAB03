using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _22520085_VoDucAnh_LAB03
{
    public partial class LAB03_bai05 : Form
    {
        public LAB03_bai05()
        {
            InitializeComponent();
        }
      
        private void button1_Click_1(object sender, EventArgs e)
        {
            BAI05_server serverbai05 = new BAI05_server();
            serverbai05.Show();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            BAI05_client clientbai05 = new BAI05_client();
            clientbai05.Show();
        }
    }
}
