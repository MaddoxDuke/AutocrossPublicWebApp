using AutocrossPublicWebApp.Models;

namespace AutocrossPublicWebApp.Interfaces {
    public interface IReadingRepository {

        bool Add(ReadingModel model);
        bool Delete(ReadingModel model);
        Task<IEnumerable<ReadingModel>> GetAll();
        bool Save();
    }
}
