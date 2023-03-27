using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using Proyecto.Recolector.DTOS;
using RabbitMQ.Client;
using System.Globalization;
using System.Text;

namespace Proyecto.Recolector.Services
{
    public class Recolector
    {
        private List<SalesDataTransferObject> dataSalesList = new List<SalesDataTransferObject>();

        public void SendSales()
        {
            string filePath = $"C:\\Users\\pggis\\source\\repos\\ProyectoConcurrencia\\sales.csv";
            using (var reader = new StreamReader(filePath))
            using (var csvReader = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                HeaderValidated = null,
                Delimiter = ","
            }))
            {
                csvReader.Read();
                csvReader.ReadHeader();
                while (csvReader.Read())
                {
                    SalesDataTransferObject dataSales = new SalesDataTransferObject();
                    dataSales.username = csvReader.GetField("username");
                    dataSales.car_id = csvReader.GetField("car_id");
                    dataSales.price = csvReader.GetField("price");
                    dataSales.vin = csvReader.GetField("vin");
                    dataSales.buyer_first_name = csvReader.GetField("buyer_first_name");
                    dataSales.buyer_last_name = csvReader.GetField("buyer_last_name");
                    dataSales.buyer_id = csvReader.GetField("buyer_id");
                    dataSales.division_id = csvReader.GetField("division_id");
                    Console.WriteLine("Username:" + dataSales.username);
                    dataSalesList.Add(dataSales);
                    SendDataSalesToValidation(dataSales);

                    if (dataSalesList.Count == 50)  //lee 50 rows de  csv file
                    {
                        Console.WriteLine("50 en la lista\n\n"); //envia  50 rows a la clase de validacion
                        dataSalesList.Clear(); //elimina los  50 rows actualmente
                    }
                }
            }
        }

        //envia  50 rows
        private Task SendDataSalesToValidation(SalesDataTransferObject dataSalesList)
        {
            var json = JsonConvert.SerializeObject(dataSalesList);

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672
            };

            using var conn = factory.CreateConnection();
            using var channel = conn.CreateModel();

            //declarar queue
            channel.QueueDeclare("validationsQueue", false, false, false, null);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(string.Empty, "validationsQueue", null, body);
            Console.WriteLine(" [x] Sent {0} ", json.ToString());
            return Task.CompletedTask;
        }
    }
}