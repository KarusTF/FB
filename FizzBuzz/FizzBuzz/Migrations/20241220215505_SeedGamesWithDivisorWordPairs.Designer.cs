﻿// <auto-generated />
using FizzBuzz.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FizzBuzz.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241220215505_SeedGamesWithDivisorWordPairs")]
    partial class SeedGamesWithDivisorWordPairs
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("DivisorWordPair", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Divisor")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FizzBuzzRuleId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Word")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FizzBuzzRuleId");

                    b.ToTable("DivisorWordPairs");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Divisor = 3,
                            FizzBuzzRuleId = 1,
                            Word = "Fizz"
                        },
                        new
                        {
                            Id = 2,
                            Divisor = 5,
                            FizzBuzzRuleId = 1,
                            Word = "Buzz"
                        },
                        new
                        {
                            Id = 3,
                            Divisor = 3,
                            FizzBuzzRuleId = 2,
                            Word = "Foo"
                        },
                        new
                        {
                            Id = 4,
                            Divisor = 7,
                            FizzBuzzRuleId = 2,
                            Word = "Bar"
                        },
                        new
                        {
                            Id = 5,
                            Divisor = 4,
                            FizzBuzzRuleId = 3,
                            Word = "Qux"
                        },
                        new
                        {
                            Id = 6,
                            Divisor = 6,
                            FizzBuzzRuleId = 3,
                            Word = "Quux"
                        });
                });

            modelBuilder.Entity("FizzBuzzRule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GameName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("FizzBuzzRules");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Author = "Admin",
                            GameName = "Game1"
                        },
                        new
                        {
                            Id = 2,
                            Author = "Admin1",
                            GameName = "Game2"
                        },
                        new
                        {
                            Id = 3,
                            Author = "Admin2",
                            GameName = "Game3"
                        });
                });

            modelBuilder.Entity("DivisorWordPair", b =>
                {
                    b.HasOne("FizzBuzzRule", "FizzBuzzRule")
                        .WithMany("DivisorWordPairs")
                        .HasForeignKey("FizzBuzzRuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FizzBuzzRule");
                });

            modelBuilder.Entity("FizzBuzzRule", b =>
                {
                    b.Navigation("DivisorWordPairs");
                });
#pragma warning restore 612, 618
        }
    }
}
