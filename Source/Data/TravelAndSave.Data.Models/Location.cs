namespace TravelAndSave.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Location
    {
        private ICollection<Trip> trips;

        public Location()
        {
            this.trips = new HashSet<Trip>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Trip> Trips
        {
            get
            {
                return this.trips;
            }
            set
            {
                this.trips = value;
            }
        }
    }
}
