﻿using InternBook.DataAccess.Data;
using InternBook.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternBook.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICategoryRepository? Category { get; private set; }
        public IProductRepository? Product { get; private set; }
        private ApplicationDBContext _db;
        public UnitOfWork(ApplicationDBContext db)
        {
            _db = db;
            Product= new ProductRepository(_db);
            Category = new CategoryRepository(_db);
        }
        

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
