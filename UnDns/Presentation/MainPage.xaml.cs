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
            StartTimer();
        }

        private void StartTimer()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5); // Atualiza a cada 5 segundos
            dispatcherTimer.Start();
        }

        private async void DispatcherTimer_Tick(object sender, object e)
        {
            await GetData();
        }

        private async Task GetData()
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
                TotalDadosTextBlock.Text = "Error";
                NumPacotesTextBlock.Text = "Error";
                MediaDadosTextBlock.Text = ex.Message;
            }
        }
    }

    public class DnsData
    {
        public int TotalDados { get; set; }
        public int NumPacotes { get; set; }
        public double MediaDados { get; set; }
    }