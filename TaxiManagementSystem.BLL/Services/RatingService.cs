using TaxiManagementSystem.BLL.Interfaces;
using TaxiManagementSystem.DAL.Interfaces;
using TaxiManagementSystem.DTO;

namespace TaxiManagementSystem.BLL.Services
{
    public class RatingService : Service<Rating>, IRatingService
    {
        public RatingService(IRepository<Rating> repository, IUnitOfWork uow) : base(repository, uow)
        {
        }
    }
}