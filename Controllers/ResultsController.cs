using AutocrossPublicWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using AutocrossPublicWebApp.Data;
using System.Net;
using AutocrossPublicWebApp.Repositories;

namespace AutocrossPublicWebApp.Controllers {
    public class ResultsController : Controller {
        // GET: ResultsController
        private readonly ApplicationDbContext _context;
        private readonly ReadingRepository _readingRepository;


        public ResultsController(ApplicationDbContext context, ReadingRepository readingRepository) {
            _context = context;
            _readingRepository = readingRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Results(ReadingModel modelVM) {

            var result = _readingRepository;
            if (ModelState.IsValid) {

                var model = new ReadingModel {
                    Name = modelVM.Name,
                    Year = modelVM.Year,
                    PaxRaw = modelVM.PaxRaw
                };

                _context.Add(model); // adds to db
                _context.Save();

                return RedirectToAction("Results");
            } else {
                ModelState.AddModelError("", "ReadingModel population failed");
            }
            return View(modelVM);
        }
        public async Task<IActionResult> Index(ReadingModel model) {
            var CurrentYear = DateTime.Now.Year;

            Console.WriteLine("\n\nIndex Action:\nName " + model.Name + "Year " + model.Year + "\n\n");

            if (model.Year < CurrentYear - 10 || model.Year > CurrentYear) {
                return BadRequest("Year Invalid: must be within the last 10 years.");
            }
            if (!ModelState.IsValid) {
                Console.WriteLine("ModelState error");
                return View("Index", model); // Return to the form with validation messages
            }

            //_readingService.setYearDoc(model.Year);
            //_readingService.setTrNthChild(model.Name);

            var results = new List<string>();
            _readingRepository.Output(results);

            return View(model);
        }

        // GET: ResultsController/Details/5
        public ActionResult Details(int id) {
            return View();
        }

        // GET: ResultsController/Create
        public ActionResult Create() {
            return View();
        }

        // POST: ResultsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection) {
            try {
                return RedirectToAction(nameof(Index));
            } catch {
                return View();
            }
        }

        // GET: ResultsController/Edit/5
        public ActionResult Edit(int id) {
            return View();
        }

        // POST: ResultsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection) {
            try {
                return RedirectToAction(nameof(Index));
            } catch {
                return View();
            }
        }

        // GET: ResultsController/Delete/5
        public ActionResult Delete(int id) {
            return View();
        }

        // POST: ResultsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection) {
            try {
                return RedirectToAction(nameof(Index));
            } catch {
                return View();
            }
        }
    }
}
