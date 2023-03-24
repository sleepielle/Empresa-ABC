using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Proyecto.Gateway.DTOS;
using Proyecto.Gateway.Validations;
using System.Threading.Channels;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.VisualBasic;
using ProyectoConcurrencia.Validations;

namespace Proyecto.Gateway.Service
{
    public class TransactionService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly EventingBasicConsumer _consumer;
        private readonly HttpClient _httpClient;

        public TransactionService()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare("transaccion-queue" , false, false, false, null);


        _consumer = new EventingBasicConsumer(_channel);
          
    }
      
        public async Task<CreateTransaccion> ProcessTransaction(int Id, CreateTransaccion basketToCreate)
        {
            var result = await _httpClient.PostAsJsonAsync("/transaction/",basketToCreate);
            result.EnsureSuccessStatusCode();
            var response = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CreateTransaccion>(response);
           
            }

        public async Task<CreateTransaccion> ValidateTransaction(CreateTransaccion basketToCreate, CancellationToken token)
        {
            var error = new List<string>();
            var _info = new CreateTransaccion();
            if (_info.Id == _info.Id)
            {
                error.Add("La transaccion debe tener un Id valido");
            }

            await Task.Delay(2000, token);
            _info.Status = error.Any() ? Status.Errored : Status.Done;
            _info.Errors = error;
            return _info;

        }


    }
   



}
