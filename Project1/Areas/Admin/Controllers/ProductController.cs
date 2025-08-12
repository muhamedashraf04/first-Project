using InternBook.DataAccess.Repository.IRepository;
using InternBook.Models;
using InternBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace InternBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment; 
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public ActionResult Index()
        {
            List<Product> ProductList= _unitOfWork.Product.GetAll(IncludeProperties:"Category").ToList();

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
                string WWWRootPath = _webHostEnvironment.WebRootPath;
                if (file != null) 
                {
                    string filename = Guid.NewGuid().ToString()+ Path.GetExtension(file.FileName);
                    string productpath=Path.Combine(WWWRootPath, @"Images\Product");
                    if(!string.IsNullOrEmpty(obj.Product.ImageURL))
                    {
                        var oldImageUrl = Path.Combine(WWWRootPath, obj.Product.ImageURL.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImageUrl))
                        {
                            System.IO.File.Delete(oldImageUrl);
                        }
                    }
                        
                    using (var filestream = new FileStream(Path.Combine(productpath, filename), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    obj.Product.ImageURL = @"\Images\Product\" + filename;
                }
                if (obj.Product.Id == 0) 
                {
                    _unitOfWork.Product.Add(obj.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                }
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
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var ProductList = _unitOfWork.Product.GetAll(IncludeProperties: "Category").ToList();
            return Json(new { data=ProductList});
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Product ProductToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if(ProductToBeDeleted==null)
            {
                return Json(new {success=false, Message="Error While Deleting"});
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                ProductToBeDeleted.ImageURL.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Product.Remove(ProductToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Deleted Successfully" });
        }
        #endregion
    }
}
