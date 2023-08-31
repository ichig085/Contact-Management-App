using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using ContactAppWeb.Data;
using ContactAppWeb;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Configure services for MVC and enable Razor runtime compilation
builder.Services.AddMvc().AddRazorRuntimeCompilation();

// Configure services for the DataContext using SQLite database provider
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add the DatabaseResetService as a hosted service
builder.Services.AddHostedService<DatabaseResetService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // Use the error handler middleware for non-development environments
    app.UseExceptionHandler("/Home/Error");

    // Enable HTTP Strict Transport Security (HSTS) for enhanced security (optional)
    app.UseHsts();
}

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

// Serve static files (e.g., CSS, JavaScript) from the wwwroot folder
app.UseStaticFiles();

// Enable routing for controllers and actions
app.UseRouting();

// Use authentication and authorization (if implemented)
app.UseAuthorization();

// Map the default route for the application, which uses the Contact controller and its Index action as the default route.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Contact}/{action=Index}/{id?}");

// Seed initial contacts
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<DataContext>();

    // Delete existing contacts and seed new ones
    DataSeeder.SeedInitialContacts(dbContext);
}

// Start processing incoming HTTP requests
app.Run();
