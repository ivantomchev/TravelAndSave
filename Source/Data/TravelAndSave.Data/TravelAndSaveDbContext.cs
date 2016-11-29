namespace TravelAndSave.Data
{
    using Models;
    using System.Data.Entity;

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

        public virtual IDbSet<User> Users { get; set; }

        public virtual IDbSet<Trip> Trips { get; set; }

        public virtual IDbSet<Location> Locations { get; set; }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>().HasMany(l => l.Trips).WithRequired(t => t.EndLocation).HasForeignKey(t => t.EndLocationId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Location>().HasMany(l => l.Trips).WithRequired(t => t.StartLocation).HasForeignKey(t => t.StartLocationId).WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
