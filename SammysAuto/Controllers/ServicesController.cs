using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SammysAuto.Data;
using SammysAuto.ViewModels;

namespace SammysAuto.Controllers
{
    public class ServicesController : Controller
    {

        private readonly ApplicationDbContext _db;

        public ServicesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }


        //Get Services/Create
        public IActionResult Create(int carId)
        {
            var model = new CarAndServicesViewModel
            {
                carObj = _db.Cars.FirstOrDefault(c => c.Id == carId),
                ServiceTypesObj = _db.ServiceTypes.ToList(),
                PastSericesObj = _db.Services.Where(s => s.CarId == carId).OrderByDescending(s => s.DateAdded).Take(5)
            };

            return View(model);
        }

        //POST: Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarAndServicesViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.NewServiceObj.CarId = model.carObj.Id;
                model.NewServiceObj.DateAdded = DateTime.Now;
                _db.Services.Add(model.NewServiceObj);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Create), new {carId = model.carObj.Id});
            }

            var newModel = new CarAndServicesViewModel
            {
                carObj = _db.Cars.FirstOrDefault(c => c.Id == model.carObj.Id),
                ServiceTypesObj = _db.ServiceTypes.ToList(),
                PastSericesObj = _db.Services.Where(s => s.CarId == model.carObj.Id).OrderByDescending(s => s.DateAdded).Take(5)
            };

            return View(newModel);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
        }
    }
}