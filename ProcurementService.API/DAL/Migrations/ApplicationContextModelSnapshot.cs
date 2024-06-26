﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProcurementService.API.DAL.Core;

#nullable disable

namespace ProcurementService.API.DAL.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProcurementService.API.DAL.Schemes.Purchase.Files.ServerFile", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("create_at");

                    b.Property<int>("IdCreate")
                        .HasColumnType("int")
                        .HasColumnName("id_create");

                    b.Property<int>("IdUpdate")
                        .HasColumnType("int")
                        .HasColumnName("id_update");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("name");

                    b.Property<long>("Size")
                        .HasColumnType("bigint")
                        .HasColumnName("size");

                    b.Property<DateTime>("UpdateAt")
                        .ValueGeneratedOnUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("update_at");

                    b.HasKey("Id")
                        .HasName("FilesPrimaryKey");

                    b.ToTable("files", "purchase");
                });

            modelBuilder.Entity("ProcurementService.API.DAL.Schemes.Purchase.Filters.Filter", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int?>("CountCors")
                        .HasColumnType("int")
                        .HasColumnName("count_cors");

                    b.Property<int?>("Diagonal")
                        .HasColumnType("int")
                        .HasColumnName("diagonal");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("manufacturer");

                    b.Property<int?>("RAM")
                        .HasColumnType("int")
                        .HasColumnName("ram");

                    b.Property<int?>("SizeDisk")
                        .HasColumnType("int")
                        .HasColumnName("size_disk");

                    b.Property<string>("TypeDisk")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("type_disk");

                    b.Property<int?>("VRAM")
                        .HasColumnType("int")
                        .HasColumnName("vram");

                    b.HasKey("Id")
                        .HasName("FiltersPrimaryKey");

                    b.ToTable("filters", "purchase");
                });

            modelBuilder.Entity("ProcurementService.API.DAL.Schemes.Purchase.Products.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("create_at");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("name");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("price");

                    b.Property<int>("Type")
                        .HasColumnType("int")
                        .HasColumnName("type");

                    b.Property<DateTime>("UpdateAt")
                        .ValueGeneratedOnUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("update_at");

                    b.HasKey("Id")
                        .HasName("ProductsPrimaryKey");

                    b.ToTable("products", "purchase");
                });

            modelBuilder.Entity("ProcurementService.API.DAL.Schemes.Purchase.Requests.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("create_at");

                    b.Property<int>("IdConfirmed")
                        .HasColumnType("int")
                        .HasColumnName("id_confirmed");

                    b.Property<int>("IdCreate")
                        .HasColumnType("int")
                        .HasColumnName("id_create");

                    b.Property<int>("IdUpdate")
                        .HasColumnType("int")
                        .HasColumnName("id_update");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("bit")
                        .HasColumnName("is_confirmed");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("name");

                    b.Property<int>("SummaryMain")
                        .HasColumnType("int")
                        .HasColumnName("summary_main");

                    b.Property<int>("SummarySub")
                        .HasColumnType("int")
                        .HasColumnName("summary_sub");

                    b.Property<DateTime>("UpdateAt")
                        .ValueGeneratedOnUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("update_at");

                    b.HasKey("Id")
                        .HasName("RequestsPrimaryKey");

                    b.ToTable("requests", "purchase");
                });

            modelBuilder.Entity("ProcurementService.API.DAL.Schemes.Purchase.RequestsProducts.RequestProduct", b =>
                {
                    b.Property<int>("RequestId")
                        .HasColumnType("int")
                        .HasColumnName("request_id");

                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("product_id");

                    b.Property<int>("Count")
                        .HasColumnType("int")
                        .HasColumnName("count");

                    b.HasKey("RequestId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable(" requests_products", "purchase");
                });

            modelBuilder.Entity("ProcurementService.API.DAL.Schemes.Security.Roles.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("RolesPrimaryKey");

                    b.ToTable("roles", "security");
                });

            modelBuilder.Entity("ProcurementService.API.DAL.Schemes.Security.Users.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("email");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("hash");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("login");

                    b.Property<DateTime?>("UpdatedAt")
                        .IsRequired()
                        .ValueGeneratedOnUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("UsersPrimaryKey");

                    b.ToTable("users", "security");
                });

            modelBuilder.Entity("ProcurementService.API.DAL.Schemes.Security.UsersRoles.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.Property<int>("RoleId")
                        .HasColumnType("int")
                        .HasColumnName("role_id");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("users_roles", "security");
                });

            modelBuilder.Entity("ProcurementService.API.DAL.Schemes.Purchase.Files.ServerFile", b =>
                {
                    b.HasOne("ProcurementService.API.DAL.Schemes.Purchase.Requests.Request", "Request")
                        .WithOne("File")
                        .HasForeignKey("ProcurementService.API.DAL.Schemes.Purchase.Files.ServerFile", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Request");
                });

            modelBuilder.Entity("ProcurementService.API.DAL.Schemes.Purchase.Filters.Filter", b =>
                {
                    b.HasOne("ProcurementService.API.DAL.Schemes.Purchase.Products.Product", "Product")
                        .WithOne("Filter")
                        .HasForeignKey("ProcurementService.API.DAL.Schemes.Purchase.Filters.Filter", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ProcurementService.API.DAL.Schemes.Purchase.RequestsProducts.RequestProduct", b =>
                {
                    b.HasOne("ProcurementService.API.DAL.Schemes.Purchase.Products.Product", "Product")
                        .WithMany("RequestsProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProcurementService.API.DAL.Schemes.Purchase.Requests.Request", "Request")
                        .WithMany("RequestsProducts")
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Request");
                });

            modelBuilder.Entity("ProcurementService.API.DAL.Schemes.Security.UsersRoles.UserRole", b =>
                {
                    b.HasOne("ProcurementService.API.DAL.Schemes.Security.Roles.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProcurementService.API.DAL.Schemes.Security.Users.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProcurementService.API.DAL.Schemes.Purchase.Products.Product", b =>
                {
                    b.Navigation("Filter");

                    b.Navigation("RequestsProducts");
                });

            modelBuilder.Entity("ProcurementService.API.DAL.Schemes.Purchase.Requests.Request", b =>
                {
                    b.Navigation("File");

                    b.Navigation("RequestsProducts");
                });

            modelBuilder.Entity("ProcurementService.API.DAL.Schemes.Security.Roles.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("ProcurementService.API.DAL.Schemes.Security.Users.User", b =>
                {
                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
