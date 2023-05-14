using System;
using System.Collections.Generic;
using TaxiManagementSystem.DTO;
using TaxiManagementSystem.DTO.Enums;

namespace TaxiManagementSystem.BLL.Interfaces
{
    public interface ITaxiService : IService<Taxi>
    {
        Taxi GetByDriverId(string driverId);
        IEnumerable<Taxi> GetAllByDriverId(string driverId);
        IEnumerable<Taxi> GetAllByType(TaxiTypes type);
        void AssignToDriver(ApplicationDriver driver, Taxi taxi);
        void RemoveFromDriver(Taxi taxi);
        Taxi GetById(Guid taxiId);
    }
}