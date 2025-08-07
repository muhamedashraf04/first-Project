using InternBook.DataAccess.Repository.IRepository;
using InternBook.Models;
using InternBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ActionResult Index()
        {
            List<Product> ProductList= _unitOfWork.Product.GetAll().ToList();

            return View(ProductList);
        }
        public ActionResult Upsert(int? id)
        {
            ProductVM ProductVM = new()
            {
                CategoryLists = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                Text = u.Name,
                Value = u.Id.ToString(),
                }),
            Product = new Product()
            };
            if(id==null || id==0)
            {
                //create
                return View(ProductVM);
            }
            else
            {
                //update
                ProductVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(ProductVM);
            }
        }
        [HttpPost]
        public ActionResult Upsert (ProductVM obj,IFormFile? file)
        {
            if (obj.Product.Title.ToLower() == obj.Product.ISBN.ToLower())
            {
                ModelState.AddModelError("Title", "Title and ISBN Order cannot be the same");
            }
            if (obj.Product.Title.ToLower() == obj.Product.Author.ToLower())
            {
                ModelState.AddModelError("Title", "Title and Author Order cannot be the same");
            }
            if (obj.Product.ListPrice == obj.Product.Price50)
            {
                ModelState.AddModelError("List Price", "List Price and Price above 50 orders cannot be the same");
            }
            if (obj.Product.Price50 == obj.Product.Price100)
            {
                ModelState.AddModelError("Price50", "Price above 50 orders and Price above 100 orders cannot be the same");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product added successfully";
                return RedirectToAction("Index");
            }
            else
            {
                obj.CategoryLists = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                });
                
                return View(obj);
            }
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? ProdFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (ProdFromDb == null)
            {
                return NotFound();
            }
            return View(ProdFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (id == null || id == 0)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category Deleted successfully";
            return RedirectToAction("Index");
        }

    }
}
