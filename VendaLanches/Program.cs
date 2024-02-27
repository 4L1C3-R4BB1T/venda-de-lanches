using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using VendaLanches.Areas.Admin.Services;
using VendaLanches.Context;
using VendaLanches.Models;
using VendaLanches.Repositories;
using VendaLanches.Repositories.Interfaces;
using VendaLanches.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var appConnection = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(appConnection, ServerVersion.AutoDetect(appConnection));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddTransient<ILancheRepository, LancheRepository>();
builder.Services.AddTransient<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();
builder.Services.AddScoped<RelatorioVendasService>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Admin", policy => policy.RequireRole("Admin"));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped(sp => CarrinhoCompra.GetCarrinho(sp));

builder.Services.AddMemoryCache();
builder.Services.AddSession();

builder.Services.AddPaging(options =>
{
    options.ViewName = "Bootstrap5";
    options.PageParameterName = "pageindex";
});

var app = builder.Build();

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

app.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Add SeedUsers e SeedRoles
using var scope = app.Services.CreateScope();
var service = scope.ServiceProvider;
var seedService = service.GetRequiredService<ISeedUserRoleInitial>();
seedService.SeedRoles();
seedService.SeedUsers();

app.Run();
