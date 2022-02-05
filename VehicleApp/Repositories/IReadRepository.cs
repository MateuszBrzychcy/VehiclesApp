using VehicleApp.Entities;

namespace VehicleApp.Repositories
{
    public interface IReadRepository<out T> where T : class, IEntity, new()
    {
        IEnumerable<T> GetAll();
        T? GetById(int id);
    }
}