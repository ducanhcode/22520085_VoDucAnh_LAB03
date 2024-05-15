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
    public partial class LAB03_bai03 : Form
    {
        public LAB03_bai03()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BAI03_client clientbai03 = new BAI03_client();
            clientbai03.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BAI03_server serverbai03 = new BAI03_server();
            serverbai03.Show();
        }
    }
}
