namespace TravelAndSave.Data
{
    using Mappings;
    using Models;
    using System.Data.Entity;
    using System.Threading;
    using System.Threading.Tasks;

    public class TravelAndSaveDbContext : DbContext, ITravelAndSaveDbContext
    {
        public TravelAndSaveDbContext()
            : base("DefaultConnection")
        {
        }

        public static TravelAndSaveDbContext Create()
        {
            return new TravelAndSaveDbContext();
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Trip> Trips { get; set; }

        public virtual DbSet<Location> Locations { get; set; }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new LocationEntityMap());
            //modelBuilder.Entity<Location>().HasMany(l => l.Trips).WithRequired(t => t.EndLocation).HasForeignKey(t => t.EndLocationId).WillCascadeOnDelete(false);
            //modelBuilder.Entity<Location>().HasMany(l => l.Trips).WithRequired(t => t.StartLocation).HasForeignKey(t => t.StartLocationId).WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
