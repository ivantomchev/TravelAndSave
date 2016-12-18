namespace TravelAndSave.Services.Data.Interfaces
{
    using Common.Models;
    using System.Collections.Generic;
    using TravelAndSave.Data.Models;

    public interface ILocationsService
    {
        Result<Location> GetById(int id);

        Result Add(Location location, bool saveChanges = true);

        Result Update(Location location, bool saveChanges = true);

        Result Detete(int id, bool saveChanges = true);

        Result<IEnumerable<Location>> GetAll();
    }
}
