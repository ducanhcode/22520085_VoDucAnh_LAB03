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
    public partial class LAB03_bai01 : Form
    {
        public LAB03_bai01()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BAI01_client client = new BAI01_client();
            client.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BAI01_server server = new BAI01_server();
            server.Show();
        }
    }
}
