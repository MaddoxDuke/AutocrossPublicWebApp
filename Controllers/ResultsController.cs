using AutocrossPublicWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using AutocrossPublicWebApp.Services;

namespace AutocrossPublicWebApp.Controllers {
    public class ResultsController : Controller {
        // GET: ResultsController
        private readonly ReadingService _readingService;

        public ResultsController(ReadingService readingService) {
            _readingService = readingService;
        }
        public IActionResult Results() {
            var model = new ReadingModel {
                Name = "Molyneux, Matthew",
                Year = 2024
            };

            return View(model);
        }
        public async Task<IActionResult> Index() {


            var CurrentYear = DateTime.Now.Year;
            if (_readingService.Year < CurrentYear - 10 || _readingService.Year > CurrentYear) {
                return BadRequest("Year Invalid: must be within the last 10 years.");
            }

            _readingService.setYearDoc(_readingService.Year);
            _readingService.setTrNthChild(_readingService.Name);

            var results = new List<string>();

            _readingService.Output(results);

            return View(results);

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
