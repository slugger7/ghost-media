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
        public DbSet<User> Users { get; set; }
        public DbSet<FavouriteActor> FavouriteActors { get; set; }
        public DbSet<FavouriteVideo> FavouriteVideos { get; set; }
        public DbSet<Progress> Progress { get; set; }
        public DbSet<RelatedVideo> RelatedVideos { get; set; }

        public GhostContext(DbContextOptions<GhostContext> options) : base(options) { }

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
            modelBuilder.Entity<User>().ToTable("Users");

            var favouriteVideo = modelBuilder.Entity<FavouriteVideo>().ToTable("FavouriteVideos");
            favouriteVideo
              .HasOne<User>(x => x.User)
              .WithMany(x => x.FavouriteVideos);
            favouriteVideo
              .HasOne<Video>(x => x.Video)
              .WithMany(x => x.FavouritedBy);

            var favouriteActor = modelBuilder.Entity<FavouriteActor>().ToTable("FavouriteActors");
            favouriteActor
              .HasOne<User>(x => x.User)
              .WithMany(x => x.FavouriteActors);
            favouriteActor
              .HasOne<Actor>(x => x.Actor)
              .WithMany(x => x.FavouritedBy);

            var progress = modelBuilder.Entity<Progress>().ToTable("Progress");
            progress
              .HasOne<User>(x => x.User)
              .WithMany(x => x.VideoProgress);
            progress
              .HasOne<Video>(x => x.Video)
              .WithMany(x => x.WatchedBy);

            var relatedVideo = modelBuilder.Entity<RelatedVideo>().ToTable("RelatedVideos");
            relatedVideo
              .HasOne<Video>(v => v.Video)
              .WithMany(v => v.RelatedVideos);
            relatedVideo
              .HasOne<Video>(v => v.RelatedTo);
        }
    }
}