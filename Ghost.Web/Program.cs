using Ghost.Data;
using Ghost.Media;
using Ghost.Repository;
using Ghost.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var AllowSpecificOrigins = "_allowFrontend";

builder.Services.AddCors(options =>
{
  options.AddPolicy(name: AllowSpecificOrigins,
  builder =>
  {
    builder
          .AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod();
  });
});
builder.Services.AddControllers();
// Services
builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<IDirectoryService, DirectoryService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IActorService, ActorService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IImageIoService, ImageIoService>();
builder.Services.AddScoped<INfoService, NfoService>();
builder.Services.AddScoped<IUserService, UserService>();

// Repositories
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IActorRepository, ActorRepository>();
builder.Services.AddScoped<IVideoRepository, VideoRepository>();
builder.Services.AddScoped<ILibraryRepository, LibraryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddDbContext<GhostContext>();
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

app.MapFallbackToFile("index.html"); ;

app.Run();
