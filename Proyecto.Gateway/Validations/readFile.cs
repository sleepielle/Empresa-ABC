using CsvHelper;
using CsvHelper.Configuration;
using Proyecto.Gateway.DTOS;
using System.Globalization;

namespace ProyectoConcurrencia.Validations
{
    public class readFile
    {


        public IEnumerable<SalesDataTransferObject> readCsvFile()
        {
            string filepath = $"C:\\Users\\pggis\\OneDrive\\Documents\\02_University\\Q12023\\03_Concurrencia\\02_Proyecto\\sales.csv";

            using (var reader = new StreamReader(filepath))
            using (var csvReader = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                HeaderValidated = null,
                Delimiter = ","
            }))
            {
                var item = csvReader.GetRecords<SalesDataTransferObject>().ToList();
                Parallel.Invoke(() =>
                {
                    foreach (var i in item)
                    {
                        Database.sales.Add(i);
                    }
                });

                return csvReader.GetRecords<SalesDataTransferObject>().ToList();
            }
        }

        public void readFromList()
        {
            foreach (var item in Database.sales)
            {
                Console.WriteLine($"{item.username}, " +
                    $"{item.car_id}," +
                    $" {item.price}," +
                    $"{item.vin}," +
                    $"{item.buyer_first_name}, " +
                    $"{item.buyer_last_name}, " +
                    $"{item.buyer_id}, " +
                    $" {item.division_id}");
            }
        }
    }
}
