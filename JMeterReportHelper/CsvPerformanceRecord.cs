using CsvHelper.Configuration.Attributes;

namespace JMeterReportHelper
{
    public class CsvPerformanceRecord
    {
        [Name("timeStamp")]
        public long Timestamp { get; set; }

        [Name("elapsed")]
        public int Elapsed { get; set; }

        [Name("success")]
        public bool Success { get; set; }

        [Name("failureMessage")]
        public string FailureMessage { get; set; }

        [Name("Latency")]
        public int Latency { get; set; }

        [Name("Connect")]
        public int Connect { get; set; }
    }
}
