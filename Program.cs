using DN_8.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<InventoryRepository>();

var app = builder.Build();
DatabaseInitializer.Initialize();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Inventory/Index");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inventory}/{action=Index}/{id?}");

app.Run();
