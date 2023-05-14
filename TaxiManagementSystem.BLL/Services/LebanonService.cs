using System.Collections.Generic;
using TaxiManagementSystem.BLL.Interfaces;
using TaxiManagementSystem.DAL.Interfaces;
using TaxiManagementSystem.DTO;

namespace TaxiManagementSystem.BLL.Services
{
    public class LebanonService : Service<Lebanon>, ILebanonService
    {
        public LebanonService(IRepository<Lebanon> repository, IUnitOfWork uow) : base(repository, uow)
        {
        }

        public IEnumerable<Lebanon> GetAllByCity(string cityName)
        {
            return Repository.GetAllWhere(x => x.City.Equals(cityName));
        }

        public IEnumerable<Lebanon> GetAllByArea(string areaName)
        {
            return Repository.GetAllWhere(x => x.Area.Equals(areaName));
        }

        public IEnumerable<Lebanon> GetAllByDistrict(string districtName)
        {
            return Repository.GetAllWhere(x => x.District.Equals(districtName));
        }
    }
}