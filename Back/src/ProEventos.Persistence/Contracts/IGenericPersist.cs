using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contracts
{
    public interface IGenericPersist
    {
        void Add<T>(T entity) where T: class;
        void Update<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        void DeleteRange<T>(T[] entityArray) where T: class;
        Task<bool> SaveChangesAsync();
    }
}