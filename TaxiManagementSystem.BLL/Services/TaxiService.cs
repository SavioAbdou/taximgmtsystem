using System;
using System.Collections.Generic;
using TaxiManagementSystem.BLL.Interfaces;
using TaxiManagementSystem.DAL.Interfaces;
using TaxiManagementSystem.DTO;
using TaxiManagementSystem.DTO.Enums;

namespace TaxiManagementSystem.BLL.Services
{
    public class TaxiService : Service<Taxi>, ITaxiService
    {
        private readonly ITaxiRepository _taxiRepository;
        public TaxiService(ITaxiRepository taxiRepository, IRepository<Taxi> repository, IUnitOfWork uow) : base(repository, uow)
        {
            _taxiRepository = taxiRepository;
        }

        public Taxi GetByDriverId(string driverId)
        {
            return _taxiRepository.GetByDriverId(driverId);
        }

        public IEnumerable<Taxi> GetAllByDriverId(string driverId)
        {
            return Repository.GetAllWhere(x => x.DriverId.Equals(driverId));
        }

        public IEnumerable<Taxi> GetAllByType(TaxiTypes type)
        {
            return Repository.GetAllWhere(x => x.Type == type);
        }

        public void AssignToDriver(ApplicationDriver driver, Taxi taxi)
        {
            if (driver == null) throw new ArgumentNullException(nameof(driver));

            taxi.DriverId = driver.Id;

            base.Update(taxi);
        }

        public void RemoveFromDriver(Taxi taxi)
        {
            if (taxi == null) throw new ArgumentNullException(nameof(taxi));

            taxi.DriverId = null;

            base.Update(taxi);
        }

        public Taxi GetById(Guid taxiId)
        {
            return _taxiRepository.GetById(taxiId);
        }
    }
}