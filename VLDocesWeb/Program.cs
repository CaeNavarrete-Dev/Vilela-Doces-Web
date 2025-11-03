//Builder
using VLDocesWeb.Repositories;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
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

//App
var app = builder.Build();

var supportedCultures = new[] { new CultureInfo("en-US") };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),

    SupportedCultures = supportedCultures,

    SupportedUICultures = supportedCultures
};
app.UseRequestLocalization(localizationOptions);

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.MapControllerRoute("defaut", "{controller=Home}/{action=Index}/{id?}");

app.Run();
