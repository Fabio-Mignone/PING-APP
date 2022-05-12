using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows;

namespace MignonePingApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private List<int> RTT = new List<int>();
        private List<int> TTL = new List<int>();
        private List<bool> DATA = new List<bool>();
        private void BtnPing_Click(object sender, RoutedEventArgs e)
        {
            string address = TxtPing.Text;
            if (VerifyAddress(address) == true)
            {
                for (int i = 0; i <= 3; i++)
                {
                    Ping(address);
                }
                LblPacket.Content = TotalDataCalculation();
                LblRTT.Content = TotalRoundTripTime();
                LblStatuspack1.Content = "TTL: " + TTL[0];
                LblStatuspack2.Content = "TTL: " + TTL[1];
                LblStatuspack3.Content = "TTL: " + TTL[2];
                LblStatuspack4.Content = "TTL: " + TTL[3];
                LblStatus1.Content = "Pacchetto Ricevuto: " + DATA[0];
                LblStatus2.Content = "Pacchetto Ricevuto: " + DATA[1];
                LblStatus3.Content = "Pacchetto Ricevuto: " + DATA[2];
                LblStatus4.Content = "Pacchetto Ricevuto: " + DATA[3];
                LblRttsingle1.Content = "RTT: " + RTT[0];
                LblRttsingle2.Content = "RTT: " + RTT[1];
                LblRttsingle3.Content = "RTT: " + RTT[2];
                LblRttsingle4.Content = "RTT: " + RTT[3];
                DATA.Clear();
                RTT.Clear();
                TTL.Clear();
            }
            else
            {
                MessageBox.Show("ERRORE INSERIRE UN INDIRIZZO IP VALIDO");
            }
        }

        public bool VerifyAddress(string ipAddress)
        {
            bool retVal = false;
            try
            {
                IPAddress address;
                retVal = IPAddress.TryParse(ipAddress, out address);
            }
            catch (Exception ex)
            {
                retVal = false;
            }
            return retVal;
        }

        public void Ping(string address)
        {
            bool TempDataTransfer;
            LblPacket.Content = "";
            Ping pingSender = new Ping();
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 10000;
            PingOptions options = new PingOptions(64, true);
            PingReply reply = pingSender.Send(address, timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
            {
                if (reply.Status.ToString() != "DestinationHostUnreachable")
                {
                    TempDataTransfer = true;
                    DATA.Add(TempDataTransfer);
                    RTT.Add((int)reply.RoundtripTime);
                    TTL.Add(reply.Options.Ttl);
                }
                else if (reply.Status.ToString() == "DestinationHostUnreachable")
                {
                    TempDataTransfer = false;
                    DATA.Add(TempDataTransfer);
                }
            }
            else
            {
                LblPacket.Content = "ERRORE INDIRIZZO NON RAGGIUNGIBILE, INSERIRE UN INDIRIZZO VALIDO";
            }
        }

        public string TotalDataCalculation()
        {
            int percentuale;
            int dataok = 0;
            int dataloss = 0;
            string totaloutput = string.Empty;
            foreach (bool elemento in DATA)
            {
                if (elemento == true)
                {
                    dataok++;
                }
                else if (elemento == false)
                {
                    dataloss++;
                }
            }
            percentuale = (dataok / dataok) * 100;
            totaloutput = "pacchetti inviati: " + dataok + " / " + " pacchetti persi: " + dataloss + " / " + " Percentuale di successo: " + percentuale;
            return totaloutput;
        }

        public string TotalRoundTripTime()
        {
            string totrtt = string.Empty;
            int vmax;
            int med;
            int vmin;
            vmax = (RTT.Max());
            vmin = (RTT.Min());
            med = RTT.Sum() / 4;
            totrtt = "RTT Massimo: " + vmax + " / " + " RTT Minimo: " + vmin + " / " + " RTT Medio: " + med;
            return totrtt;
        }
    }
}