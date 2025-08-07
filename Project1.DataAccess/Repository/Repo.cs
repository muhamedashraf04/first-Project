using Microsoft.EntityFrameworkCore;
using InternBook.DataAccess.Data;
using InternBook.DataAccess.Repository.IRepository;
using System.Linq.Expressions;
using System.Collections.Generic;
namespace InternBook.DataAccess.Repository
{
    public class Repo<T> : IRepo<T> where T : class
    {
        private readonly ApplicationDBContext _db;
        DbSet<T> dbSet;
        public Repo(ApplicationDBContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query= query.Where(filter);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
