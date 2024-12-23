using AutocrossPublicWebApp.Interfaces;
using AutocrossPublicWebApp.Models;

namespace AutocrossPublicWebApp.Repositories {
    public class ReadingRepository : IReadingRepository {
        public bool Add(ReadingModel model) {
            _context.Add(model); //generates sql and does not actually add until save().
            return Save();
        }

        public bool Delete(ReadingModel model) {
            _context.Remove(model);
            return Save();
        }

        public async Task<IEnumerable<ReadingModel>> GetAll() {
            // returns a list. Reason is because it is generating a page
            return await ReadingModel.ToListAsync();
        }
        public bool Save() {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
