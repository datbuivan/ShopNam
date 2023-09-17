using Microsoft.EntityFrameworkCore;
using QLBanQuanAoNam.Models;
using QLBanQuanAoNam.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("QlshopNamContext");
builder.Services.AddDbContext<QlshopNamContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddScoped<ILoaiSpRepository, LoaiSpRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(cfg =>
{
    cfg.Cookie.Name = "nhom3";
    cfg.IOTimeout = new TimeSpan(0, 30, 0);
});
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
app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
