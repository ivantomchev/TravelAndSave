namespace TravelAndSave.Data.Repositories
{
    using Models;
    using Base;

    public class TripsRepository : DbRepository<Trip>, IDbRepository<Trip>
    {
    }
}
