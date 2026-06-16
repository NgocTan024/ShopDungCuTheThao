using SportShop.Data;
using SportShop.Models.Interfaces;
using SportShop.Models.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Kết nối cơ sở dữ liệu
builder.Services.AddDbContext<SportShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

// Hệ thống xác thực danh tính người dùng Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddDefaultUI()
    .AddEntityFrameworkStores<SportShopDbContext>()
    .AddDefaultTokenProviders();

// Đăng ký dịch vụ Repositories vào IoC Container
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>(ShoppingCartRepository.GetCart);
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession(); // Chạy Session trước khi Routing

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();