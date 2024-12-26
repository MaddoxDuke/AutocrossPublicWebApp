using AutocrossPublicWebApp.Models;

namespace AutocrossPublicWebApp.Interfaces {
    public interface IResultsRepository {

        bool Add(EventResult model);
        bool Delete(EventResult model);
        Task<IEnumerable<EventResult>> GetAll();
        bool Save();
    }
}
