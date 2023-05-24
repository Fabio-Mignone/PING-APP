using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PingApp
{
    public partial class MainWindow : Window
    {
        private List<int> RTT = new List<int>();
        private List<int> TTL = new List<int>();
        private List<bool> DATA = new List<bool>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void BtnPing_Click(object sender, RoutedEventArgs e)
        {
            string address = TxtPing.Text;
            string? realPing = await VerifyAddressAsync(address); // Add nullability annotation

            if (realPing != null) // Null check
            {
                await PerformPingsAsync(realPing);

                LblPacket.Content = TotalDataCalculation();
                LblRTT.Content = TotalRoundTripTime();
                LblStatuspack1.Content = "TTL: " + TTL.ElementAtOrDefault(0); // Use ElementAtOrDefault to handle index out of range
                LblStatuspack2.Content = "TTL: " + TTL.ElementAtOrDefault(1);
                LblStatuspack3.Content = "TTL: " + TTL.ElementAtOrDefault(2);
                LblStatuspack4.Content = "TTL: " + TTL.ElementAtOrDefault(3);
                LblStatus1.Content = "Packet Received: " + DATA.ElementAtOrDefault(0);
                LblStatus2.Content = "Packet Received: " + DATA.ElementAtOrDefault(1);
                LblStatus3.Content = "Packet Received: " + DATA.ElementAtOrDefault(2);
                LblStatus4.Content = "Packet Received: " + DATA.ElementAtOrDefault(3);
                LblRttsingle1.Content = "RTT: " + RTT.ElementAtOrDefault(0);
                LblRttsingle2.Content = "RTT: " + RTT.ElementAtOrDefault(1);
                LblRttsingle3.Content = "RTT: " + RTT.ElementAtOrDefault(2);
                LblRttsingle4.Content = "RTT: " + RTT.ElementAtOrDefault(3);

                DATA.Clear();
                RTT.Clear();
                TTL.Clear();
            }
            else
            {
                MessageBox.Show("ERROR: Please enter a valid IP address or URL.");
            }
        }

        private async Task<string?> VerifyAddressAsync(string ipAddress) // Add nullability annotation
        {
            try
            {
                IPAddress[] addresses = await Dns.GetHostAddressesAsync(ipAddress);
                return addresses[0].ToString();
            }
            catch (Exception)
            {
                return null; // Return null if an error occurs
            }
        }

        private async Task PerformPingsAsync(string ipAddress)
        {
            using (Ping pingSender = new Ping())
            {
                for (int i = 0; i < 4; i++)
                {
                    PingReply reply = await pingSender.SendPingAsync(ipAddress);

                    if (reply != null && reply.Status == IPStatus.Success)
                    {
                        DATA.Add(true);
                        RTT.Add((int)reply.RoundtripTime);
                        TTL.Add(reply.Options?.Ttl ?? 0); // Perform null check on reply.Options before accessing its properties
                    }
                    else
                    {
                        DATA.Add(false);
                        RTT.Add(0);
                        TTL.Add(0);
                    }
                }
            }
        }

        private string TotalDataCalculation()
        {
            int dataOk = DATA.Count(d => d);
            int dataLoss = DATA.Count(d => !d);
            int percentage = (int)((dataOk / (double)DATA.Count) * 100);
            return $"{dataOk} packets received, {dataLoss} packets lost, {percentage}% packet success";
        }

        private string TotalRoundTripTime()
        {
            if (RTT.Any())
            {
                int minRtt = RTT.Min();
                int maxRtt = RTT.Max();
                int avgRtt = (int)RTT.Average();
                return $"Min RTT: {minRtt}ms, Max RTT: {maxRtt}ms, Avg RTT: {avgRtt}ms";
            }
            else
            {
                return "No packets received.";
            }
        }
    }
}
