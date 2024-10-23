﻿// <auto-generated />
using System;
using BitRent.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BitRent.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BitRent.Models.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AppUser", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("AppUser");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("BitRent.Models.BitTransaction", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Amount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("OwnerId");

                    b.ToTable("BitTansaction", (string)null);
                });

            modelBuilder.Entity("BitRent.Models.Property", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CustomerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<string>("OwnerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Property", (string)null);
                });

            modelBuilder.Entity("BitRent.Models.Customer", b =>
                {
                    b.HasBaseType("BitRent.Models.AppUser");

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Customer");
                });

            modelBuilder.Entity("BitRent.Models.Owner", b =>
                {
                    b.HasBaseType("BitRent.Models.AppUser");

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("AppUser", t =>
                        {
                            t.Property("AccountNumber")
                                .HasColumnName("Owner_AccountNumber");
                        });

                    b.HasDiscriminator().HasValue("Owner");
                });

            modelBuilder.Entity("BitRent.Models.BitTransaction", b =>
                {
                    b.HasOne("BitRent.Models.Customer", "Customer")
                        .WithMany("BitTransaction")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BitRent.Models.Owner", "Owner")
                        .WithMany("Transactions")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("BitRent.Models.Property", b =>
                {
                    b.HasOne("BitRent.Models.Customer", "Customer")
                        .WithMany("Properties")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BitRent.Models.Owner", "Owner")
                        .WithMany("Properties")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Customer");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("BitRent.Models.Customer", b =>
                {
                    b.Navigation("BitTransaction");

                    b.Navigation("Properties");
                });

            modelBuilder.Entity("BitRent.Models.Owner", b =>
                {
                    b.Navigation("Properties");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
