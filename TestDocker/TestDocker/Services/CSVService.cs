using CsvHelper;
using System.Globalization;

namespace TestDocker.Services
{
    public class CSVService : ICSVService
    {
        public void WriteCSV<T>(List<T> records)
        {
            using (var writer = new StreamWriter("D:\\file.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(records);
            }
        }
    }
}
