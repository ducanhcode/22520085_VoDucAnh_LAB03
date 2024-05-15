using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Newtonsoft.Json;

namespace _22520085_VoDucAnh_LAB03
{
    public partial class BAI04_client : Form
    {
        Socket clientSocket;
        Dictionary<string, (int price, List<string> rooms)> movies;
        List<CheckBox> seats = new List<CheckBox>();
        public BAI04_client()
        {
            InitializeComponent();
            InitializeSeats();
            LoadMoviesFromJson();
        }
        private void LoadMoviesFromJson()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string jsonContent = File.ReadAllText(openFileDialog.FileName);

                    if (!string.IsNullOrEmpty(jsonContent))
                    {
                        var moviesData = JsonConvert.DeserializeObject<List<Movie>>(jsonContent);

                        if (moviesData != null && moviesData.Count > 0)
                        {
                            foreach (var movie in moviesData)
                            {
                                if (string.IsNullOrEmpty(movie.TenPhim) || movie.GiaVeChuan <= 0 || movie.PhongChieu == null || movie.PhongChieu.Count == 0)
                                {
                                    MessageBox.Show("Cấu trúc của file JSON không đúng. Vui lòng kiểm tra lại.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            movies = new Dictionary<string, (int, List<string>)>();
                            foreach (var movie in moviesData)
                            {
                                movies.Add(movie.TenPhim, (movie.GiaVeChuan, movie.PhongChieu.Select(x => x.ToString()).ToList()));
                            }
                            PopulateComboBoxes();

                            MessageBox.Show("Dữ liệu phim đã được tải thành công từ tệp JSON.");
                        }
                        else
                        {
                            MessageBox.Show("Tệp JSON không chứa dữ liệu phim hợp lệ.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tệp JSON trống.");
                    }
                }
                else
                {
                    MessageBox.Show("Bạn chưa chọn file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi đọc dữ liệu từ tệp JSON: " + ex.Message);
            }
        }
        public class Movie
        {
            public string TenPhim { get; set; }
            public int GiaVeChuan { get; set; }
            public List<string> PhongChieu { get; set; }
        }
        private void PopulateComboBoxes()
        {
            if (movies != null)
            {
                foreach (var movie in movies)
                {
                    comboBox1.Items.Add(movie.Key);
                }

                if (comboBox1.Items.Count > 0)
                {
                    comboBox1.SelectedIndex = 0;
                }
            }
        }
        private void InitializeSeats()
        {
            seats.AddRange(new CheckBox[] { A1, A2, A3, A4, A5, B1, B2, B3, B4, B5, C1, C2, C3, C4, C5 });
        }
        private void button2_Click(object sender, EventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(IPAddress.Parse("127.0.0.1"), 8080);
        }
        private void ClientRecv()
        {
            int totalPrice = 0;
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead = clientSocket.Receive(buffer);
                string text = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                string[] tach = text.Split(';');
                if (tach[0] == "Fail")
                {
                    MessageBox.Show("Ghế đã được chọn vui lòng chọn ghế khác", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (tach[0] == "Success")
                {
                    int seatPrice = movies[tach[2]].price;
                    if (tach[1] == "A1" || tach[1] == "A5" || tach[1] == "B1" || tach[1] == "B5" || tach[1] == "C1" || tach[1] == "C5")
                    {
                        seatPrice /= 4;
                    }
                    else if (tach[1] == "B2" || tach[1] == "B3" || tach[1] == "B4")
                    {
                        seatPrice *= 2;
                    }
                    else
                    {
                        seatPrice *= 1;
                    }
                    totalPrice += seatPrice;
                    MessageBox.Show($"Họ tên: {nameTextBox.Text}\nPhim: {tach[2]}\nPhòng chiếu:{tach[3]}\nTổng tiền: {totalPrice}đ");
                }
                else if (tach[0] == "Export")
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                    saveFileDialog.Title = "Chọn nơi lưu trữ cho tệp JSON";
                    saveFileDialog.RestoreDirectory = true;

                    // Hiển thị hộp thoại và kiểm tra kết quả
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(saveFileDialog.FileName, tach[1]);
                    }
                }
            }
            catch (SocketException sockEx)
            {
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string message = "Export;";
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(buffer);
            ClientRecv();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string selectedMovie = comboBox1.SelectedItem.ToString();
            string selectedRoom = comboBox2.SelectedItem.ToString();
            bool seatSelected = false;
            foreach (var seat in seats)
            {
                if (seat.Checked)
                {
                    string message = selectedMovie + ";" + selectedRoom + ";" + seat.Name;
                    byte[] buffer = Encoding.UTF8.GetBytes(message);
                    clientSocket.Send(buffer);
                    seatSelected = true;
                }
            }
            if (!seatSelected)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một ghế", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ClientRecv();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedMovie = comboBox1.SelectedItem.ToString();
            comboBox2.Items.Clear();
            foreach (string room in movies[selectedMovie].rooms)
            {
                comboBox2.Items.Add(room);
            }
            if (comboBox2.Items.Count > 0)
            {
                comboBox2.SelectedIndex = 0;
            }
        }
    }
}
