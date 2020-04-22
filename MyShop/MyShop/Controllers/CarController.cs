using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.ViewModels;
using MyShop.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using cloudscribe.Pagination.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyShop.Controllers
{
    [Authorize(Roles = "Admin,Executive")]
    public class CarController : Controller
    {
        private readonly ShopDataContext _dataContext;
        private readonly IHostingEnvironment _hostEnvironment;

        [BindProperty]
        public CarViewModel CarVM { get; set; }

        public CarController(ShopDataContext dataContext, IHostingEnvironment hostEnvironment)
        {
            _dataContext = dataContext;
            _hostEnvironment = hostEnvironment;
            CarVM = new CarViewModel()
            {
                Makes = _dataContext.Makes.ToList(),
                Models = _dataContext.Models.ToList(),
                Car = new Car()
            };
        }


        [AllowAnonymous]
        public IActionResult Index(string searchString, string sortOrder, int pagenumber = 1, int pageSize = 3)
        {
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.CurrentFilter = searchString;
            ViewBag.PriceSortParam = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";

            int ExludeRecords = (pageSize * pagenumber) - pageSize;
            var cars = from b in _dataContext.Cars.Include(m => m.Make).Include(m => m.Model)
                       select b;
            var carCount = cars.Count();

            if (!String.IsNullOrEmpty(searchString))
            {
                cars = cars.Where(b => b.Make.Name.Contains(searchString));
                carCount = cars.Count();
            }

            switch(sortOrder)
            {
                case "price_desc":
                    cars = cars.OrderByDescending(b => b.Price);
                    break;
                default:
                    cars = cars.OrderBy(b => b.Price);
                    break;
            }
              cars = cars
                .Skip(ExludeRecords)
                .Take(pageSize);

            var result = new PagedResult<Car>
            {
                Data = cars.AsNoTracking().ToList(),
                TotalItems = carCount,
                PageNumber = pagenumber,
                PageSize = pageSize
            };


            return View(result);
        }


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult EditPost()
        {
            if (!ModelState.IsValid)
            {
                CarVM.Makes = _dataContext.Makes.ToList();
                CarVM.Models = _dataContext.Models.ToList();
                return View(CarVM);
            }

            _dataContext.Cars.Update(CarVM.Car);
            _dataContext.SaveChanges();
             UploadImage();
            _dataContext.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Create()
        {
            return View(CarVM);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost()
        {
            if (!ModelState.IsValid)
            {
                CarVM.Makes = _dataContext.Makes.ToList();
                CarVM.Models = _dataContext.Models.ToList();
                return View(CarVM);
            }

            _dataContext.Cars.Add(CarVM.Car);
            _dataContext.SaveChanges();
            UploadImage();
             _dataContext.SaveChanges();
            return RedirectToAction("Index");
        }
        private void UploadImage()
        {
            var CarId = CarVM.Car.Id;
            string wwwrootPath = _hostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var SavedCar = _dataContext.Cars.Find(CarId);

            if (files.Count != 0)
            {
                var ImagePath = @"images\car\";
                var Extension = Path.GetExtension(files[0].FileName);
                var RelativeImagePath = ImagePath + CarId + Extension;
                var AbsImagPath = Path.Combine(wwwrootPath, RelativeImagePath);

                using (var fileStream = new FileStream(AbsImagPath, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                SavedCar.ImagePath = RelativeImagePath;
            }
        }

        public IActionResult Edit(int id)
        {
            CarVM.Car = _dataContext.Cars.SingleOrDefault(b => b.Id == id);
            CarVM.Models = _dataContext.Models.Where(m => m.MakeID == CarVM.Car.MakeID);


            if(CarVM.Car == null)
            {
                return NotFound();
            }

            return View(CarVM);
        }

           

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var car = _dataContext.Cars.Find(id);

            if (car == null)
            {
                return NotFound();
            }

            _dataContext.Cars.Remove(car);
            _dataContext.SaveChanges();
            return RedirectToAction("Index");
        }

        

        [AllowAnonymous]
        public IActionResult View(int id)
        {
            CarVM.Car = _dataContext.Cars.FirstOrDefault(b => b.Id == id);
          
            if (CarVM.Car == null)
            {
                return NotFound();
            }

            return View(CarVM);
        }
    }
}
