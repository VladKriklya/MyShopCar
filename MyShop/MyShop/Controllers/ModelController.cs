using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;
using MyShop.ViewModels;

namespace MyShop.Controllers
{
    [Authorize(Roles = "Admin,Executive")]
    public class ModelController : Controller
    {
        private readonly ShopDataContext _dataContext;
        
        [BindProperty]
        public ModelViewModel ModelVM { get; set; }

        public ModelController(ShopDataContext dataContext)
        {
            _dataContext = dataContext;
            ModelVM = new ModelViewModel()
            {
                Makes = _dataContext.Makes.ToList(),
                Model = new Model()
            };
        }

        public IActionResult Index()
        {
            var model = _dataContext.Models.Include(m => m.Make);
            return View(model);
        }

        public IActionResult Create()
        {
            return View(ModelVM);
        }

        [HttpPost,ActionName("Create")]
        public IActionResult CreatePost()
        {
            if(!ModelState.IsValid)
            {
                return View(ModelVM);
            }

            _dataContext.Models.Add(ModelVM.Model);
            _dataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            ModelVM.Model = _dataContext.Models.Include(m => m.Make).SingleOrDefault(m => m.Id == id);
            if(ModelVM.Model == null)
            {
                return NotFound();
            }

            return View(ModelVM);
        }
        [HttpPost,ActionName("Edit")]
        public IActionResult EditPost()
        {
           if(!ModelState.IsValid)
           {
                return View(ModelVM);
           }

            _dataContext.Update(ModelVM.Model);
            _dataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var model = _dataContext.Models.Find(id);

            if (model == null)
            {
                return NotFound();
            }

            _dataContext.Models.Remove(model);
            _dataContext.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
