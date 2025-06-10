using FizzBuzz.DTOs;
using FizzBuzz.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FizzBuzz.Services
{
    public interface IGameDefinitionService
    {
        Task<FizzBuzzRule> CreateGameAsync(GameCreateRequestDTO request);
        
        Task<FizzBuzzRuleDTO> GetGameDefinitionByNameAsync(string gameName);
        
        Task<List<string>> GetGameNamesAsync();
        Task<int> GetLastGameIdAsync();
    }
}