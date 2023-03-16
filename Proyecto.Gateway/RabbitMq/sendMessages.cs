using Newtonsoft.Json;
using Proyecto.Gateway.DTOS;
using ProyectoConcurrencia.DTOS;
using ProyectoConcurrencia.Validations;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;


namespace Proyecto.Gateway.RabbitMq  
{

    public class sendMessages : BackgroundService
    {

        
        //solo se quiere recibir. no se usan parametros en constructor
        private readonly IConnection conn;
        private readonly IModel channel_validations, channel_recolect;
        private readonly EventingBasicConsumer consumer;



        public sendMessages()
        {

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672
            };

            conn = factory.CreateConnection();
            channel_validations = conn.CreateModel();
            channel_recolect = conn.CreateModel();
            channel_validations.QueueDeclare("Validations", false, false, false, null);
            channel_recolect.QueueDeclare("Recolect", false, false, false, null);
            consumer = new EventingBasicConsumer(channel_recolect);



        }
        // para recolector
        public override Task StartAsync(CancellationToken cancellationToken)
        {

            var message = "Recopilar datos";
            var body = Encoding.UTF8.GetBytes(message);
            channel_validations.BasicPublish(exchange: "", routingKey: "recolector", basicProperties: null, body: body);
            Console.WriteLine("Mensaje enviado a la cola de recolector: {0}", message);
          
            return Task.CompletedTask;
        }
        public Task SAsync(CancellationToken cancellationToken)
        {

            var message = "Validar datos";
            var body = Encoding.UTF8.GetBytes(message);
            channel_recolect.BasicPublish(exchange: "", routingKey: "validacion", basicProperties: null, body: body);
            Console.WriteLine("Mensaje enviado a la cola de validación: {0}", message);
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
