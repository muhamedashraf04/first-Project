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
    public class ProductRepository : Repo<Product>, IProductRepository
    {
        private ApplicationDBContext _db;
        public ProductRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Product obj)
        {
            _db.Products.Update(obj);
        }
    }
}
