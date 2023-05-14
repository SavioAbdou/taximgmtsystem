using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TaxiManagementSystem.DTO;

namespace TaxiManagementSystem.DAL.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        T GetById(Guid id, params Expression<Func<T, object>>[] includeProps);
        T GetWhere(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAllWhere(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAll(bool? deleted = false, params Expression<Func<T, object>>[] includeProps);
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        int Save();
    }
}
