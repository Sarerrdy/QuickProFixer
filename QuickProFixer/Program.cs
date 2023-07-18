using Microsoft.EntityFrameworkCore;
using QuickProFixer.Models;
using QuickProFixer.Models.Context;
using QuickProFixer.Services;
using Microsoft.AspNetCore.Identity;
using QuickProFixer.Models.Entities;
using System;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IQuickProFixerRepository, QuickProFixerRepository>();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<QuickProFixerContext>(cfg =>
               //cfg.UseLazyLoadingProxies()
              cfg.UseSqlServer(builder.Configuration.GetConnectionString("QuickProFixerConnectionString")));

builder.Services.AddIdentity<User, IdentityRole<int>>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<QuickProFixerContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();
builder.Services.AddTransient<QuickProFixerSeeder>();

builder.Services.AddControllers(
    options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);


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
app.UseAuthentication();;

app.UseAuthorization();


if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var seeder = scope.ServiceProvider.GetService<QuickProFixerSeeder>();
        await seeder.Seed();
    }

}


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
