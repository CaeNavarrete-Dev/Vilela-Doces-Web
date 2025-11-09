//Builder
using VLDocesWeb.Repositories;
using VLDocesWeb.Models;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddTransient<ICustomerRepository>(_ => 
    new CustomerDatabaseRepository(
        builder.Configuration.GetConnectionString("default")));
builder.Services.AddTransient<IAddressRepository>(_ => 
    new AddressDatabaseRepository(
        builder.Configuration.GetConnectionString("default")));
builder.Services.AddTransient<IProductRepository>(_ =>
    new ProductDatabaseRepository(
        builder.Configuration.GetConnectionString("default")));
builder.Services.AddTransient<IProdCategorieRepository>(_ =>
    new ProdCategorieDatabaseRepository(
        builder.Configuration.GetConnectionString("Default")));
builder.Services.AddTransient<IOrderRepository>(_ =>
    new OrderRepository(
        builder.Configuration.GetConnectionString("default")));

//App
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.MapControllerRoute("defaut", "{controller=Home}/{action=Index}");

app.Run();
