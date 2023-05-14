using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TaxiManagementSystem.BLL.Interfaces;
using TaxiManagementSystem.DAL.Interfaces;
using TaxiManagementSystem.DTO;
using TaxiManagementSystem.DTO.Enums;

namespace TaxiManagementSystem.BLL.Services
{
    public abstract class Service<T> : IService<T> where T : BaseEntity
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IRepository<T> Repository;

        protected Service(IRepository<T> repository, IUnitOfWork uow)
        {
            Repository = repository;
            UnitOfWork = uow;
        }

        public virtual T Create(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            Repository.Add(entity);
            UnitOfWork.Commit();
            return entity;
        }

        public void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            if (entity is Taxi)
            {
                (entity as Taxi).TaxiStatus = TaxiStatus.NotInUse;
                (entity as Taxi).Type = TaxiTypes.NotInUse;
            }
            else if (entity is Ride)
            {
                (entity as Ride).RideStatus = RideStatus.NotInUse;
            }

            Repository.Delete(entity);
            UnitOfWork.Commit();
        }

        public IEnumerable<T> GetAll(bool? deleted = false, params Expression<Func<T, object>>[] includeProps)
        {
            return Repository.GetAll(deleted, includeProps);
        }

        public virtual void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            Repository.Update(entity);
            UnitOfWork.Commit();
        }
    }
}