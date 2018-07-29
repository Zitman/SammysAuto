using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SammysAuto.Data;
using SammysAuto.Models;
using SammysAuto.ViewModels;

namespace SammysAuto.Controllers
{
    public class CarsController : Controller
    {

        private readonly ApplicationDbContext _db;

        public CarsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(string userId = null)
        {
            if (userId == null)
                userId = this.User.FindFirstValue((ClaimTypes.NameIdentifier)); // Only used when a guest user logs in

            var model = new CarAndCustomerViewModel
            {
                Cars = _db.Cars.Where(c => c.UserId == userId),
                UserObj = _db.Users.FirstOrDefault(u => u.Id == userId)
            };

            return View(model);
        }

        //GET : Cars/Create
        public IActionResult Create(string userId)
        {
            Car car = new Car
            {
                Year = DateTime.Now.Year,
                UserId = userId
            };
            return View(car);
        }

        //POST : Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car)
        {
            if (ModelState.IsValid)
            {
                _db.Add(car);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new {userId = car.UserId});
            }

            return View(car);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
        }
    }
}