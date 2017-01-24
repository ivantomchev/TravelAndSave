namespace TravelAndSave.Data
{
    using Models;
    using System.Data.Entity;
    using System.Threading.Tasks;

    public interface ITravelAndSaveDbContext
    {
        DbSet<User> Users { get; set; }

        DbSet<Trip> Trips { get; set; }

        DbSet<Location> Locations { get; set; }

        int SaveChanges();

        Task<int> SaveChangesAsync();

        void Dispose();
    }
}
