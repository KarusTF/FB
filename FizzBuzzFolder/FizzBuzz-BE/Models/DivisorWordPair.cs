public class FizzBuzzRule
{
    public int Id { get; set; }
    public string GameName { get; set; }
    public string Author { get; set; }
    
    public List<DivisorWordPair> DivisorWordPairs { get; set; }
}

public class DivisorWordPair
{
    public int Id { get; set; }  // Primary key for the pair
    public int FizzBuzzRuleId { get; set; }  // Foreign key
    public int Divisor { get; set; }
    public string Word { get; set; }

    public FizzBuzzRule FizzBuzzRule { get; set; }
}