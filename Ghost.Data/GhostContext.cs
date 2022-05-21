using System;
using Ghost.Data;
using Microsoft.EntityFrameworkCore;

namespace Ghost.Data
{
  public class GhostContext : DbContext
  {
    public DbSet<Library> Libraries { get; set; }
    public DbSet<LibraryPath> LibraryPaths { get; set; }
    public DbSet<Video> Videos { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<VideoGenre> VideoGenres { get; set; }
    public DbSet<VideoActor> VideoActors { get; set; }
    public DbSet<VideoImage> VideoImages { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Chapter> Chapters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      var dbPath = Environment.GetEnvironmentVariable("DATABASE_PATH") ?? $"..{Path.DirectorySeparatorChar}data{Path.DirectorySeparatorChar}Ghost.db";
      optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Library>().ToTable("Libraries");
      modelBuilder.Entity<LibraryPath>().ToTable("LibraryPaths");
      modelBuilder.Entity<Video>().ToTable("Videos");
      modelBuilder.Entity<Image>().ToTable("Images");
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
      var videoImage = modelBuilder.Entity<VideoImage>().ToTable("VideoImages");
      videoImage
        .HasOne<Video>(x => x.Video)
        .WithMany(x => x.VideoImages);
      videoImage
        .HasOne<Image>(x => x.Image)
        .WithMany(x => x.VideoImages);
      modelBuilder.Entity<Genre>().ToTable("Genres");
      modelBuilder.Entity<Actor>().ToTable("Actors");
      var chapter = modelBuilder.Entity<Chapter>().ToTable("Chapters");
      chapter
        .HasOne<Image>(x => x.Image);
      chapter
        .HasOne<Video>(x => x.Video)
        .WithMany(x => x.Chapters);
    }
  }
}