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
    public partial class LAB03_bai04 : Form
    {
        public LAB03_bai04()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BAI04_server serverbai04 = new BAI04_server();
            serverbai04.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BAI04_client clientbai04 = new BAI04_client();
            clientbai04.Show();
        }
    }
}
