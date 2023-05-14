using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TaxiManagementSystem.DTO;

namespace TaxiManagementSystem.BLL.Interfaces
{
    public interface IService<T> where T : BaseEntity
    {
        T Create(T entity);
        void Delete(T entity);
        IEnumerable<T> GetAll(bool? deleted = false, params Expression<Func<T, object>>[] includeProps);
        void Update(T entity);
    }
}
