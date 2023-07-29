using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

public class PingManager
{
    private List<int> RTT { get; set; }
    private List<int> TTL { get; set; }
    private List<bool> DATA { get; set; }

    public PingManager()
    {
        RTT = new List<int>();
        TTL = new List<int>();
        DATA = new List<bool>();
    }

    public async Task<PingResult> PerformPingsAsync(string address)
    {
        try
        {
            IPAddress[] addresses = await Dns.GetHostAddressesAsync(address);
            string realPing = addresses[0].ToString();
            using (Ping pingSender = new Ping())
            {
                for (int i = 0; i < 4; i++)
                {
                    PingReply reply = await pingSender.SendPingAsync(realPing);

                    if (reply != null && reply.Status == IPStatus.Success)
                    {
                        DATA.Add(true);
                        RTT.Add((int)reply.RoundtripTime);
                        TTL.Add(reply.Options?.Ttl ?? 0);
                    }
                    else
                    {
                        DATA.Add(false);
                        RTT.Add(0);
                        TTL.Add(0);
                    }
                }
            }

            return new PingResult(DATA, RTT, TTL);
        }
        catch (Exception)
        {
            return null;
        }
    }
}

public class PingResult
{
    public List<bool> Data { get; private set; }
    public List<int> Rtt { get; private set; }
    public List<int> Ttl { get; private set; }

    public PingResult(List<bool> data, List<int> rtt, List<int> ttl)
    {
        Data = data;
        Rtt = rtt;
        Ttl = ttl;
    }
}