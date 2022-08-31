using DAL;
using Microsoft.EntityFrameworkCore;
using CalculatorLogic;
using CalculatorLogic.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<CalculatorDBContext>(options => {

    var connectionString = builder.Configuration.GetConnectionString("CalculatorCS");
    options.UseSqlServer(connectionString);

});
builder.Services.AddTransient<ICalculatorService, CalculatorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
