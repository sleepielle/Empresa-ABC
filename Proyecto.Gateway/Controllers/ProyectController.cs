using Microsoft.AspNetCore.Mvc;
using Proyecto.Gateway.DTOS;
using Proyecto.Gateway.Service;

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
        [HttpPost]
        public Task<IActionResult> Post(int Id, CreateTransaccion lineToCreate, Task<IActionResult> result)
        {

            var results =  _TransaccionService.ProcessTransaction( Id, lineToCreate);
            return result;
        }


    }
}
