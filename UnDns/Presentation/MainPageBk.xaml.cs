/*
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json.Linq;
using System.Threading;
using Microsoft.UI.Xaml.Navigation;

namespace UnDns.Presentation;
    public sealed partial class MainPage : Page
    {
        private static readonly HttpClient client = new HttpClient();
        private DispatcherTimer dispatcherTimer;

        public MainPage()
        {
            this.InitializeComponent();
            InitializeTimer();
            GetData();
        }

        private void InitializeTimer()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(5); // Intervalo de 5 segundos
            dispatcherTimer.Start();
        }

        private async void DispatcherTimer_Tick(object sender, object e)
        {
            await GetData(); // Adicionado await aqui
        }

        private async Task GetData() // Alterado para Task
        {
            try
            {
                var response = await client.GetStringAsync("http://localhost:5000/dns_data");
                var dnsData = JsonSerializer.Deserialize<DnsData>(response);

                TotalDadosTextBlock.Text = dnsData.TotalDados.ToString();
                NumPacotesTextBlock.Text = dnsData.NumPacotes.ToString();
                MediaDadosTextBlock.Text = dnsData.MediaDados.ToString("F2");
            }
            catch (Exception ex)
            {
                TotalDadosTextBlock.Text = $"Error: {ex.Message}";
                NumPacotesTextBlock.Text = $"Error: {ex.Message}";
                MediaDadosTextBlock.Text = $"Error: {ex.Message}";
            }
        }

        private void RefreshDataButton_Click(object sender, RoutedEventArgs e)
        {
            GetData();
        }

        public class DnsData
        {
            public int TotalDados { get; set; }
            public int NumPacotes { get; set; }
            public double MediaDados { get; set; }
        }
    }
    */
/*
    using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace UnDns.Presentation;

    public sealed partial class MainPage : Page
    {
        private static readonly HttpClient client = new HttpClient();
        private DispatcherTimer dispatcherTimer;

        public MainPage()
        {
            this.InitializeComponent();
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(0.5); // Intervalo de 0.5 segundos
            dispatcherTimer.Start();
            GetData();
        }

        private void DispatcherTimer_Tick(object? sender, object? e)
        {
            GetData();
        }

        private async void GetData()
        {
            try
            {
                var response = await client.GetStringAsync("http://localhost:5000/dns_data");
                if (!string.IsNullOrEmpty(response))
                {
                    var dnsData = JsonSerializer.Deserialize<DnsData>(response);
                    if (dnsData != null)
                    {
                        TotalDadosTextBlock.Text = dnsData.TotalDados.ToString();
                        NumPacotesTextBlock.Text = dnsData.NumPacotes.ToString();
                        MediaDadosTextBlock.Text = dnsData.MediaDados.ToString("F2");
                    }
                }
            }
            catch (Exception ex)
            {
                TotalDadosTextBlock.Text = $"Error: {ex.Message}";
                NumPacotesTextBlock.Text = $"Error: {ex.Message}";
                MediaDadosTextBlock.Text = $"Error: {ex.Message}";
            }
        }

        private void RefreshDataButton_Click(object sender, RoutedEventArgs e)
        {
            GetData();
        }

        public class DnsData
        {
            public int TotalDados { get; set; }
            public int NumPacotes { get; set; }
            public double MediaDados { get; set; }
        }
    }
*/
/*
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace UnDns.Presentation;

    public sealed partial class MainPage : Page
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public MainPage()
        {
            this.InitializeComponent();
            FetchDataAsync();
        }

        private async void FetchDataAsync()
        {
            try
            {
                var response = await httpClient.GetStringAsync("http://localhost:5000/dns_data");
                var dnsData = JsonSerializer.Deserialize<DNSData>(response);

                TotalDataTextBlock.Text = $"Total de Dados: {dnsData.TotalDados} bytes";
                PacketCountTextBlock.Text = $"Número de Pacotes: {dnsData.NumPacotes}";
                AverageDataTextBlock.Text = $"Média de Dados: {dnsData.MediaDados} bytes";

                ConsultasListView.ItemsSource = dnsData.ConsultasDNS;
                RespostasListView.ItemsSource = dnsData.RespostasDNS;
            }
            catch (Exception ex)
            {
                TotalDataTextBlock.Text = $"Erro: {ex.Message}";
            }
        }

        private void OnRefreshDataButtonClick(object sender, RoutedEventArgs e)
        {
            FetchDataAsync();
        }
    }

    public class DNSData
    {
        public int TotalDados { get; set; }
        public int NumPacotes { get; set; }
        public double MediaDados { get; set; }
        public List<DNSInfo> ConsultasDNS { get; set; } = new List<DNSInfo>();
        public List<DNSInfo> RespostasDNS { get; set; } = new List<DNSInfo>();
    }

    public class DNSInfo
    {
        public string Src { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public int Size { get; set; }
    }
*/
/*
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace UnDns.Presentation;
    public sealed partial class MainPage : Page
    {
        private static readonly HttpClient client = new HttpClient();

        public MainPage()
        {
            this.InitializeComponent();
            GetData();
        }

        private async void GetData()
        {
            try
            {
                var response = await client.GetStringAsync("http://localhost:5001/dns_data");
                var dnsData = JsonSerializer.Deserialize<DNSData>(response);

                TotalDataTextBlock.Text = $"Total de Dados: {dnsData.total_dados} bytes";
                PacketCountTextBlock.Text = $"Número de Pacotes: {dnsData.num_pacotes}";
                AverageDataTextBlock.Text = $"Média de Dados: {dnsData.media_dados} bytes";

                ConsultasListView.ItemsSource = dnsData.consultas_dns;
                RespostasListView.ItemsSource = dnsData.respostas_dns;
            }
            catch (Exception ex)
            {
                TotalDataTextBlock.Text = $"Error: {ex.Message}";
            }
        }

        private void RefreshDataButton_Click(object sender, RoutedEventArgs e)
        {
            GetData();
        }
    }

    public class DNSData
    {
        public int total_dados { get; set; }
        public int num_pacotes { get; set; }
        public double media_dados { get; set; }
        public List<DNSInfo> consultas_dns { get; set; }
        public List<DNSInfo> respostas_dns { get; set; }
    }

    public class DNSInfo
    {
        public string src { get; set; }
        public string domain { get; set; }
        public int size { get; set; }
    }
    */