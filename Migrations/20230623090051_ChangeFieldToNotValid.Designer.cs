﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using StarkovTestApp.DbLayer;

#nullable disable

namespace StarkovTestApp.Migrations
{
    [DbContext(typeof(DbLayerContext))]
    [Migration("20230623090051_ChangeFieldToNotValid")]
    partial class ChangeFieldToNotValid
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-preview.5.23280.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("StarkovTestApp.Models.Department", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<int>("ManagerID")
                        .HasColumnType("integer");

                    b.Property<string>("ManagerName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("NotValid")
                        .HasColumnType("boolean");

                    b.Property<int>("ParentID")
                        .HasColumnType("integer");

                    b.Property<string>("ParentName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("StarkovTestApp.Models.Employee", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<int>("DepartmentID")
                        .HasColumnType("integer");

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("JobDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("JobTitle")
                        .HasColumnType("integer");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("NotValid")
                        .HasColumnType("boolean");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("StarkovTestApp.Models.JobTitle", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("JobTitles");
                });
#pragma warning restore 612, 618
        }
    }
}
