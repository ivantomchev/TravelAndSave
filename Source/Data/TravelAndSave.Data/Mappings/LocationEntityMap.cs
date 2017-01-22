namespace TravelAndSave.Data.Mappings
{
    using Models;
    using System.Data.Entity.ModelConfiguration;

    public class LocationEntityMap : EntityTypeConfiguration<Location>
    {
        public LocationEntityMap()
        {
            this.HasMany(l => l.Trips).WithRequired(t => t.EndLocation).HasForeignKey(t => t.EndLocationId).WillCascadeOnDelete(false);
            this.HasMany(l => l.Trips).WithRequired(t => t.StartLocation).HasForeignKey(t => t.StartLocationId).WillCascadeOnDelete(false);
        }
    }
}
