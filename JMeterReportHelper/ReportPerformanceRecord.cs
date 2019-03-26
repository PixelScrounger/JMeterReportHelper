using System.Collections.Generic;
using System.Linq;

namespace JMeterReportHelper
{
    public class ReportPerformanceRecord
    {
        public string RequestName { get; set; }

        public string NumberOfUsers { get; set; }

        public string Throughput { get; set; }

        public double AverageLatency { get; set; }

        public double MinimumLatency { get; set; }

        public double MaximumLatency { get; set; }

        public double AverageConnectionTime { get; set; }

        public double MinimumConnectionTime { get; set; }

        public double MaximumConnectionTime { get; set; }

        public string ErrorPercentage { get; set; }

        public void PopulateWithData(List<CsvPerformanceRecord> csvRecords)
        {
            Throughput = CalculateThroughput(csvRecords);
            AverageLatency = csvRecords.Average(record => record.Latency);
            MinimumLatency = csvRecords.Min(record => record.Latency);
            MaximumLatency = csvRecords.Max(record => record.Latency);
            AverageConnectionTime = csvRecords.Average(record => record.Connect);
            MinimumConnectionTime = csvRecords.Min(record => record.Connect);
            MaximumConnectionTime = csvRecords.Max(record => record.Connect);
            ErrorPercentage = CalculateErrorPercentage(csvRecords);
        }

        public string CalculateThroughput(List<CsvPerformanceRecord> csvRecords)
        {
            long minimumTimestamp = csvRecords.Min(record => record.Timestamp);
            long maximumTimestamp = csvRecords.Max(record => record.Timestamp + record.Elapsed);
            long deltaTimestamp = maximumTimestamp - minimumTimestamp;
            int numberOfRecords = csvRecords.Count;

            string measurement = "/sec";
            decimal result = (decimal)numberOfRecords / deltaTimestamp * 1000;
            if(result < 1.0m)
            {
                result *= 1000;
                measurement = "/min";
            }
            if (result < 1.0m)
            {
                result *= 1000;
                measurement = "/hour";
            }
            decimal roundedResult = decimal.Round(result, 2);
            string formattedResult = roundedResult.ToString("F");

            return $"{formattedResult}{measurement}";
        }

        public string CalculateErrorPercentage(List<CsvPerformanceRecord> csvRecords)
        {
            int numberOfErrors = csvRecords.Select(record => !record.Success).ToList().Count;
            int totalNumberOfRecords = csvRecords.Count;
            decimal result = 1 - decimal.Round((decimal)numberOfErrors / totalNumberOfRecords, 2);
            result *= 100;
            string formatedResult = result.ToString("F");

            return $"{formatedResult}%";
        }
    }
}
