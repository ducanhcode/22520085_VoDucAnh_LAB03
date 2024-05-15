using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace _22520085_VoDucAnh_LAB03
{
    public partial class BAI03_client : Form
    {
        private TcpClient tcpClient = new TcpClient();
        private NetworkStream ns;
        public BAI03_client()
        {
            InitializeComponent();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            ns.Close();
            tcpClient.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ns = tcpClient.GetStream();

            // Gửi dữ liệu đến Server
            Byte[] data = System.Text.Encoding.UTF8.GetBytes(textBox3.Text);
            ns.Write(data, 0, data.Length);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Tạo đối tượng TcpClient
            tcpClient = new TcpClient();

            // Kết nối đến Server với 1 địa chỉ Ip và Port xác định
            IPAddress ipAddress = IPAddress.Parse(textBox1.Text);
            int port = int.Parse(textBox2.Text);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);

            try
            {
                tcpClient.Connect(ipEndPoint);
                MessageBox.Show("Kết nối thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi kết nối đến máy chủ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
