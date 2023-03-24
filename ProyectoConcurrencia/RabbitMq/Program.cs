using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Microsoft.AspNetCore.Connections;
using System.Formats.Asn1;

namespace ProyectoConcurrencia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string filePath = "RabbitMq//sales.csv";

            List<DataSales> dataSalesList = new List<DataSales>();

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CurrentCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    DataSales dataSales = new DataSales();
                    dataSales.username = csv.GetField("username");
                    dataSales.car_id = csv.GetField("car_id");
                    dataSales.price = csv.GetField("price");
                    dataSales.vin = csv.GetField("vin");
                    dataSales.buyerFirstN = csv.GetField("buyer_first_name");
                    dataSales.buyerLastN = csv.GetField("buyer_last_name");
                    dataSales.buyersId = csv.GetField("buyer_id");
                    dataSales.divisionId = csv.GetField("division_id");

                    dataSalesList.Add(dataSales);

                    if (dataSalesList.Count == 50)  //lee 50 rows de  csv file
                    {

                        Console.WriteLine("50 en la lista\n\n\n\n");
                        SendDataSalesToValidation(dataSalesList); //envia  50 rows a la clase de validacion
                        dataSalesList.Clear(); //elimina los  50 rows actualmente
                    }
                }
            }
        }

        //envia  50 rows 
        private static void SendDataSalesToValidation(List<DataSales> dataSalesList)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "validation_queue",
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

                string message = JsonConvert.SerializeObject(dataSalesList);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                    routingKey: "validation_queue",
                                    basicProperties: null,
                                    body: body);

                Console.WriteLine(" [x] Sent {0}", message);
            }
        }
    }
}