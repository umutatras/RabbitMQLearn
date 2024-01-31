using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.ExcelWebApp.Hubs;
using RabbitMQ.ExcelWebApp.Models;
using RabbitMQ.ExcelWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(sp => new ConnectionFactory() { Uri = new Uri(builder.Configuration.GetConnectionString("RabbitMQ")), DispatchConsumersAsync = true });
builder.Services.AddSingleton<RabbitMQClientService>();
builder.Services.AddSingleton<RabbitMQPublisher>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});
builder.Services.AddIdentity<IdentityUser,IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    
}).AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();


// Add services to the container.


var app = builder.Build();
using var scope = app.Services.CreateScope();
var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
appDbContext.Database.Migrate();
if (!appDbContext.Users.Any())
{
    userManager.CreateAsync(new IdentityUser()
    {
        UserName = "deneme",
        Email = "deneme@gmail.com",

    }, "Password12*").Wait();
    userManager.CreateAsync(new IdentityUser()
    {
        UserName = "deneme2",
        Email = "deneme2@gmail.com",

    }, "Password12*").Wait();
}
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
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<MyHub>("/myhub");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
