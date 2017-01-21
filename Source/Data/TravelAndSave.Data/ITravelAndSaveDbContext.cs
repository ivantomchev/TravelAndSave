namespace TravelAndSave.Data
{
    using Models;
    using System.Data.Entity;

    public interface ITravelAndSaveDbContext
    {
        DbSet<User> Users { get; set; }

        DbSet<Trip> Trips { get; set; }

        DbSet<Location> Locations { get; set; }

        int SaveChanges();
    }
}
