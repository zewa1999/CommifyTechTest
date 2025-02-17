﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Tax.Calculator.Commify.Data;

#nullable disable

namespace Tax.Calculator.Commify.Migrations
{
    [DbContext(typeof(TaxCalculatorDbContext))]
    partial class TaxCalculatorDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Tax.Calculator.Commify.Data.Entities.TaxBand", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BandName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<DateTime>("LastUpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<int>("LowerSalaryRange")
                        .HasColumnType("integer");

                    b.Property<int>("TaxRate")
                        .HasColumnType("integer");

                    b.Property<int?>("UpperSalaryRange")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("TaxBands");

                    b.HasData(
                        new
                        {
                            Id = new Guid("1102b5a2-4e98-4ae8-b627-34cae4ab4fd9"),
                            BandName = "Tax Band A",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastUpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LowerSalaryRange = 0,
                            TaxRate = 0,
                            UpperSalaryRange = 5000
                        },
                        new
                        {
                            Id = new Guid("3a734862-f345-4197-9f0f-4bb6f2bf3f39"),
                            BandName = "Tax Band B",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastUpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LowerSalaryRange = 5000,
                            TaxRate = 20,
                            UpperSalaryRange = 20000
                        },
                        new
                        {
                            Id = new Guid("092cbf43-9e33-4730-b5bd-c987e1bd8df1"),
                            BandName = "Tax Band C",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastUpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LowerSalaryRange = 20000,
                            TaxRate = 40
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
