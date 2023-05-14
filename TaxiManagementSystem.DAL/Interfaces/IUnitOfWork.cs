using System;
using System.Threading.Tasks;

namespace TaxiManagementSystem.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();
        Task<int> CommitAsync();
    }
}