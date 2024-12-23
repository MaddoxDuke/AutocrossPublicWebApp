﻿using AutocrossPublicWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using AutocrossPublicWebApp.Services;
using System.Net;

namespace AutocrossPublicWebApp.Controllers {
    public class ResultsController : Controller {
        // GET: ResultsController
        private readonly ReadingService _readingService;

        public ResultsController(ReadingService readingService) {
            _readingService = readingService;
        }
        [HttpPost]
        public async Task<IActionResult> Results(ReadingModel modelVM) {

            var result = _readingService;
            if (ModelState.IsValid) {

                var model = new ReadingModel {
                    Name = modelVM.Name,
                    Year = modelVM.Year,
                    PaxRaw = modelVM.PaxRaw
                };

                _readingService.Add(model);

                return RedirectToAction("Results");
            } else {
                ModelState.AddModelError("", "ReadingModel population failed");
            }
            return View(modelVM);
        }
        public async Task<IActionResult> Index(ReadingModel model) {
            var CurrentYear = DateTime.Now.Year;

            Console.WriteLine("\n\nIndex Action:\nName " + model.Name + "Year " + model.Year + "\n\n");

            if (_readingService.Year < CurrentYear - 10 || _readingService.Year > CurrentYear) {
                return BadRequest("Year Invalid: must be within the last 10 years.");
            }
            if (!ModelState.IsValid) {
                Console.WriteLine("ModelState error");
                return View("Index", model); // Return to the form with validation messages
            }

            //_readingService.setYearDoc(model.Year);
            //_readingService.setTrNthChild(model.Name);

            var results = new List<string>();
            _readingService.Output(results);

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