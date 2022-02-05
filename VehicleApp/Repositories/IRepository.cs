using VehicleApp.Entities;

namespace VehicleApp.Repositories
{
    public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T> 
        where T : class, IEntity, new()
    {
    }
}