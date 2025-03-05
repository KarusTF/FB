using FizzBuzz.Data;
using FizzBuzz.Models;
using Microsoft.EntityFrameworkCore;

namespace FizzBuzz.Services
{
    public class FizzBuzzService
    {
        private readonly AppDbContext _context;

        public FizzBuzzService(AppDbContext context)
        {
            _context = context;
        }

        public string GetFizzBuzz(int number, string game = "Default")
        {
            var rules = _context.FizzBuzzRules
                .Where(r => r.GameName == game)
                .Include(r => r.DivisorWordPairs)
                .ToList();

            var result = rules
                .SelectMany(r => r.DivisorWordPairs) 
                .Where(dwp => number % dwp.Divisor == 0) 
                .Aggregate("", (current, pair) => current + pair.Word); 

            return string.IsNullOrEmpty(result) ? number.ToString() : result;
        }
    }
}