//Builder
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();



//App
var app = builder.Build();
app.MapControllerRoute("defaut", "{controller=Home}/{action=Index}");
app.UseStaticFiles();


app.Run();
