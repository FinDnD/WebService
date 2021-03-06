﻿// <auto-generated />
using System;
using Espresso401_WebService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Espresso401_WebService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200906191836_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Espresso401_WebService.Models.DungeonMaster", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CampaignDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CampaignName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ExperienceLevel")
                        .HasColumnType("int");

                    b.Property<string>("PersonalBio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DungeonMasters");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CampaignDesc = "Deep in the dungeon's of Elderon, evil secrets stir. The world is once again thrust into peril, and our only hope is a small group of unlikely adventurers.",
                            CampaignName = "Campaign Sample",
                            ExperienceLevel = 2,
                            PersonalBio = "I'm just a test Dungeon Master, I don't actually exist :)",
                            UserId = "SeededDM"
                        });
                });

            modelBuilder.Entity("Espresso401_WebService.Models.Party", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DungeonMasterId")
                        .HasColumnType("int");

                    b.Property<bool>("Full")
                        .HasColumnType("bit");

                    b.Property<int>("MaxSize")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DungeonMasterId")
                        .IsUnique();

                    b.ToTable("Parties");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DungeonMasterId = 1,
                            Full = false,
                            MaxSize = 2147483647
                        });
                });

            modelBuilder.Entity("Espresso401_WebService.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CharacterName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Class")
                        .HasColumnType("int");

                    b.Property<int>("ExperienceLevel")
                        .HasColumnType("int");

                    b.Property<int>("GoodAlignment")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LawAlignment")
                        .HasColumnType("int");

                    b.Property<int?>("PartyId")
                        .HasColumnType("int");

                    b.Property<int>("Race")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PartyId");

                    b.ToTable("Players");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CharacterName = "Grontosh The Pummeler",
                            Class = 0,
                            ExperienceLevel = 0,
                            GoodAlignment = 50,
                            ImageUrl = "https://i.pinimg.com/236x/06/5d/fa/065dfa0df7eda641ab45bdeafc09dd22.jpg",
                            LawAlignment = 50,
                            PartyId = 1,
                            Race = 6,
                            UserId = "SeededPlayer"
                        });
                });

            modelBuilder.Entity("Espresso401_WebService.Models.PlayerInParty", b =>
                {
                    b.Property<int?>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int>("PartyId")
                        .HasColumnType("int");

                    b.HasKey("PlayerId", "PartyId");

                    b.HasIndex("PartyId");

                    b.ToTable("PlayerInParty");
                });

            modelBuilder.Entity("Espresso401_WebService.Models.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<bool>("DungeonMasterAccepted")
                        .HasColumnType("bit");

                    b.Property<int>("DungeonMasterId")
                        .HasColumnType("int");

                    b.Property<bool>("PlayerAccepted")
                        .HasColumnType("bit");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DungeonMasterId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("Espresso401_WebService.Models.Party", b =>
                {
                    b.HasOne("Espresso401_WebService.Models.DungeonMaster", "DungeonMaster")
                        .WithOne("Party")
                        .HasForeignKey("Espresso401_WebService.Models.Party", "DungeonMasterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Espresso401_WebService.Models.Player", b =>
                {
                    b.HasOne("Espresso401_WebService.Models.Party", "Party")
                        .WithMany()
                        .HasForeignKey("PartyId");
                });

            modelBuilder.Entity("Espresso401_WebService.Models.PlayerInParty", b =>
                {
                    b.HasOne("Espresso401_WebService.Models.Party", "Party")
                        .WithMany("PlayersInParty")
                        .HasForeignKey("PartyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Espresso401_WebService.Models.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Espresso401_WebService.Models.Request", b =>
                {
                    b.HasOne("Espresso401_WebService.Models.DungeonMaster", "DungeonMaster")
                        .WithMany("ActiveRequests")
                        .HasForeignKey("DungeonMasterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Espresso401_WebService.Models.Player", "Player")
                        .WithMany("ActiveRequests")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
