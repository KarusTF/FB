using System.ComponentModel.DataAnnotations;
namespace FizzBuzz.DTOs
{
    public class GameCreateRequestDTO
    {
        public GameCreateRequestDTO()
        {
               DivisorWordPairs = new List<DivisorWordPairDTO>();
        }

        [Required(ErrorMessage = "Game name is required.")]
        public string GameName { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        public string Author { get; set; }

        [MinLength(1, ErrorMessage = "At least one divisor-word pair is required.")]
        public List<DivisorWordPairDTO> DivisorWordPairs { get; set; }
        
        
        
    }
}