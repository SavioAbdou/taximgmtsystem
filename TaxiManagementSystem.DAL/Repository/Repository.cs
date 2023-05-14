using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaxiManagementSystem.DAL.Interfaces;
using TaxiManagementSystem.DTO;

namespace TaxiManagementSystem.DAL.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext Context;
        protected IDbSet<T> Entities => Context.Set<T>();

        protected Repository(ApplicationDbContext context)
        {
            Context = context;
        }

        public T GetById(Guid id, params Expression<Func<T, object>>[] includeProps)
        {
            if (includeProps == null || includeProps.Length == 0) return Entities.Find(id);

            IQueryable<T> query = Entities.AsQueryable();

            foreach (Expression<Func<T, object>> prop in includeProps)
            {
                query = query.Include(prop);
            }
            return query.SingleOrDefault(x => x.Id == id);
        }

        public T GetWhere(Expression<Func<T, bool>> predicate)
        {
            return Entities.FirstOrDefault(predicate);
        }

        public IEnumerable<T> GetAllWhere(Expression<Func<T, bool>> predicate)
        {
            return Entities.Where(predicate).AsEnumerable();
        }

        public IEnumerable<T> GetAll(bool? deleted = false, params Expression<Func<T, object>>[] includeProps)
        {
            IQueryable<T> query = Entities.AsQueryable();
            foreach (Expression<Func<T, object>> prop in includeProps)
            {
                query = query.Include(prop);
            }

            if (deleted.HasValue)
            {
                return query.Where(x => x.IsDeleted == deleted.Value).OrderByDescending(x => x.CreatedDate).AsEnumerable();
            }
            return query.OrderByDescending(x => x.CreatedDate).AsEnumerable();
        }

        public T Add(T entity)
        {
            if (entity == null) throw new ArgumentException(nameof(entity));

            return Entities.Add(entity);
        }

        public void Update(T entity)
        {
            if (entity == null) throw new ArgumentException(nameof(entity));

            Entities.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            if (entity == null) throw new ArgumentException(nameof(entity));

            entity.IsDeleted = true;
            Context.Entry(entity).State = EntityState.Modified;
        }

        public int Save()
        {
            return Context.SaveChanges();
        }
    }
}
