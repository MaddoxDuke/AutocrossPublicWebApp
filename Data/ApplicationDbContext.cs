using Microsoft.EntityFrameworkCore;
using AutocrossPublicWebApp.Models;

namespace AutocrossPublicWebApp.Data {
    public class ApplicationDbContext : DbContext {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {

        }
        //Db Tables
        public DbSet<EventResult> EventResults { get; set; }
    }
}
