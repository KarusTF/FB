using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FizzBuzz.DTOs
{
    public class FizzBuzzRuleDTO
    {
        public int Id { get; set; }  // Include the Id property

        [Required(ErrorMessage = "Game name is required.")]
        public string GameName { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        public string Author { get; set; }

        [MinLength(1, ErrorMessage = "At least one divisor-word pair is required.")]
        public List<DivisorWordPairDTO> DivisorWordPairs { get; set; }

        public FizzBuzzRuleDTO()
        {
            DivisorWordPairs = new List<DivisorWordPairDTO>();
        }
    }
}