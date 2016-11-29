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

            var users = new List<User>()
            {
                new User
                {
                    FirstName = "Ivan",
                    LastName = "Tomchev",
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
