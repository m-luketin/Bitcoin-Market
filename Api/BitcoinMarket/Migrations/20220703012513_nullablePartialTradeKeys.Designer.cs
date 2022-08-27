﻿// <auto-generated />
using System;
using BitcoinMarket.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BitcoinMarket.Migrations
{
    [DbContext(typeof(BitcoinMarketDbContext))]
    [Migration("20220703012513_nullablePartialOrderKeys")]
    partial class nullablePartialOrderKeys
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.15")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BitcoinMarket.Data.Models.PartialOrder", b =>
                {
                    b.Property<int>("PartialOrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BuyOrderId")
                        .HasColumnType("int");

                    b.Property<int?>("SellOrderId")
                        .HasColumnType("int");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(12,4)");

                    b.HasKey("PartialOrderId");

                    b.HasIndex("BuyOrderId");

                    b.HasIndex("SellOrderId");

                    b.ToTable("PartialOrders");
                });

            modelBuilder.Entity("BitcoinMarket.Data.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("FilledValue")
                        .HasColumnType("decimal(12,2)");

                    b.Property<bool>("IsBuy")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("TransactionFinished")
                        .HasColumnType("datetime2");

                    b.Property<int>("TransactionOwnerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TransactionStarted")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<decimal>("ValueInBtc")
                        .HasPrecision(20, 10)
                        .HasColumnType("decimal(20,10)");

                    b.Property<decimal>("ValueInUsd")
                        .HasColumnType("decimal(12,2)");

                    b.HasKey("Id");

                    b.HasIndex("TransactionOwnerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("BitcoinMarket.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("BtcBalance")
                        .HasPrecision(20, 10)
                        .HasColumnType("decimal(20,10)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("UsdBalance")
                        .HasColumnType("decimal(12,2)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BitcoinMarket.Data.Models.PartialOrder", b =>
                {
                    b.HasOne("BitcoinMarket.Data.Order", "BuyOrder")
                        .WithMany("PartialBuyOrders")
                        .HasForeignKey("BuyOrderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BitcoinMarket.Data.Order", "SellOrder")
                        .WithMany("PartialSellOrders")
                        .HasForeignKey("SellOrderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("BuyOrder");

                    b.Navigation("SellOrder");
                });

            modelBuilder.Entity("BitcoinMarket.Data.Order", b =>
                {
                    b.HasOne("BitcoinMarket.Data.User", "TransactionOwner")
                        .WithMany("Orders")
                        .HasForeignKey("TransactionOwnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("TransactionOwner");
                });

            modelBuilder.Entity("BitcoinMarket.Data.Order", b =>
                {
                    b.Navigation("PartialBuyOrders");

                    b.Navigation("PartialSellOrders");
                });

            modelBuilder.Entity("BitcoinMarket.Data.User", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
