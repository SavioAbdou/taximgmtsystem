using System.Collections.Generic;
using TaxiManagementSystem.DAL.Interfaces;
using TaxiManagementSystem.DTO;

namespace TaxiManagementSystem.DAL.Repository
{
    public class LebanonRepository : Repository<Lebanon>, ILebanonRepository
    {
        public LebanonRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}