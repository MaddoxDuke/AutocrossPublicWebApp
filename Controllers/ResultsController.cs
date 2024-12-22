using AutocrossPublicWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutocrossPublicWebApp.Controllers {
    public class ResultsController : Controller {
        // GET: ResultsController
        public async Task<IActionResult> Index(ReadingController reading, ReadingModel readingModel) {

            int CurrentYear = DateTime.Now.Year;
            // if (readingModel.Year < CurrentYear - 10 || readingModel.Year > CurrentYear) return ModelState.AddModelError("", "Year Invalid: must be within the last 10 years.");

            reading.setYearDoc(readingModel.Year);
            reading.setTrNthChild(readingModel.Name);

            IEnumerable<string> results = Enumerable.Empty<string>();

            //Add valid results here to the IEnumerable, results

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
