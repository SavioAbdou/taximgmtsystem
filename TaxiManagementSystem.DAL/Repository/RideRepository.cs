using TaxiManagementSystem.DAL.Interfaces;
using TaxiManagementSystem.DTO;

namespace TaxiManagementSystem.DAL.Repository
{
    public class RideRepository : Repository<Ride>, IRideRepository
    {
        public RideRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}