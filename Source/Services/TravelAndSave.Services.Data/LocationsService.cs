namespace TravelAndSave.Services.Data
{
    using Common.Models;
    using Interfaces;
    using System.Collections.Generic;
    using System.Linq;
    using TravelAndSave.Data.Models;
    using TravelAndSave.Data.Repositories;
    using TravelAndSave.Data.Repositories.Base;

    public class LocationsService : ILocationsService
    {
        private readonly IDbRepository<Location> locationsRepository;

        public LocationsService(IDbRepository<Location> locationsRepository)
        {
            this.locationsRepository = locationsRepository;
        }

        public LocationsService()
            : this(new LocationsRepository())
        {
        }

        public Result<Location> GetById(int id)
        {
            var location = this.locationsRepository.GetById(id);
            if (location == null)
            {
                return Result.Fail<Location>(string.Format("Location with id {0} doesn't exist.", id), 404);
            }

            return Result.Ok(location);
        }

        public Result Add(Location location, bool saveChanges = true)
        {
            this.locationsRepository.Add(location);
            if (saveChanges)
            {
                this.locationsRepository.SaveChanges();
            }

            return Result.Ok();
        }

        public Result Update(Location location, bool saveChanges = true)
        {
            var locationResult = this.GetById(location.Id);
            if (locationResult.IsFailure)
            {
                return Result.Fail(locationResult);
            }

            this.locationsRepository.Update(location);
            if (saveChanges)
            {
                this.locationsRepository.SaveChanges();
            }

            return Result.Ok();
        }

        public Result Detete(int id, bool saveChanges = true)
        {
            var locationResult = this.GetById(id);
            if (locationResult.IsFailure)
            {
                return Result.Fail(locationResult);
            }

            this.locationsRepository.Delete(locationResult.Value);
            if (saveChanges)
            {
                this.locationsRepository.SaveChanges();
            }

            return Result.Ok();
        }

        public Result<IEnumerable<Location>> GetAll()
        {
            var locations = this.locationsRepository.All().AsEnumerable();

            return Result.Ok(locations);
        }
    }
}
