using InternBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternBook.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepo<Category>
    {
        void Update(Category obj);
    }
}
