using System.Threading.Tasks;
using Weather.Data.Enums;

namespace Weather.Data.Interface
{
    public interface IGenericUoW
    {
        IRepository<T> Repository<T>() where T : class;
        Task<bool> SaveAsync(long userId, EnuAction actionType, string entityName);
        bool Save(long userId, EnuAction actionType, string entityName);
        void DetachAllEntities();
    }
}
