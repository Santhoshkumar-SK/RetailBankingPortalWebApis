﻿// <auto-generated />
using AccountManagementApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AccountManagementApi.Migrations
{
    [DbContext(typeof(AccountManagementContext))]
    [Migration("20210609110500_AddedAccountTable")]
    partial class AddedAccountTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AccountManagementApi.Models.Account", b =>
                {
                    b.Property<int>("AccountID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("AccountBalance")
                        .HasColumnType("float");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.HasKey("AccountID");

                    b.ToTable("Account");
                });
#pragma warning restore 612, 618
        }
    }
}
