using System;
using System.Net;
using BusinessLayer.Services.Abstractions;
using BusinessLayer.Services.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repository.Abstractions;
using DataAccessLayer.Repository.Concretes;
using DataAccessLayer.UnitOfWorks;
using EntityLayer.Concrete;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
     .AddNToastNotifyToastr(new NToastNotify.ToastrOptions()
     {
         PositionClass = ToastPositions.TopRight,
         TimeOut = 3000
     })
    .AddRazorRuntimeCompilation();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Events.OnRedirectToLogin = context =>
    {
        if (!context.Request.Path.StartsWithSegments("/api") && !context.Response.HasStarted)
        {
            context.Response.Clear();
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            context.RedirectUri = "/Account/Login?ReturnUrl=" + context.Request.Path;
        }

        return Task.CompletedTask;
    };

    config.LoginPath = new PathString("/Home/Login");
    config.LogoutPath = new PathString("/Home/Logout");
    config.SlidingExpiration = true;
    config.ExpireTimeSpan = TimeSpan.FromDays(1);
    config.AccessDeniedPath = new PathString("/Home/AccessDenied");
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<Context>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IUserIdentityService, UserIdentityService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<ISurveyService, SurveyService>();

builder.Services.AddIdentity<AppUser, IdentityRole>()
        .AddEntityFrameworkStores<Context>()
        .AddDefaultTokenProviders();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Home/Login", "?statusCode={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Admin",
    pattern: "/Admin/{controller=Home}/{action=SurveyList}/{id?}",
    defaults: new { area = "Admin" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);
app.Run();
