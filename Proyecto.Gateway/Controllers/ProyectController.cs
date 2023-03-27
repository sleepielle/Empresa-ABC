using Microsoft.AspNetCore.Mvc;
using Proyecto.Gateway.DTOS;
using Proyecto.Gateway.Service;
using Proyecto.Gateway.RabbitMq;
using System.Transactions;

namespace Proyecto.Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProyectController : ControllerBase
    {
        private readonly TransactionService _TransaccionService;
        public ProyectController(TransactionService transaccionService)
        {
            _TransaccionService = transaccionService;
        }
        [HttpPost("transaction")]
        public async Task<List<CreateTransaccion>>ProcessTransaction([FromBody] CreateTransaccion create )
        {
            List<CreateTransaccion> list = new List<CreateTransaccion>();
            list.Add(create);
            return await Task.FromResult(list);
        }


    }
}
