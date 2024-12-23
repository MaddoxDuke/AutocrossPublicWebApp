using Microsoft.EntityFrameworkCore;
using AutocrossPublicWebApp.Models;

namespace AutocrossPublicWebApp.Data {
    public class ApplicationDbContext : DbContext {

        public ApplicatonDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {

        }
        //Db Tables
        public DbSet<ResultsModel> Results { get; set; }
    }
}
