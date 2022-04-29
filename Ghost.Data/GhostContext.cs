using Ghost.Data;
using Microsoft.EntityFrameworkCore;

namespace Ghost.Data
{
  public class GhostContext : DbContext
  {
    public DbSet<Library> Libraries { get; set; }
    public DbSet<LibraryPath> LibraryPaths { get; set; }
    public DbSet<Video> Videos { get; set; }
    public DbSet<VideoGenre> VideoGenres { get; set; }
    public DbSet<VideoActor> VideoActors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Actor> Actors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlite("Data Source=Ghost.db;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Library>().ToTable("Libraries");
      modelBuilder.Entity<LibraryPath>().ToTable("LibraryPaths");
      modelBuilder.Entity<Video>().ToTable("Videos");
      var videoGenre = modelBuilder.Entity<VideoGenre>().ToTable("VideoGenres");
      videoGenre
        .HasOne<Video>(x => x.Video)
        .WithMany(x => x.VideoGenres);
      videoGenre
        .HasOne<Genre>(x => x.Genre)
        .WithMany(x => x.VideoGenres);
      var videoActor = modelBuilder.Entity<VideoActor>().ToTable("VideoActors");
      videoActor
        .HasOne<Video>(x => x.Video)
        .WithMany(x => x.VideoActors);
      videoActor
        .HasOne<Actor>(x => x.Actor)
        .WithMany(x => x.VideoActors);
      modelBuilder.Entity<Genre>().ToTable("Genres");
      modelBuilder.Entity<Actor>().ToTable("Actors");
    }
  }
}