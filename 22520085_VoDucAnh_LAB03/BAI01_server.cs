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
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace _22520085_VoDucAnh_LAB03
{
    public partial class BAI01_server : Form
    {
        private Socket udpSocket;
        private bool isListening;
        private byte[] buffer = new byte[1024];
        public BAI01_server()
        {
            InitializeComponent();
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            if (isListening)
            {
                udpSocket.Close();
                isListening = false;
                btnListen.Text = "Start Listening";
            }
            else
            {
                StartListening();
                isListening = true;
                btnListen.Text = "Stop Listening";
            }
        }
        private void StartListening()
        {
            // Tạo socket UDP
            udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            // Lấy port từ textbox1
            int port = Convert.ToInt32(txtPort.Text);

            // Buộc socket UDP với endpoint
            udpSocket.Bind(new IPEndPoint(IPAddress.Any, port));

            // Tạo endpoint để nhận dữ liệu
            EndPoint ipRec = new IPEndPoint(IPAddress.Any, 0);

            // Bắt đầu nhận dữ liệu từ client
            udpSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref ipRec, ReceiveCallback, null);
        }
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Lấy endpoint của client
                EndPoint ipRec = new IPEndPoint(IPAddress.Any, 0);

                // Nhận số byte đã đọc
                int bytesRead = udpSocket.EndReceiveFrom(ar, ref ipRec);

                // Chuyển đổi dữ liệu sang chuỗi
                string mess = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                // Định dạng tin nhắn
                string formattedMessage = $"[{ipRec.ToString()}] {mess}\r\n";

                // Hiển thị tin nhắn lên richTextBox
                this.Invoke((MethodInvoker)delegate
                {
                    richTextBox1.AppendText(formattedMessage);
                });

                // Bắt đầu nhận dữ liệu mới
                EndPoint newIpRec = new IPEndPoint(IPAddress.Any, 0);
                udpSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref newIpRec, ReceiveCallback, null);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
