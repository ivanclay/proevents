using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contracts
{
    public interface ILotePersist
    {
         Task<Lote[]> GetLotesByEventoIdAsync(int eventoId);
         Task<Lote> GetLoteByIdsAsync(int eventoId, int id);
    }
}