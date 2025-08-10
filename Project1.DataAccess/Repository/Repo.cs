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
            _db.Products.Include(u => u.Category).Include(u => u.CategoryId);
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? IncludeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query= query.Where(filter);
            if (!string.IsNullOrEmpty(IncludeProperties))
            {
                foreach (var property in IncludeProperties.
                    Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(string? IncludeProperties=null)
        {
            IQueryable<T> query = dbSet;
            if(!string.IsNullOrEmpty(IncludeProperties))
            {
                foreach (var property in IncludeProperties.
                    Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
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
