using System;
using System.Collections.Generic;

namespace FizzBuzz.DTOs
{
    public class GameSessionDTO
    {
        public string GameId { get; set; }
        public DateTime StartTime { get; set; }
        public int CorrectAnswers { get; set; }
        public int IncorrectAnswers { get; set; }
        public HashSet<int> UsedNumbers { get; set; }
    }
}