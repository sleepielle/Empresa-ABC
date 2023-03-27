using Newtonsoft.Json;
using Proyecto.Validaciones.DTOS;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
namespace Proyecto.Validaciones.Services
{

    public class ReceiveMessages : BackgroundService
    {

        string queueName1 = "validationsQueue";
        private readonly IConnection conn1;
        private readonly IModel channel1;
        private readonly EventingBasicConsumer consumer1;
        private readonly HttpClient client;
        //private readonly IConnection conn2;
        //private readonly IModel channel2;
        //private readonly EventingBasicConsumer consumer2;
        //string queueName2 = "employeesQueue";

        //private readonly IConnection conn3;
        //private readonly IModel channel3;
        //private readonly EventingBasicConsumer consumer3;
        //string queueName3 = "branchesQueue";

        //private readonly IConnection conn4;
        //private readonly IModel channel4;
        //private readonly EventingBasicConsumer consumer4;
        //string queueName4 = "carsQueue";



        public ReceiveMessages()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672
            };

            conn1 = factory.CreateConnection();
            channel1 = conn1.CreateModel();
            consumer1 = new EventingBasicConsumer(channel1);
            client = new HttpClient();

            //conn2 = factory.CreateConnection();
            //channel2 = conn2.CreateModel();
            //consumer2 = new EventingBasicConsumer(channel2);

            //conn3 = factory.CreateConnection();
            //channel3 = conn3.CreateModel();
            //consumer3 = new EventingBasicConsumer(channel3);

            //conn4 = factory.CreateConnection();
            //channel4 = conn4.CreateModel();
            //consumer4 = new EventingBasicConsumer(channel4);

        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            int count = 0;

            consumer1.Received += async (model, content) =>
            {
                count++;
                var body = content.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonConvert.DeserializeObject<SalesDataTransferObject>(json);
                Database.sales.Add(message);
                employeesValidation(message.username);
                carsValidation(message.car_id);
                vinValidation(message.vin);
                buyerValidation(message.buyer_first_name, message.buyer_last_name, message.buyer_id);
            };

            channel1.BasicConsume(queueName1, true, consumer1);
            return Task.CompletedTask;
        }

        public async void employeesValidation(string username)
        {

            var baseUrl = $"http://localhost:5500/employees/{username}";

            var employee = await client.GetStringAsync($"{baseUrl}");
            if (!string.IsNullOrEmpty(employee))
            {
                var employeesInfo = JsonConvert.DeserializeObject<EmployeesDataTransferObject>(employee);
                if (Database.sales.Any(e => e.username == employeesInfo.username))
                {
                    Database.employees.Add(employeesInfo);

                    var baseUrlBranches = $"http://localhost:5500/branches/{employeesInfo.branch_id}";
                    var branch = await client.GetStringAsync($"{baseUrlBranches}");
                    var branchesInfo = JsonConvert.DeserializeObject<BranchesDataTransferObject>(branch);

                    if (Database.employees.Any(e => e.username == branchesInfo.username))
                    {
                        Console.WriteLine($"{branchesInfo.username} SI existe en la sucursal.");
                    }


                }

            }
            else
            {
                Console.WriteLine($"{username} no existe en la sucursal.");
            }
        }

        public async void carsValidation(string carId)
        {

            var baseUrl = $"http://localhost:5500/cars/{carId}";

            var car = await client.GetStringAsync($"{baseUrl}");
            if (!string.IsNullOrEmpty(car))
            {
                var carsInfo = JsonConvert.DeserializeObject<CarsDataTransferObject>(car);
                if (Database.sales.Any(e => e.car_id == carsInfo.id))
                {
                    Database.cars.Add(carsInfo);
                    var baseUrlBranches = $"http://localhost:5500/branches/{carsInfo.branch_id}";
                    var branch = await client.GetStringAsync($"{baseUrlBranches}");
                    var branchesInfo = JsonConvert.DeserializeObject<BranchesDataTransferObject>(branch);

                    if (Database.cars.Any(e => e.id == branchesInfo.car_id))
                    {
                        Console.WriteLine($"{carId} SI existe en la sucursal.");
                    }
                }


            }
            else
            {
                Console.WriteLine($"{carId} NO existe en la sucursal.");
            }
        }


        public void vinValidation(string vin)
        {
            if (vin.Length != 17)
            {
                Console.WriteLine("Car vin {0} not valid", vin);
            }
        }
        public void buyerValidation(string firstName, string lastName, string buyerId)
        {
            if (string.IsNullOrEmpty(buyerId) && string.IsNullOrEmpty(lastName))
            {
                Console.WriteLine("Buyer {0} last name and id not valid", firstName);
            }
            else if (string.IsNullOrEmpty(lastName))
            {
                Console.WriteLine("Buyer {0} last Name not valid", firstName);
            }
            else if (string.IsNullOrEmpty(buyerId))
            {
                Console.WriteLine("Buyer {0} id not valid", firstName);
            }

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Se está ejecutando.");
                await Task.Delay(1000, stoppingToken);
            }

        }
    }
}
