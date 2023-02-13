﻿// <auto-generated />
using System;
using Ghost.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Ghost.Data.Migrations
{
    [DbContext(typeof(GhostContext))]
    partial class GhostContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.4");

            modelBuilder.Entity("Ghost.Data.Actor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Actors", (string)null);
                });

            modelBuilder.Entity("Ghost.Data.Chapter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ImageId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Timestamp")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VideoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.HasIndex("VideoId");

                    b.ToTable("Chapters", (string)null);
                });

            modelBuilder.Entity("Ghost.Data.FavouriteActor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ActorId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ActorId");

                    b.HasIndex("UserId");

                    b.ToTable("FavouriteActors", (string)null);
                });

            modelBuilder.Entity("Ghost.Data.FavouriteVideo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VideoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VideoId");

                    b.ToTable("FavouriteVideos", (string)null);
                });

            modelBuilder.Entity("Ghost.Data.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Genres", (string)null);
                });

            modelBuilder.Entity("Ghost.Data.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Images", (string)null);
                });

            modelBuilder.Entity("Ghost.Data.Library", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Libraries", (string)null);
                });

            modelBuilder.Entity("Ghost.Data.LibraryPath", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("LibraryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("LibraryId");

                    b.ToTable("LibraryPaths", (string)null);
                });

            modelBuilder.Entity("Ghost.Data.Progress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Timestamp")
                        .HasColumnType("REAL");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VideoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VideoId");

                    b.ToTable("Progress", (string)null);
                });

            modelBuilder.Entity("Ghost.Data.RelatedVideo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("RelatedToId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VideoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RelatedToId");

                    b.HasIndex("VideoId");

                    b.ToTable("RelatedVideos", (string)null);
                });

            modelBuilder.Entity("Ghost.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Ghost.Data.Video", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Height")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastMetadataUpdate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastNfoScan")
                        .HasColumnType("TEXT");

                    b.Property<int>("LibraryPathId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Runtime")
                        .HasColumnType("REAL");

                    b.Property<long>("Size")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Width")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LibraryPathId");

                    b.ToTable("Videos", (string)null);
                });

            modelBuilder.Entity("Ghost.Data.VideoActor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ActorId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VideoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ActorId");

                    b.HasIndex("VideoId");

                    b.ToTable("VideoActors", (string)null);
                });

            modelBuilder.Entity("Ghost.Data.VideoGenre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GenreId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VideoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GenreId");

                    b.HasIndex("VideoId");

                    b.ToTable("VideoGenres", (string)null);
                });

            modelBuilder.Entity("Ghost.Data.VideoImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ImageId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("VideoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.HasIndex("VideoId");

                    b.ToTable("VideoImages", (string)null);
                });

            modelBuilder.Entity("Ghost.Data.Chapter", b =>
                {
                    b.HasOne("Ghost.Data.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ghost.Data.Video", "Video")
                        .WithMany("Chapters")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Ghost.Data.FavouriteActor", b =>
                {
                    b.HasOne("Ghost.Data.Actor", "Actor")
                        .WithMany("FavouritedBy")
                        .HasForeignKey("ActorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ghost.Data.User", "User")
                        .WithMany("FavouriteActors")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Actor");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ghost.Data.FavouriteVideo", b =>
                {
                    b.HasOne("Ghost.Data.User", "User")
                        .WithMany("FavouriteVideos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ghost.Data.Video", "Video")
                        .WithMany("FavouritedBy")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Ghost.Data.LibraryPath", b =>
                {
                    b.HasOne("Ghost.Data.Library", null)
                        .WithMany("Paths")
                        .HasForeignKey("LibraryId");
                });

            modelBuilder.Entity("Ghost.Data.Progress", b =>
                {
                    b.HasOne("Ghost.Data.User", "User")
                        .WithMany("VideoProgress")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ghost.Data.Video", "Video")
                        .WithMany("WatchedBy")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Ghost.Data.RelatedVideo", b =>
                {
                    b.HasOne("Ghost.Data.Video", "RelatedTo")
                        .WithMany()
                        .HasForeignKey("RelatedToId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ghost.Data.Video", "Video")
                        .WithMany("RelatedVideos")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RelatedTo");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Ghost.Data.Video", b =>
                {
                    b.HasOne("Ghost.Data.LibraryPath", "LibraryPath")
                        .WithMany("Videos")
                        .HasForeignKey("LibraryPathId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LibraryPath");
                });

            modelBuilder.Entity("Ghost.Data.VideoActor", b =>
                {
                    b.HasOne("Ghost.Data.Actor", "Actor")
                        .WithMany("VideoActors")
                        .HasForeignKey("ActorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ghost.Data.Video", "Video")
                        .WithMany("VideoActors")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Actor");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Ghost.Data.VideoGenre", b =>
                {
                    b.HasOne("Ghost.Data.Genre", "Genre")
                        .WithMany("VideoGenres")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ghost.Data.Video", "Video")
                        .WithMany("VideoGenres")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Ghost.Data.VideoImage", b =>
                {
                    b.HasOne("Ghost.Data.Image", "Image")
                        .WithMany("VideoImages")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ghost.Data.Video", "Video")
                        .WithMany("VideoImages")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Ghost.Data.Actor", b =>
                {
                    b.Navigation("FavouritedBy");

                    b.Navigation("VideoActors");
                });

            modelBuilder.Entity("Ghost.Data.Genre", b =>
                {
                    b.Navigation("VideoGenres");
                });

            modelBuilder.Entity("Ghost.Data.Image", b =>
                {
                    b.Navigation("VideoImages");
                });

            modelBuilder.Entity("Ghost.Data.Library", b =>
                {
                    b.Navigation("Paths");
                });

            modelBuilder.Entity("Ghost.Data.LibraryPath", b =>
                {
                    b.Navigation("Videos");
                });

            modelBuilder.Entity("Ghost.Data.User", b =>
                {
                    b.Navigation("FavouriteActors");

                    b.Navigation("FavouriteVideos");

                    b.Navigation("VideoProgress");
                });

            modelBuilder.Entity("Ghost.Data.Video", b =>
                {
                    b.Navigation("Chapters");

                    b.Navigation("FavouritedBy");

                    b.Navigation("RelatedVideos");

                    b.Navigation("VideoActors");

                    b.Navigation("VideoGenres");

                    b.Navigation("VideoImages");

                    b.Navigation("WatchedBy");
                });
#pragma warning restore 612, 618
        }
    }
}
