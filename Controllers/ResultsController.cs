using AutocrossPublicWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using AutocrossPublicWebApp.Data;
using AutocrossPublicWebApp.ViewModels;
using AutocrossPublicWebApp.Interfaces;

namespace AutocrossPublicWebApp.Controllers {
    public class ResultsController : Controller {
        // GET: ResultsController
        private readonly ApplicationDbContext _context;
        private readonly IResultsRepository _resultsRepository;


        public ResultsController(ApplicationDbContext context, IResultsRepository resultsRepository) {
            _context = context;
            _resultsRepository = resultsRepository;
        }
        public async Task<IActionResult> Results(EventResult modelVM) {
            Console.WriteLine("Results");

            return View(modelVM);
        }
        public async Task<IActionResult> Index() {

            IEnumerable<EventResult> eventResults = await _resultsRepository.GetAll(); //M: Db to events and builsd query/db and brings back.
            return View(eventResults); //V
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
                    Name = resultVM.Name,
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
