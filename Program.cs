using Microsoft.EntityFrameworkCore;
using AutocrossPublicWebApp.Interfaces;
using AutocrossPublicWebApp.Data;
using AutocrossPublicWebApp.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IResultsRepository, ResultsRepository>();
builder.Services.AddDbContext<ApplicationDbContext>(options => //adds the Database into the application
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
