namespace TravelAndSave.Data.UnitOfWork
{
    using Models;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class TravelAndSaveData : ITravelAndSaveData
    {
        private readonly ITravelAndSaveDbContext context;

        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public TravelAndSaveData()
            : this(new TravelAndSaveDbContext())
        {
        }

        public TravelAndSaveData(ITravelAndSaveDbContext context)
        {
            this.context = context;
        }

        public ITravelAndSaveDbContext Context
        {
            get
            {
                return this.context;
            }
        }

        public IDbRepository<User> Users
        {
            get { return this.GetRepository<User>(); }
        }

        public IDbRepository<Location> Locations
        {
            get { return this.GetRepository<Location>(); }
        }

        public IDbRepository<Trip> Trips
        {
            get { return this.GetRepository<Trip>(); }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return this.Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.context != null)
                {
                    this.context.Dispose();
                }
            }
        }

        private IDbRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(DbRepository<T>);
                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IDbRepository<T>)this.repositories[typeof(T)];
        }
    }
}
