namespace TravelAndSave.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IDbRepository<T> : IDisposable where T : class
    {
        IQueryable<T> All(bool enableTrackChanges = true);

        T GetById(object id);

        void Add(T entity);

        void AddRange(params T[] entities);

        void Update(T entity);

        void Delete(T entity);

        void Delete(object id);

        void DeleteRange(params T[] entities);

        T Attach(T entity);

        void Detach(T entity);

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
