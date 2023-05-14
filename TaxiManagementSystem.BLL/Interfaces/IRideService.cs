using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TaxiManagementSystem.DTO;

namespace TaxiManagementSystem.BLL.Interfaces
{
    public interface IRideService : IService<Ride>
    {
        Ride FindById(Guid? id);
        IEnumerable<Ride> GetAllMine(string userId, bool? deleted = false, params Expression<Func<Ride, object>>[] includeProps);
        IEnumerable<Ride> GetAllByMyTaxi(Guid taxiId, bool? deleted = false, params Expression<Func<Ride, object>>[] includeProps);
    }
}