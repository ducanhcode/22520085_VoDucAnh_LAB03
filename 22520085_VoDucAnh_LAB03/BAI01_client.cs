using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace _22520085_VoDucAnh_LAB03
{
    public partial class BAI01_client : Form
    {
        public BAI01_client()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            UdpClient udpClient = new UdpClient();
            IPAddress ipadd = IPAddress.Parse(txtIP.Text);
            int port = Convert.ToInt32(txtPort.Text);
            IPEndPoint ipend = new IPEndPoint(ipadd, port);
            string messageToSend = $"From {txtIP.Text}: {richTextBox1.Text}";
            Byte[] sendBytes = Encoding.UTF8.GetBytes(messageToSend);
            udpClient.Send(sendBytes, sendBytes.Length, ipend);
            richTextBox1.Clear();
        }
    }
}
