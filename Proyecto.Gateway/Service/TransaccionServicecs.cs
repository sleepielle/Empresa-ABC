using Proyecto.Gateway.DTOS;
using ProyectoConcurrencia.DTOS;

namespace Proyecto.Gateway.Service
{
    public interface TransaccionServicecs
    {
        Task<SalesDataTransferObject> AddAsync(Guid tId,CreateTransaccion basketToCreate);
    }
}
