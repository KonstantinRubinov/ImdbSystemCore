﻿// <auto-generated />
using System;
using ImdbServerCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ImdbServerCore.Migrations
{
    [DbContext(typeof(ImdbFavoritesEntities))]
    partial class ImdbFavoritesEntitiesModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ImdbServerCore.MOVIEEXTEND", b =>
                {
                    b.Property<string>("movieImdbID");

                    b.Property<float>("movieImdbRating");

                    b.Property<string>("moviePlot");

                    b.Property<string>("movieRated");

                    b.Property<bool>("movieSeen");

                    b.Property<string>("movieUrl");

                    b.Property<string>("userID");

                    b.HasKey("movieImdbID");

                    b.HasIndex("userID");

                    b.ToTable("MOVIEEXTENDS");
                });

            modelBuilder.Entity("ImdbServerCore.MOVy", b =>
                {
                    b.Property<string>("movieImdbID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("moviePoster");

                    b.Property<string>("movieTitle");

                    b.Property<int>("movieYear");

                    b.Property<string>("userID");

                    b.HasKey("movieImdbID");

                    b.HasIndex("userID");

                    b.ToTable("MOVIES");
                });

            modelBuilder.Entity("ImdbServerCore.USER", b =>
                {
                    b.Property<string>("userID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("userBirthDate");

                    b.Property<string>("userEmail")
                        .IsRequired();

                    b.Property<string>("userFirstName")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.Property<string>("userGender")
                        .IsRequired();

                    b.Property<string>("userImdbPass");

                    b.Property<string>("userLastName")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.Property<string>("userNickName")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.Property<string>("userPassword")
                        .IsRequired();

                    b.Property<string>("userPicture");

                    b.HasKey("userID");

                    b.ToTable("USERS");
                });

            modelBuilder.Entity("ImdbServerCore.MOVIEEXTEND", b =>
                {
                    b.HasOne("ImdbServerCore.MOVy", "MOVy")
                        .WithMany("MOVIEEXTENDS")
                        .HasForeignKey("movieImdbID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImdbServerCore.USER", "USER")
                        .WithMany("MOVIEEXTENDS")
                        .HasForeignKey("userID");
                });

            modelBuilder.Entity("ImdbServerCore.MOVy", b =>
                {
                    b.HasOne("ImdbServerCore.USER", "USER")
                        .WithMany("MOVIES")
                        .HasForeignKey("userID");
                });
#pragma warning restore 612, 618
        }
    }
}
