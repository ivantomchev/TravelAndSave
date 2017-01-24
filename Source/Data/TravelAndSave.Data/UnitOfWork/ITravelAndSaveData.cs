namespace TravelAndSave.Data.UnitOfWork
{
    using Models;
    using Repositories;

    public interface ITravelAndSaveData
    {
        ITravelAndSaveDbContext Context { get; }

        IDbRepository<Location> Locations { get; }

        IDbRepository<Trip> Trips { get; }

        IDbRepository<User> Users { get; }

        int SaveChanges();
    }
}
