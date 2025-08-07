using InternBook.DataAccess.Repository.IRepository;
using InternBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternBook.DataAccess.Data;
using InternBook.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;

namespace InternBook.DataAccess.Repository
{
    public class CategoryRepository : Repo<Category>, ICategoryRepository
    {
        private ApplicationDBContext _db;
        public CategoryRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Category obj)
        {
            _db.Categories.Update(obj);
        }
    }
}
