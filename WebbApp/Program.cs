using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static WebbApp.Services.ContactService;
using WebbApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

builder.Services.AddDbContext<ApplicationContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("WebApp_Database")));

builder.Services.AddIdentity<UserEntity, IdentityRole>(x =>
{
    x.SignIn.RequireConfirmedAccount = false;
    x.User.RequireUniqueEmail = true;
    x.Password.RequiredLength = 8;

}).AddEntityFrameworkStores<ApplicationContext>();

builder.Services.ConfigureApplicationCookie(x =>
{
    x.LoginPath = "/signin";
    x.Cookie.HttpOnly = true;
    x.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    x.ExpireTimeSpan = TimeSpan.FromHours(1);
    x.SlidingExpiration = true;
});



/*
builder.Services.AddAuthentication().AddFacebook(x =>
{
    x.AppId = "394802060104809";
    x.AppSecret = "c73ced261f72947126f1e036c3ee3f53";
    x.Fields.Add("first_name");
    x.Fields.Add("last_name");
});


builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "950991653346-8agrslijabtv4ovo4tvtq1la0icp9rst.apps.googleusercontent.com";
    options.ClientSecret = "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";
});

*/














// Add the following line for ContactService registration
builder.Services.AddScoped<IContactService, ContactServices>();

builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<CourseService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStatusCodePagesWithReExecute("/Error/{0}");



app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Default}/{action=Home}/{id?}");

app.Run();











