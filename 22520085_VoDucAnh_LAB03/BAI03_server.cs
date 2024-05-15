using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _22520085_VoDucAnh_LAB03
{
    public partial class BAI03_server : Form
    {
        Socket Client;
        IPEndPoint IPEP;
        TcpListener Listener;
        public BAI03_server()
        {
            InitializeComponent();
        }
        void Connect()
        {
            IPEP = new IPEndPoint(IPAddress.Any, int.Parse(textBox1.Text));
            Listener = new TcpListener(IPEP);
            Addmessage("Server started!");

            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    Listener.Start();
                    Client = Listener.AcceptSocket();
                    Addmessage("Connection accepted from: " + Client.RemoteEndPoint.ToString() + "\n");

                    Thread receive = new Thread(Receive);
                    receive.IsBackground = true;
                    receive.Start(Client);
                }
            });

            thread.IsBackground = true;
            thread.Start();
        }

        void Receive(Object obj)
        {
            while (true)
            {
                Socket client = obj as Socket;
                byte[] recv = new byte[1000];
                Client.Receive(recv);
                string str = Encoding.UTF8.GetString(recv);
                Addmessage("From client: " + str);
            }
        }
        void Addmessage(string message)
        {
            listView1.Items.Add(message);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            Connect();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
