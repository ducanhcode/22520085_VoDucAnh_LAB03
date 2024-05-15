using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace _22520085_VoDucAnh_LAB03
{
    public partial class BAI04_server : Form
    {
        public BAI04_server()
        {
            InitializeComponent();
        }
        List<BoughtTicket> soldTickets = new List<BoughtTicket>();
        private List<Movie> movies;
        private List<MovieStatistics> movieStatistics = new List<MovieStatistics>(); // Danh sách thống kê doanh thu của mỗi phim
        private List<MovieRanking> movieRankings = new List<MovieRanking>(); // Danh sách xếp hạng doanh thu phòng vé
        
        public void Receive(Socket clientSocket)
        {
            while (clientSocket.Connected)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = clientSocket.Receive(buffer);
                    string text = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    string[] tach = text.Split(';');
                    if (tach[0] == "Export")
                    {
                        // Cập nhật xếp hạng doanh thu phòng vé
                        UpdateMovieRankings();

                        // Gửi thông tin thống kê về cho Client
                        var combinedData = new
                        {
                            Statistics = movieStatistics,
                            Rankings = movieRankings
                        };

                        // Chuyển danh sách thông tin thành JSON
                        string statisticsJson = "Export;" + JsonConvert.SerializeObject(combinedData);

                        // Gửi JSON chứa thông tin thống kê và xếp hạng về cho Client
                        byte[] buffer_ = Encoding.UTF8.GetBytes(statisticsJson);
                        clientSocket.Send(buffer_);
                    }
                    else
                    {
                        if (soldTickets.Any(ticket => ticket.Movie == tach[0] && ticket.Room == tach[1] && ticket.Seat == tach[2]))
                        {
                            string message = "Fail;";
                            byte[] buffer_ = Encoding.UTF8.GetBytes(message);
                            clientSocket.Send(buffer_);
                        }
                        else
                        {
                            soldTickets.Add(new BoughtTicket { Movie = tach[0], Room = tach[1], Seat = tach[2] });

                            // Tính toán doanh thu và số lượng vé tồn của phim
                            int ticketPrice = movies.FirstOrDefault(m => m.TenPhim == tach[0])?.GiaVeChuan ?? 0;
                            int remainingSeats = 15 - soldTickets.Count(ticket => ticket.Movie == tach[0] && ticket.Room == tach[1]);

                            // Cập nhật thông tin thống kê của phim
                            UpdateMovieStatistics(tach[0], ticketPrice, remainingSeats);

                            string message = "Success;" + tach[2] + ";" + tach[0] + ";" + tach[1];
                            byte[] buffer_ = Encoding.UTF8.GetBytes(message);
                            clientSocket.Send(buffer_);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error receiving message: " + ex.Message);
                    break;
                }
            }
        }

        public void UpdateMovieRankings()
        {
            // Xếp hạng doanh thu phòng vé theo thứ tự giảm dần
            movieRankings = movieStatistics.OrderByDescending(x => x.DoanhThu).Select((stat, index) =>
                new MovieRanking
                {
                    TenPhim = stat.TenPhim,
                    DoanhThuPhongVe = stat.DoanhThu,
                    XepHang = index + 1
                }).ToList();
        }

        public void StartListening()
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            listener.Bind(ip);
            listener.Listen(5);
            while (true)
            {
                Socket clientSocket = listener.Accept();
                Thread thread = new Thread(() => Receive(clientSocket));
                thread.Start();
            }
        }

        public void UpdateMovieStatistics(string movieName, int revenue, int remainingSeats)
        {
            // Tính tổng số ghế của tất cả các phòng chiếu mà phim đó được chiếu
            int totalSeats = movies.FirstOrDefault(m => m.TenPhim == movieName)?.PhongChieu.Count * 15 ?? 0;


            // Kiểm tra xem phim đã có trong danh sách thống kê chưa
            MovieStatistics movieStat = movieStatistics.FirstOrDefault(m => m.TenPhim == movieName);

            // Nếu phim đã có trong danh sách, cập nhật thông tin
            if (movieStat != null)
            {
                movieStat.DoanhThu += revenue; // Cập nhật doanh thu
                movieStat.SoLuongVeTon = totalSeats - soldTickets.Count(ticket => ticket.Movie == movieName); // Cập nhật số lượng vé tồn
                movieStat.TiLeVeBan = 1 - (double)movieStat.SoLuongVeTon / totalSeats; // Cập nhật tỷ lệ vé bán
            }
            // Nếu phim chưa có trong danh sách, thêm vào danh sách
            else
            {
                movieStatistics.Add(new MovieStatistics
                {
                    TenPhim = movieName,
                    DoanhThu = revenue,
                    SoLuongVeTon = totalSeats - soldTickets.Count(ticket => ticket.Movie == movieName),
                    TiLeVeBan = 1 - (double)(totalSeats - soldTickets.Count(ticket => ticket.Movie == movieName)) / totalSeats
                });
            }
        }


        public class BoughtTicket
        {
            public string Movie { get; set; }
            public string Room { get; set; }
            public string Seat { get; set; }
        }

        public class MovieStatistics
        {
            public string TenPhim { get; set; }
            public int DoanhThu { get; set; }
            public int SoLuongVeTon { get; set; }
            public double TiLeVeBan { get; set; }
        }

        public class MovieRanking
        {
            public string TenPhim { get; set; }
            public int DoanhThuPhongVe { get; set; }
            public int XepHang { get; set; }
        }
        public class Movie
        {
            public string TenPhim { get; set; }
            public int GiaVeChuan { get; set; }
            public List<int> PhongChieu { get; set; }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            Thread thread = new Thread(StartListening);
            thread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string jsonContent = File.ReadAllText(openFileDialog.FileName);

                try
                {
                    movies = JsonConvert.DeserializeObject<List<Movie>>(jsonContent);
                    MessageBox.Show("Dữ liệu phim đã được tải thành công từ tệp JSON.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi đọc dữ liệu từ tệp JSON: " + ex.Message);
                }
            }
        }
    }
}
