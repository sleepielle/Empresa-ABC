using Microsoft.AspNetCore.Mvc;
using Proyecto.Gateway.DTOS;
using Proyecto.Gateway.Services;

namespace Proyecto.Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GatewayController : ControllerBase
    {
        private readonly Transaction _TransaccionService;
        public GatewayController(Transaction transaccionService)
        {
            _TransaccionService = transaccionService;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            string hola = "hola";
            return await Task.FromResult(hola);
        }

        [HttpPost("transaction")]
        public async Task<CreateTransactionDataTransferObject> Post([FromBody] CreateTransactionDataTransferObject lineToCreate)
        {

            var results = await _TransaccionService.ProcessTransaction(lineToCreate);
            return results;

            //    var newTransaction = new CreateTransactionDataTransferObject
            //    {
            //        Id = lineToCreate.Id,
            //        //suma todas las lineas
            //        Status = lineToCreate.Status,
            //        Errors = lineToCreate.Errors
            //    };

            //    try
            //    {
            //        var json = JsonConvert.SerializeObject(newTransaction);
            //        //esto es generico
            //        var factory = new ConnectionFactory
            //        {
            //            HostName = "localhost",
            //            Port = 5672,

            //        };

            //        using (var connection = factory.CreateConnection())
            //        {

            //            using (var channel = connection.CreateModel())
            //            {
            //                channel.QueueDeclare("gatewayQueue", false, false, false, null);
            //                //se manda el json a la queue en bytes. 
            //                var body = Encoding.UTF8.GetBytes(json);
            //                //enviar el mensaje, siempre se pone string.empty para nosotros,
            //                channel.BasicPublish(string.Empty, "gatewayQueue", null, body);

            //            }
            //        }

            //    }

            //    catch (Exception e)
            //    {
            //        return BadRequest(e.Message);
            //    }
            //    return Ok(newTransaction);
            //}
        }

    }
}
