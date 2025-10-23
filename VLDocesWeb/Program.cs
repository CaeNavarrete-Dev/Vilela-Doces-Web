//Builder
using VLDocesWeb.Repositories;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddSingleton<ICustomerRepository, CustomerMemoryRepository>();



//App
var app = builder.Build();


app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.MapControllerRoute("defaut", "{controller=Home}/{action=Index}");


app.Run();
