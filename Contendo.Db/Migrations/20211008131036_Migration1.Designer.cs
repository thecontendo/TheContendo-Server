﻿// <auto-generated />
using System;
using Contendo.Db.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Contendo.Db.Migrations
{
    [DbContext(typeof(CDbContext))]
    [Migration("20211008131036_Migration1")]
    partial class Migration1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Contendo.Models.Challenges.Challenge", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("ChallengeStatus")
                        .HasColumnType("integer");

                    b.Property<Guid>("ChallengerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime>("Duration")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("ParticipantId")
                        .HasColumnType("uuid");

                    b.Property<int>("Points")
                        .HasColumnType("integer");

                    b.Property<Guid>("ShotId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ChallengerId");

                    b.HasIndex("ParticipantId");

                    b.HasIndex("ShotId");

                    b.ToTable("Challenges");
                });

            modelBuilder.Entity("Contendo.Models.Identity.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ValidFrom")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("ValidTo")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("Username", "Email")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("3b92e3ec-fb30-492c-a75f-e266f4b372ff"),
                            Email = "abhinav10p@gmail.com",
                            FirstName = "Super",
                            LastName = "Admin",
                            Status = 0,
                            Username = "SuperAdmin",
                            ValidFrom = new DateTime(2020, 12, 31, 23, 0, 0, 0, DateTimeKind.Utc),
                            ValidTo = new DateTime(9999, 12, 31, 22, 59, 59, 999, DateTimeKind.Utc).AddTicks(9999)
                        });
                });

            modelBuilder.Entity("Contendo.Models.Identity.UserContact", b =>
                {
                    b.Property<Guid>("ContactId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("ContactId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserContacts");
                });

            modelBuilder.Entity("Contendo.Models.Shots.Shot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Icon")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Shots");

                    b.HasData(
                        new
                        {
                            Id = new Guid("0a4263c0-4f89-421c-871c-1a811092c316"),
                            Icon = "pushups.png",
                            Name = "Push Ups"
                        },
                        new
                        {
                            Id = new Guid("7141c807-233b-42de-8b18-878f4c5d6f91"),
                            Icon = "burpees.png",
                            Name = "Burpees"
                        },
                        new
                        {
                            Id = new Guid("d0562f7f-bf94-44a3-a3e0-d8d40d419880"),
                            Icon = "jumpingjacks.png",
                            Name = "Jumping Jacks"
                        },
                        new
                        {
                            Id = new Guid("02b8a53a-9b37-439a-88d1-d0363d621508"),
                            Icon = "classicplank.png",
                            Name = "Classical Plank"
                        },
                        new
                        {
                            Id = new Guid("a5bb13cb-adb2-4bb6-b490-77bee49182e4"),
                            Icon = "straighthandplank.png",
                            Name = "Straight Hand Plank"
                        },
                        new
                        {
                            Id = new Guid("27b4717e-bc68-484d-b98b-07387425604c"),
                            Icon = "sideplank.png",
                            Name = "Side Plank"
                        });
                });

            modelBuilder.Entity("Contendo.Models.Challenges.Challenge", b =>
                {
                    b.HasOne("Contendo.Models.Identity.User", "Challenger")
                        .WithMany()
                        .HasForeignKey("ChallengerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Contendo.Models.Identity.User", "Participant")
                        .WithMany()
                        .HasForeignKey("ParticipantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Contendo.Models.Shots.Shot", "Shot")
                        .WithMany()
                        .HasForeignKey("ShotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Challenger");

                    b.Navigation("Participant");

                    b.Navigation("Shot");
                });

            modelBuilder.Entity("Contendo.Models.Identity.UserContact", b =>
                {
                    b.HasOne("Contendo.Models.Identity.User", "Contact")
                        .WithMany("ContactUsers")
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Contendo.Models.Identity.User", "User")
                        .WithMany("UserContacts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contact");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Contendo.Models.Identity.User", b =>
                {
                    b.Navigation("ContactUsers");

                    b.Navigation("UserContacts");
                });
#pragma warning restore 612, 618
        }
    }
}
