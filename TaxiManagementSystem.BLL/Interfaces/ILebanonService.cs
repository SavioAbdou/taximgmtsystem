using System.Collections.Generic;
using TaxiManagementSystem.DTO;

namespace TaxiManagementSystem.BLL.Interfaces
{
    public interface ILebanonService : IService<Lebanon>
    {
        IEnumerable<Lebanon> GetAllByCity(string cityName);
        IEnumerable<Lebanon> GetAllByArea(string areaName);
        IEnumerable<Lebanon> GetAllByDistrict(string districtName);
    }
}