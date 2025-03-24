using System;
using System.Threading.Tasks;
using FizzBuzz.DTOs;

namespace FizzBuzz.Services
{
    public interface IGamePlayService
    {
        Task<GameSessionDTO> StartGameAsync(int timeLimit);
        Task<AnswerResultDTO> SubmitAnswerAsync(string gameId, int number, string answer);
    }
}