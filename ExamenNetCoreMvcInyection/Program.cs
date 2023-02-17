using ExamenNetCoreMvcInyection.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//INYECCICON
//builder.Services.AddTransient<IRepositoryComic, RepositoryComic>();
builder.Services.AddTransient<IRepositoryComic, RepositoryComicOracle>();
builder.Services.AddControllersWithViews();

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
    pattern: "{controller=Comic}/{action=Index}/{id?}");

app.Run();
