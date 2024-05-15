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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LAB03_bai01 frombai01 = new LAB03_bai01();
            frombai01.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LAB03_bai02 frombai02 = new LAB03_bai02();
            frombai02.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LAB03_bai03 frombai03 = new LAB03_bai03();
            frombai03.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LAB03_bai05 frombai05 = new LAB03_bai05();
            frombai05.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LAB03_bai04 frombai04 = new LAB03_bai04();
            frombai04.Show();
        }
    }
}
