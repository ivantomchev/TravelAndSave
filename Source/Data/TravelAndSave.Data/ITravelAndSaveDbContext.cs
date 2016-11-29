namespace TravelAndSave.Data
{
    using Models;
    using System.Data.Entity;

    public interface ITravelAndSaveDbContext
    {
        IDbSet<User> Users { get; set; }

        IDbSet<Trip> Trips { get; set; }

        IDbSet<Location> Locations { get; set; }

        int SaveChanges();
    }
}
