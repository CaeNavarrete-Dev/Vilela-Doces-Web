//Builder
using VLDocesWeb.Repositories;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddTransient<ICustomerRepository>(_ => 
    new CustomerDatabaseRepository(
        builder.Configuration.GetConnectionString("default")));
builder.Services.AddTransient<IAddressRepository>(_ => 
    new AddressDatabaseRepository(
        builder.Configuration.GetConnectionString("default")));
builder.Services.AddSingleton<IProductRepository, ProductMemoryRepository>();

//App
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.MapControllerRoute("defaut", "{controller=Home}/{action=Index}");

app.Run();
