using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMeterReportHelper
{
    class Program
    {
        public static string SourceLogsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SourceLogs");
        public static string TargetLogsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TargetLogs");

        static void Main(string[] args)
        {
            //InitializeLogFolders();
            string[] endpointFolders = Directory.GetDirectories(SourceLogsFolder);
            foreach(string endpointFolder in endpointFolders)
            {
                string endpointName = new DirectoryInfo(endpointFolder).Name;
                string[] endpointReports = Directory.GetFiles(endpointFolder);
                foreach(string csvReport in endpointReports)
                {
                    string numberOfUsers = new FileInfo(csvReport).Name.Split(' ').First();
                    var performanceRecord = new ReportPerformanceRecord
                    {
                        RequestName = endpointName,
                        NumberOfUsers = numberOfUsers
                    };
                    var csvRecords = new List<CsvPerformanceRecord>();

                    using (TextReader reader = File.OpenText(csvReport))
                    {

                        var csv = new CsvReader(reader);
                        csv.Configuration.Delimiter = ",";
                        csv.Configuration.MissingFieldFound = null;

                        while (csv.Read())
                        {
                            CsvPerformanceRecord csvRecord = csv.GetRecord<CsvPerformanceRecord>();
                            csvRecords.Add(csvRecord);
                        }
                    }

                    performanceRecord.PopulateWithData(csvRecords);
                }
            }
        }

        private static void InitializeLogFolders()
        {
            if (Directory.Exists(SourceLogsFolder))
            {
                Directory.Delete(SourceLogsFolder, true);
            }
            Directory.CreateDirectory(SourceLogsFolder);

            if (!Directory.Exists(TargetLogsFolder))
            {
                Directory.CreateDirectory(TargetLogsFolder);
            }
        }
    }
}
