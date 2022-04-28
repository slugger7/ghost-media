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

            modelBuilder.Entity("Ghost.Data.Video", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("LibraryPathId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

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

            modelBuilder.Entity("Ghost.Data.LibraryPath", b =>
                {
                    b.HasOne("Ghost.Data.Library", null)
                        .WithMany("Paths")
                        .HasForeignKey("LibraryId");
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

            modelBuilder.Entity("Ghost.Data.Actor", b =>
                {
                    b.Navigation("VideoActors");
                });

            modelBuilder.Entity("Ghost.Data.Genre", b =>
                {
                    b.Navigation("VideoGenres");
                });

            modelBuilder.Entity("Ghost.Data.Library", b =>
                {
                    b.Navigation("Paths");
                });

            modelBuilder.Entity("Ghost.Data.LibraryPath", b =>
                {
                    b.Navigation("Videos");
                });

            modelBuilder.Entity("Ghost.Data.Video", b =>
                {
                    b.Navigation("VideoActors");

                    b.Navigation("VideoGenres");
                });
#pragma warning restore 612, 618
        }
    }
}