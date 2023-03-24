


using ProyectoConcurrencia;

namespace Proyecto.Gateway.Validations
{
    public class Validations
    {
        public void checkList()
        {
            foreach (var item in Database.branches)
            {
                Console.WriteLine($"{item.id}, {item.country}, {item.state}");
            }
        }
    }
}
