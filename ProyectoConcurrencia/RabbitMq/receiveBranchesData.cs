using Newtonsoft.Json;
using ProyectoConcurrencia.DTOS;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
namespace ProyectoConcurrencia.RabbitMq
{

    public class receiveBranchesData : BackgroundService
    {

        string queueName = "branchesQueue";
        //solo se quiere recibir. no se usan parametros en constructor
        private readonly IConnection conn;
        private readonly IModel channel;
        private readonly EventingBasicConsumer consumer;



        public receiveBranchesData()
        {

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672
            };

            conn = factory.CreateConnection();
            channel = conn.CreateModel();
            channel.QueueDeclare(queueName, true, false, false, null);
            consumer = new EventingBasicConsumer(channel);

        }
        //solo ocupo leer pero se usa
        public override Task StartAsync(CancellationToken cancellationToken)
        {

            consumer.Received += (model, content) =>
            {
                var body = content.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonConvert.DeserializeObject<BranchesDataTransferObject>(json);
                Console.WriteLine("Se ha recibido mensaje: " + json);

                Database.branches.Add(message);
                var validate = new ProyectoConcurrencia.Validations.Validations();
                validate.checkList();


            };
            channel.BasicConsume(queueName, true, consumer);
            return Task.CompletedTask;
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
