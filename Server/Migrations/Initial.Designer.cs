﻿// <auto-generated />
using System;
using Fracture.Server.Modules.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Fracture.Server.Migrations
{
    [DbContext(typeof(FractureDbContext))]
    [Migration("20240520174514_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("Fracture.Server.Modules.Items.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("CreatedById")
                        .HasColumnType("INTEGER");

                    b.Property<string>("History")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsEquipped")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Rarity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Fracture.Server.Modules.Items.Models.ItemStatistics", b =>
                {
                    b.Property<int>("ItemId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Armor")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Health")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Luck")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Power")
                        .HasColumnType("INTEGER");

                    b.HasKey("ItemId");

                    b.ToTable("ItemStatistics");
                });

            modelBuilder.Entity("Fracture.Server.Modules.Users.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Fracture.Server.Modules.Items.Models.Item", b =>
                {
                    b.HasOne("Fracture.Server.Modules.Users.User", "CreatedBy")
                        .WithMany("Items")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("Fracture.Server.Modules.Items.Models.ItemStatistics", b =>
                {
                    b.HasOne("Fracture.Server.Modules.Items.Models.Item", "Item")
                        .WithOne("Statistics")
                        .HasForeignKey("Fracture.Server.Modules.Items.Models.ItemStatistics", "ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Fracture.Server.Modules.Items.Models.Item", b =>
                {
                    b.Navigation("Statistics")
                        .IsRequired();
                });

            modelBuilder.Entity("Fracture.Server.Modules.Users.User", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
