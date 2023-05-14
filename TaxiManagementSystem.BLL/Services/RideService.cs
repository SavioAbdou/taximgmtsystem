using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TaxiManagementSystem.BLL.Interfaces;
using TaxiManagementSystem.DAL.Interfaces;
using TaxiManagementSystem.DTO;

namespace TaxiManagementSystem.BLL.Services
{
    public class RideService : Service<Ride>, IRideService
    {
        private const double PRICE_PER_DISTANCE_MULTIPLIER = 0.5;//if you change this, do not forget to change it in JavaScript in Ride/Create.cshtml

        public RideService(IRepository<Ride> repository, IUnitOfWork uow) : base(repository, uow)
        {
        }

        public Ride FindById(Guid? id)
        {
            if (id == null) return null;

            return Repository.GetById(id.Value);
        }

        public IEnumerable<Ride> GetAllMine(string userId, bool? deleted, params Expression<Func<Ride, object>>[] includeProps)
        {
            return Repository.GetAll(deleted, includeProps).Where(x => x.UserId.Equals(userId));
        }

        public IEnumerable<Ride> GetAllByMyTaxi(Guid taxiId, bool? deleted, params Expression<Func<Ride, object>>[] includeProps)
        {
            return Repository.GetAll(deleted, includeProps).Where(x => x.TaxiId == taxiId);
        }

        public override Ride Create(Ride entity)
        {
            entity.EstimatedPrice = (decimal)(entity.Distance * PRICE_PER_DISTANCE_MULTIPLIER);
            return base.Create(entity);
        }

        public override void Update(Ride entity)
        {
            entity.EstimatedPrice = (decimal)(entity.Distance * PRICE_PER_DISTANCE_MULTIPLIER);
            base.Update(entity);
        }
    }
}