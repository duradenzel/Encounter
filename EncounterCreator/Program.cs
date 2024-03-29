using EncounterBLL.Factories;
using EncounterDAL;
using EncounterInterfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register your services here.
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();
builder.Services.AddScoped<DataAccessFactory>();
builder.Services.AddScoped<EncounterService>();
builder.Services.AddScoped<PlayerService>();
builder.Services.AddScoped<IMonsterApiService, MonsterApi>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
