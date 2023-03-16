using Microsoft.AspNetCore.Mvc;
using Proyecto.Gateway.DTOS;
using Proyecto.Gateway.Service;

namespace Proyecto.Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProyectController : ControllerBase
    {
        private readonly TransaccionServicecs _TransaccionService;
        public ProyectController(TransaccionServicecs transaccionService)
        {
           _TransaccionService = transaccionService;
        }
        [HttpPost]
        public async Task<IActionResult> Post(Guid tId, CreateTransaccion lineToCreate)
        {
            var result = await _TransaccionService.AddAsync(tId, lineToCreate);
            return Ok(result);
        }

        
    }
}
