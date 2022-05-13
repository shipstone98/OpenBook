using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Shipstone.OpenBook.Data;
using Shipstone.OpenBook.Models;

namespace Shipstone.OpenBook
{
    internal static class Program
    {
        private static int Main(String[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            if (builder.Environment.IsDevelopment())
            {
                String connectionString = builder.Configuration.
                    GetConnectionString("OpenBookSqlite");

                builder.Services.AddDbContext<Context>(options =>
                    options.UseSqlite(connectionString));

                builder.Services.AddEndpointsApiExplorer();
            }

            else
            {
                String connectionString = builder.Configuration.
                    GetConnectionString("OpenBookSqlServer");

                builder.Services.AddDbContext<Context>(options =>
                    options.UseSqlServer(connectionString));
            }

            builder.Services.AddIdentity<User, IdentityRole<int>>()
                .AddEntityFrameworkStores<Context>();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = Internals._Lockout;
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Password.RequiredLength = Internals._PasswordMinLength;
                options.User.AllowedUserNameCharacters = Internals._UserNameAllowedCharacters;
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = Internals._Lockout;
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.SlidingExpiration = true;
            });

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }

            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
            return 0;
        }
    }
}
