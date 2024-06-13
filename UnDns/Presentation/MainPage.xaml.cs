using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Diagnostics;
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
            InitializeComponent();
            FetchDataAsync();
        }

        private async void FetchDataAsync()
        {
            try
            {
                var response = await httpClient.GetStringAsync("http://localhost:5000/dns_data");
                var dnsData = JsonSerializer.Deserialize<DNSData>(response);

                if (dnsData != null)
                {
                    TotalDataTextBlock.Text = $"Total de Dados: {dnsData.TotalDados} bytes";
                    PacketCountTextBlock.Text = $"Número de Pacotes: {dnsData.NumPacotes}";
                    AverageDataTextBlock.Text = $"Média de Dados: {dnsData.MediaDados} bytes";

                    ConsultasListView.ItemsSource = dnsData.ConsultasDNS ?? new List<DNSInfo>();
                    RespostasListView.ItemsSource = dnsData.RespostasDNS ?? new List<DNSInfo>();
                }
                else
                {
                    DisplayError("Dados de DNS não encontrados.");
                }
            }
            catch (Exception ex)
            {
                DisplayError($"Erro: {ex.Message}");
            }
        }

        private void DisplayError(string message)
        {
            TotalDataTextBlock.Text = message;
            PacketCountTextBlock.Text = string.Empty;
            AverageDataTextBlock.Text = string.Empty;
            ConsultasListView.ItemsSource = null;
            RespostasListView.ItemsSource = null;
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