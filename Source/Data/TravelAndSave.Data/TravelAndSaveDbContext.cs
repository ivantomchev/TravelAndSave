namespace TravelAndSave.Data
{
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

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
