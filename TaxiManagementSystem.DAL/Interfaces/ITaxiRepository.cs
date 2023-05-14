using TaxiManagementSystem.DTO;

namespace TaxiManagementSystem.DAL.Interfaces
{
    public interface ITaxiRepository : IRepository<Taxi>
    {
        Taxi GetByDriverId(string driverId);
    }
}