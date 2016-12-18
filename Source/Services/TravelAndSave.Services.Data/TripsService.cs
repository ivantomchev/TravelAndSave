namespace TravelAndSave.Services.Data
{
    using Common.Models;
    using Interfaces;
    using System.Collections.Generic;
    using System.Linq;
    using TravelAndSave.Data.Models;
    using TravelAndSave.Data.Repositories;
    using TravelAndSave.Data.Repositories.Base;

    public class TripsService : ITripsService
    {
        private readonly IDbRepository<Trip> tripsRepository;

        public TripsService(IDbRepository<Trip> tripsRepository)
        {
            this.tripsRepository = tripsRepository;
        }

        public TripsService()
            : this(new TripsRepository())
        {
        }

        public Result<Trip> GetById(int id)
        {
            var trip = this.tripsRepository.GetById(id);
            if (trip == null)
            {
                return Result.Fail<Trip>(string.Format("Trip with id {0} doesn't exist.", id), 404);
            }

            return Result.Ok(trip);
        }

        public Result Add(Trip trip, bool saveChanges = true)
        {
            this.tripsRepository.Add(trip);
            if (saveChanges)
            {
                this.tripsRepository.SaveChanges();
            }

            return Result.Ok();
        }

        public Result Update(Trip trip, bool saveChanges = true)
        {
            var tripResult = this.GetById(trip.Id);
            if (tripResult.IsFailure)
            {
                return Result.Fail(tripResult);
            }

            this.tripsRepository.Update(trip);
            if (saveChanges)
            {
                this.tripsRepository.SaveChanges();
            }

            return Result.Ok();
        }

        public Result Detete(int id, bool saveChanges = true)
        {
            var tripResult = this.GetById(id);
            if (tripResult.IsFailure)
            {
                return Result.Fail(tripResult);
            }

            this.tripsRepository.Delete(tripResult.Value);
            if (saveChanges)
            {
                this.tripsRepository.SaveChanges();
            }

            return Result.Ok();
        }

        public Result<IEnumerable<Trip>> GetAll()
        {
            var trips = this.tripsRepository.All().AsEnumerable();

            return Result.Ok(trips);
        }
    }
}
