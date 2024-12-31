using AutocrossPublicWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using AutocrossPublicWebApp.Data;
using AutocrossPublicWebApp.ViewModels;
using AutocrossPublicWebApp.Interfaces;
using AutocrossPublicWebApp.Services;
using Microsoft.AspNetCore.Http;

namespace AutocrossPublicWebApp.Controllers {
    public class ResultsController : Controller {
        // GET: ResultsController
        private readonly ApplicationDbContext _context;
        private readonly IResultsRepository _resultsRepository;


        public ResultsController(ApplicationDbContext context, IResultsRepository resultsRepository) {
            _context = context;
            _resultsRepository = resultsRepository;
        }
        public async Task<IActionResult> Index(EventResultViewModel modelVM) {

            var service = new ReadingService();

            var readingModel = new ReadingModel {
                Name = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase((modelVM.LastName.Trim() + ", " + modelVM.FirstName.Trim()).ToLower()),
                Year = modelVM.Year,
                PaxRaw = modelVM.PaxRaw
            };

            service.Search(readingModel);

            return View(service.saveToEventResult());
        }

        // GET: ResultsController/Details/5
        public ActionResult Details(int id) {
            return View();
        }

        // GET: ResultsController/Create
        public async Task<IActionResult> Create(EventResultViewModel resultVM) {

            Console.WriteLine("Checking Model validity in Create");

            if (ModelState.IsValid) {
                Console.WriteLine("Model is valid");

                var model = new EventResult {
                    Name = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase((resultVM.LastName.Trim() + ", " + resultVM.FirstName.Trim()).ToLower()),
                    Year = resultVM.Year,
                    PaxRaw = resultVM.PaxRaw
                };
                
                _resultsRepository.Add(model); // adds to db
                return RedirectToAction("Results"); //This redirects, not the html page.
            } else {
                ModelState.AddModelError("", "ResultsModel population failed");
            }
            return RedirectToAction("Results");
            //return View(resultVM);
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
