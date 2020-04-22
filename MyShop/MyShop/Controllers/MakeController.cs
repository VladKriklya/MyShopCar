using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Models;

namespace MyShop.Controllers
{
    [Authorize(Roles = "Admin,Executive")]
    public class MakeController : Controller
    {
        private readonly ShopDataContext _dataContext;

        public MakeController(ShopDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_dataContext.Makes.ToList());
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Make make)
        {
            if(ModelState.IsValid)
            {
                _dataContext.Add(make);
                _dataContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(make);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var make = _dataContext.Makes.Find(id);

            if(make == null)
            {
                return NotFound();
            }

            _dataContext.Makes.Remove(make);
            _dataContext.SaveChanges();

            return RedirectToAction("Index");

        }


        public IActionResult Edit(int id)
        {
            var make = _dataContext.Makes.Find(id);

            if (make == null)
            {
                return NotFound();
            }

            return View(make);

        }

        [HttpPost]
        public IActionResult Edit(Make make)
        {
            if (ModelState.IsValid)
            {
                _dataContext.Update(make);
                _dataContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(make);
        }



    }
}
