using PingApp.Logic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PingApp
{
    public partial class MainWindow : Window
    {
        private PingManager pingManager;
        private StatisticsHelper statisticsHelper;

        public MainWindow()
        {
            InitializeComponent();
            pingManager = new PingManager();
            statisticsHelper = new StatisticsHelper();

            TxtPing.KeyDown += TxtPing_KeyDown;
        }

        private async void BtnPing_Click(object sender, RoutedEventArgs e)
        {
            string address = TxtPing.Text;
            PingResult result = await pingManager.PerformPingsAsync(address);

            if (result != null)
            {
                LblPacket.Content = statisticsHelper.CalculateTotalData(result.Data);
                LblRTT.Content = statisticsHelper.CalculateTotalRoundTripTime(result.Rtt);
                LblStatuspack1.Content = "TTL: " + result.Ttl.ElementAtOrDefault(0);
                LblStatuspack2.Content = "TTL: " + result.Ttl.ElementAtOrDefault(1);
                LblStatuspack3.Content = "TTL: " + result.Ttl.ElementAtOrDefault(2);
                LblStatuspack4.Content = "TTL: " + result.Ttl.ElementAtOrDefault(3);
                LblStatus1.Content = "Packet Received: " + result.Data.ElementAtOrDefault(0);
                LblStatus2.Content = "Packet Received: " + result.Data.ElementAtOrDefault(1);
                LblStatus3.Content = "Packet Received: " + result.Data.ElementAtOrDefault(2);
                LblStatus4.Content = "Packet Received: " + result.Data.ElementAtOrDefault(3);
                LblRttsingle1.Content = "RTT: " + result.Rtt.ElementAtOrDefault(0);
                LblRttsingle2.Content = "RTT: " + result.Rtt.ElementAtOrDefault(1);
                LblRttsingle3.Content = "RTT: " + result.Rtt.ElementAtOrDefault(2);
                LblRttsingle4.Content = "RTT: " + result.Rtt.ElementAtOrDefault(3);

                result.Data.Clear();
                result.Rtt.Clear();
                result.Ttl.Clear();
            }
            else
            {
                MessageBox.Show("ERROR: Please enter a valid IP address or URL.");
            }
        }

        private void TxtPing_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnPing_Click(sender, e);
            }
        }
    }
}