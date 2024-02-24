using Microsoft.EntityFrameworkCore;
using VendaLanches.Context;
using VendaLanches.Models;
using VendaLanches.Repositories;
using VendaLanches.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services
    .AddDbContext<AppDbContext>(options =>
        options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

builder.Services.AddTransient<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddTransient<ILancheRepository, LancheRepository>();
builder.Services.AddTransient<IPedidoRepository, PedidoRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped(sp => CarrinhoCompra.GetCarrinho(sp));

builder.Services.AddMemoryCache();
builder.Services.AddSession();

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
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "categoriaFiltro",
    pattern: "Lanche/{action}/{categoria?}",
    defaults: new { Controller = "Lanche", action = "List" });

    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
