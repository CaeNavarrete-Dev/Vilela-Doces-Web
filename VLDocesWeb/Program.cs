//Builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

//App
var app = builder.Build();
app.UseStaticFiles();
app.MapControllerRoute("defaut", "{controller=Home}/{action=Index}");

app.Run();
