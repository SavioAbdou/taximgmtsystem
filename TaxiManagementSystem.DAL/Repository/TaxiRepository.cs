using TaxiManagementSystem.DAL.Interfaces;
using TaxiManagementSystem.DTO;

namespace TaxiManagementSystem.DAL.Repository
{
    public class TaxiRepository : Repository<Taxi>, ITaxiRepository
    {
        public TaxiRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Taxi GetByDriverId(string driverId)
        {
            return GetWhere(x => x.DriverId.Equals(driverId));
        }
    }
}