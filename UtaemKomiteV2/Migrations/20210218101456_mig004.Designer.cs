﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UtaemKomiteV2.Models;

namespace UtaemKomiteV2.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20210218101456_mig004")]
    partial class mig004
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("UtaemKomiteV2.Models.Dosya", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<double>("boyut")
                        .HasColumnType("float");

                    b.Property<string>("icon")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("isim")
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<string>("kulName")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<bool>("silindi")
                        .HasColumnType("bit");

                    b.Property<string>("sysname")
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<DateTime>("tarih")
                        .HasColumnType("datetime2");

                    b.Property<int>("turID")
                        .HasColumnType("int");

                    b.Property<string>("uzantı")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.HasKey("ID");

                    b.HasIndex("turID");

                    b.ToTable("Dosya");
                });

            modelBuilder.Entity("UtaemKomiteV2.Models.Kullar", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<bool>("admin")
                        .HasColumnType("bit");

                    b.Property<string>("adminCode")
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<string>("cerez")
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<int>("hatali")
                        .HasColumnType("int");

                    b.Property<bool>("hatirla")
                        .HasColumnType("bit");

                    b.Property<DateTime>("kilitliTarih")
                        .HasColumnType("datetime2");

                    b.Property<string>("kulname")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("kulpass")
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.HasKey("ID");

                    b.ToTable("Kullar");
                });

            modelBuilder.Entity("UtaemKomiteV2.Models.Tur", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("isim")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("ID");

                    b.ToTable("Tur");
                });

            modelBuilder.Entity("UtaemKomiteV2.Models.Dosya", b =>
                {
                    b.HasOne("UtaemKomiteV2.Models.Tur", "tur")
                        .WithMany("dosyalar")
                        .HasForeignKey("turID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("tur");
                });

            modelBuilder.Entity("UtaemKomiteV2.Models.Tur", b =>
                {
                    b.Navigation("dosyalar");
                });
#pragma warning restore 612, 618
        }
    }
}
