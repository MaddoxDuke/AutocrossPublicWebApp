using AutocrossPublicWebApp.Data;
using AutocrossPublicWebApp.Interfaces;
using AutocrossPublicWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AutocrossPublicWebApp.Repositories {
    public class ResultsRepository : IResultsRepository {

        private readonly ApplicationDbContext _context;
        public ResultsRepository(ApplicationDbContext context) {
            _context = context;
        }
        public bool Add(EventResult model) {
            _context.Add(model); //generates sql and does not actually add until save().
            return Save();
        }

        public bool Delete(EventResult model) {
            _context.Remove(model);
            return Save();
        }

        public async Task<IEnumerable<EventResult>> GetAll() {
            // returns a list. Reason is because it is generating a page

            return await _context.EventResults.ToListAsync();
        }
        public bool Save() {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
