using FizzBuzz.Models;
using Microsoft.EntityFrameworkCore;

namespace FizzBuzz.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<FizzBuzzRule> FizzBuzzRules { get; set; }
        public DbSet<DivisorWordPair> DivisorWordPairs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=FB_database;Username=admin;Password=admin");
            }
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the relationship between FizzBuzzRule and DivisorWordPair
            modelBuilder.Entity<FizzBuzzRule>()
                .HasMany(rule => rule.DivisorWordPairs)
                .WithOne(pair => pair.FizzBuzzRule)
                .HasForeignKey(pair => pair.FizzBuzzRuleId);

            // Seeding 3 FizzBuzzRule entities
            modelBuilder.Entity<FizzBuzzRule>().HasData(
                new FizzBuzzRule
                {
                    Id = 1,
                    GameName = "Game1",
                    Author = "Admin"
                },
                new FizzBuzzRule
                {
                    Id = 2,
                    GameName = "Game2",
                    Author = "Admin1"
                },
                new FizzBuzzRule
                {
                    Id = 3,
                    GameName = "Game3",
                    Author = "Admin2"
                }
            );

            // Seeding DivisorWordPair entities with foreign key (FizzBuzzRuleId)
            modelBuilder.Entity<DivisorWordPair>().HasData(
                new DivisorWordPair { Id = 1, Divisor = 3, Word = "Fizz", FizzBuzzRuleId = 1 },
                new DivisorWordPair { Id = 2, Divisor = 5, Word = "Buzz", FizzBuzzRuleId = 1 },
                new DivisorWordPair { Id = 3, Divisor = 3, Word = "Three", FizzBuzzRuleId = 2 },
                new DivisorWordPair { Id = 4, Divisor = 7, Word = "Seven", FizzBuzzRuleId = 2 },
                new DivisorWordPair { Id = 5, Divisor = 4, Word = "FF", FizzBuzzRuleId = 3 },
                new DivisorWordPair { Id = 6, Divisor = 6, Word = "SS", FizzBuzzRuleId = 3 }
            );
        }
    }
}
