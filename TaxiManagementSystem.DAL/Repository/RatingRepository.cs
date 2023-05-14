using TaxiManagementSystem.DAL.Interfaces;
using TaxiManagementSystem.DTO;

namespace TaxiManagementSystem.DAL.Repository
{
    public class RatingRepository : Repository<Rating>, IRatingRepository
    {
        public RatingRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}