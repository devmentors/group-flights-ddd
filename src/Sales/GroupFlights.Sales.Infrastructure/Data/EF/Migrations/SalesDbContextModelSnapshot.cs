﻿// <auto-generated />
using System;
using GroupFlights.Sales.Infrastructure.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GroupFlights.Sales.Infrastructure.Data.EF.Migrations
{
    [DbContext(typeof(SalesDbContext))]
    partial class SalesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("sales")
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GroupFlights.Sales.Application.DeadlineRegistry.DeadlineRegistryEntry", b =>
                {
                    b.Property<Guid>("DeadlineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("SourceId")
                        .HasColumnType("uuid");

                    b.Property<string>("SourceType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("DeadlineId");

                    b.ToTable("DeadlineRegistryEntries", "sales");
                });

            modelBuilder.Entity("GroupFlights.Sales.Infrastructure.Data.Models.OfferDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Object")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdateDateUtc")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Offers", "sales");
                });

            modelBuilder.Entity("GroupFlights.Sales.Infrastructure.Data.Models.ReservationDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ContractId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Object")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdateDateUtc")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Reservations", "sales");
                });
#pragma warning restore 612, 618
        }
    }
}
