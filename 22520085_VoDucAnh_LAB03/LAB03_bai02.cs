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
using System.Threading;

namespace _22520085_VoDucAnh_LAB03
{
    public partial class LAB03_bai02 : Form
    {
        public LAB03_bai02()
        {
            InitializeComponent();
        }
        private void Recieve()
        {
            // Tạo socket mới
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Tạo IPEndPoint mới
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);

            // Gắn socket với IPEndPoint
            listener.Bind(ip);

            // Lắng nghe trên socket với backlog là 5
            listener.Listen(5);

            // Chấp nhận kết nối từ socket và lưu trữ nó trong biến clientsocket
            Socket clientsocket = listener.Accept();

            // Vòng lặp để nhận và xử lý tin nhắn
            while (clientsocket.Connected)
            {
                // Tạo mảng byte để lưu trữ dữ liệu nhận được
                byte[] buffer = new byte[1024];

                // Tạo mảng byte để lưu trữ từng byte nhận được
                byte[] recv = new byte[1];

                // Biến đếm vị trí trong mảng buffer
                int i = 0;

                // Vòng lặp để nhận từng byte cho đến khi gặp ký tự kết thúc dòng '\n'
                do
                {
                    // Nhận một byte vào recv
                    clientsocket.Receive(recv);

                    // Lưu trữ byte nhận được vào buffer
                    buffer[i] = recv[0];

                    // Tăng biến đếm
                    i++;
                } while (recv[0] != '\n');

                // Chuyển đổi buffer thành chuỗi
                string text = Encoding.UTF8.GetString(buffer);

                // Kiểm tra xem ListView có yêu cầu Invoke hay không
                if (listView1.InvokeRequired)
                {
                    // Gọi Invoke trên ListView để thêm văn bản vào ListView
                    listView1.Invoke((MethodInvoker)delegate
                    {
                        listView1.Items.Add(text);
                    });
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Enabled= false;
            Thread thread = new Thread(Recieve);
            thread.Start();
        }
    }
}
