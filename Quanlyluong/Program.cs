using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quanlyluong.Data;
using Quanlyluong.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Quanlyluongnv>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("KetNoi")));

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Taikhoan/DangNhap";
        options.AccessDeniedPath = "/Taikhoan/KhongCoQuyen";

        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
    });

builder.Services.AddAuthorization();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<Quanlyluongnv>();

    var bamMatKhau = new PasswordHasher<Taikhoan>();

    if (!db.TaiKhoans.Any(t => t.TenDangNhap == "admin"))
    {
        var admin = new Taikhoan
        {
            TenDangNhap = "admin",
            Quyen = "Admin"
        };

        admin.MatKhauHash = bamMatKhau.HashPassword(
            admin, "123456");

        db.TaiKhoans.Add(admin);
        db.SaveChanges();
    }

    if (!db.TaiKhoans.Any(t => t.TenDangNhap == "nv01"))
    {
        var nhanvien = db.NhanViens
            .OrderBy(n => n.Id)
            .FirstOrDefault();

        if (nhanvien != null)
        {
            var taiKhoan = new Taikhoan
            {
                TenDangNhap = "nv01",
                Quyen = "NhanVien",
                NhanVienId = nhanvien.Id
            };

            taiKhoan.MatKhauHash = bamMatKhau.HashPassword(
                taiKhoan, "123456");

            db.TaiKhoans.Add(taiKhoan);
            db.SaveChanges();
        }
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();