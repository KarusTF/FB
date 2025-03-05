namespace FizzBuzz.Models
{
    public class FizzBuzzRule
    {
        public int Id { get; set; }  // Primary key
        // List to hold multiple divisor-word pairs
        public List<DivisorWordPair> DivisorWordPairs { get; set; }

        // Constructor to initialize the DivisorWordPairs list
        public FizzBuzzRule()
        {
            DivisorWordPairs = new List<DivisorWordPair>();
            TimeLimit = 60;
        }
        public string GameName { get; set; }  // Name of the game
        public string Author { get; set; } // Author
        public int TimeLimit { get; set; }
    }
    public class DivisorWordPair
    {
        public int Id { get; set; }  // Primary Key for DivisorWordPair
        public int Divisor { get; set; }  // Divisor
        public string Word { get; set; }  // Word to replace the divisor with
    
        // Foreign Key reference to FizzBuzzRule
        public int FizzBuzzRuleId { get; set; }
        public FizzBuzzRule FizzBuzzRule { get; set; }  // Navigation property
    }
}