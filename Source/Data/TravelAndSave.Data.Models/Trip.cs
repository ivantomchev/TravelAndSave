namespace TravelAndSave.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Trip
    {
        [Key]
        public int Id { get; set; }

        public int OpenSeats { get; set; }

        public string AdditionalInfo { get; set; }

        public DateTime StartDate { get; set; }

        public int UserId { get; set; }

        public virtual User Users { get; set; }

        public int StartLocationId { get; set; }

        public virtual Location StartLocation { get; set; }

        public int EndLocationId { get; set; }

        public virtual Location EndLocation { get; set; }
    }
}
