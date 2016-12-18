namespace TravelAndSave.Services.Data.Interfaces
{
    using Common.Models;
    using System.Collections.Generic;
    using TravelAndSave.Data.Models;

    public interface ITripsService
    {
        Result<Trip> GetById(int id);

        Result Add(Trip trip, bool saveChanges = true);

        Result Update(Trip trip, bool saveChanges = true);

        Result Detete(int id, bool saveChanges = true);

        Result<IEnumerable<Trip>> GetAll();
    }
}
