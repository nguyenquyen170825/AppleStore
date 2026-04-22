using Microsoft.EntityFrameworkCore;
using DUANCUAHANGAPPLE.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddHostedService<DUANCUAHANGAPPLE.Services.RssBackgroundService>();
builder.Services.AddScoped<DUANCUAHANGAPPLE.Services.OtpService>();

builder.Services.AddScoped<DUANCUAHANGAPPLE.Services.SendEmailService>();

// Đăng ký Admin Services
builder.Services.AddScoped<DUANCUAHANGAPPLE.Services.IOrderService, DUANCUAHANGAPPLE.Services.OrderService>();
builder.Services.AddScoped<DUANCUAHANGAPPLE.Services.IProductService, DUANCUAHANGAPPLE.Services.ProductService>();
builder.Services.AddScoped<DUANCUAHANGAPPLE.Services.IDanhMucService, DUANCUAHANGAPPLE.Services.DanhMucService>();
builder.Services.AddScoped<DUANCUAHANGAPPLE.Services.IUserService, DUANCUAHANGAPPLE.Services.UserService>();
builder.Services.AddScoped<DUANCUAHANGAPPLE.Services.IPictureService, DUANCUAHANGAPPLE.Services.PictureService>();
builder.Services.AddScoped<DUANCUAHANGAPPLE.Services.INewsService, DUANCUAHANGAPPLE.Services.NewsService>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Home/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
})
.AddCookie("ExternalCookies")
.AddGoogle(options =>
{
    options.SignInScheme = "ExternalCookies";
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.SaveTokens = true;
    options.Events.OnRedirectToAuthorizationEndpoint = context =>
    {
        context.Response.Redirect(context.RedirectUri + "&prompt=select_account");
        return Task.CompletedTask;
    };
});
var app = builder.Build();

// Dùng Migration để tạo bảng trong database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers(); 
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
