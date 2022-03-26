using Ghost.Services;
using Ghost.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var AllowSpecificOrigins = "_allowFrontend";

builder.Services.AddCors(options => 
{
    options.AddPolicy(name: AllowSpecificOrigins,
    builder => {
        builder.WithOrigins("https://localhost:44456", 
            "http://localhost:52198")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddScoped<ILibraryService, LibraryService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(AllowSpecificOrigins);

app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");;

app.Run();
