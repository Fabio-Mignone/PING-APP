using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Logic
{
    public class StatisticsHelper
    {
        public string CalculateTotalData(List<bool> data)
        {
            int dataOk = data.Count(d => d);
            int dataLoss = data.Count(d => !d);
            int percentage = (int)((dataOk / (double)data.Count) * 100);
            return $"{dataOk} packets received, {dataLoss} packets lost, {percentage}% packet success";
        }

        public string CalculateTotalRoundTripTime(List<int> rtt)
        {
            if (rtt.Any())
            {
                int minRtt = rtt.Min();
                int maxRtt = rtt.Max();
                int avgRtt = (int)rtt.Average();
                return $"Min RTT: {minRtt}ms, Max RTT: {maxRtt}ms, Avg RTT: {avgRtt}ms";
            }
            else
            {
                return "No packets received.";
            }
        }
    }
}
