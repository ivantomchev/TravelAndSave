namespace TravelAndSave.Data.Migrations
{
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<TravelAndSaveDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(TravelAndSaveDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            var locations = new List<Location>
            {
                new Location
                {
                    Name = "TestLocation"
                },
                new Location
                {
                    Name = "TestLocation2"
                }
            };

            locations.ForEach(x => context.Locations.AddOrUpdate(t => t.Id, x));
            context.SaveChanges();

            var users = new List<User>()
            {
                new User
                {
                    FirstName = "Ivan",
                    LastName = "Tomchev",
                    Trips = new List<Trip>
                    {
                        new Trip
                        {
                            OpenSeats = 2,
                            StartDate = System.DateTime.Now,
                            StartLocation = context.Locations.FirstOrDefault(),
                            EndLocation = context.Locations.FirstOrDefault()
                        },
                         new Trip
                        {
                            OpenSeats = 3,
                            StartDate = System.DateTime.Now,StartLocation = context.Locations.FirstOrDefault(),
                            EndLocation = context.Locations.FirstOrDefault()
                        },
                        new Trip
                        {
                            OpenSeats = 1,
                            StartDate = System.DateTime.Now,
                            StartLocation = context.Locations.FirstOrDefault(),
                            EndLocation = context.Locations.FirstOrDefault()
                        },
                    }
                },
                new User
                {
                    FirstName = "Georgi",
                    LastName = "Petrov",
                }
            };

            users.ForEach(x => context.Users.AddOrUpdate(t => t.Id, x));
            context.SaveChanges();
        }
    }
}
