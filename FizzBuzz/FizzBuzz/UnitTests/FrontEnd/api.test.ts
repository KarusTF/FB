// services/api.test.ts
import { describe, it, expect, vi, beforeEach } from 'vitest';
import axios from 'axios';
import { gameService } from '../../fizzbuzz-frontend/src/services/api';
import { Game, DivisorWordPair, AnswerSubmissionDTO, AnswerResultDTO } from '../../fizzbuzz-frontend/src/Models/game';

vi.mock('axios');

describe('gameService', () => {
    beforeEach(() => {
        vi.clearAllMocks();
    });

    it('create a game', async () => {
        const mockGame = {
            GameName: 'Test Game',
            Author: 'Test Author',
            DivisorWordPairs: [{ FizzBuzzRuleId: 1, Divisor: 3, Word: 'Fizz' }, { FizzBuzzRuleId: 1, Divisor: 4, Word: 'no' }],
        };
        vi.mocked(axios.post).mockResolvedValue({ });
        const result = await gameService.createGame(mockGame);
        console.log(result);

        expect(result).toEqual(expect.objectContaining({
            gameName: 'Test Game',
            author: 'Test Author',
        }));
        
    });

    it('create a game test 2', async () => {
        const mockGame = {
            GameName: 'Test2',
            Author: 'Test2',
            DivisorWordPairs: [{ FizzBuzzRuleId: 1, Divisor: 3, Word: 'Fizz' }, { FizzBuzzRuleId: 1, Divisor: 4, Word: 'no' }],
        };
        vi.mocked(axios.post).mockResolvedValue({ });
        const result = await gameService.createGame(mockGame);
        console.log(result);

        expect(result).toEqual(expect.objectContaining({
            gameName: 'Test2',
            author: 'Test2',
        }));

    });
    

    it('return game names', async () => {
        const mockResponse: string[] = ['Game1', 'Game2'];
        vi.mocked(axios.get).mockResolvedValue({ data: mockResponse });

        const result = await gameService.getGameNames();

        expect(result).toEqual(expect.arrayContaining(mockResponse));
        
    });
    
    it('fetch game definition by name', async () => {
        
        vi.mocked(axios.get).mockResolvedValue({});

        const result = await gameService.getGameDefinition('Test Game');

        expect(result).toEqual(expect.objectContaining({
            gameName: 'Test Game',
            author: 'Test Author',
            divisorWordPairs: [{ fizzBuzzRuleId: 0, divisor: 3, word: 'Fizz' }, { fizzBuzzRuleId: 0, divisor: 4, word: 'no' }]
        }));
        
    });
    
    it('start a game', async () => {
        const mockResponse = { gameId: '1' };

        vi.mocked(axios.post).mockResolvedValue({});
        const result = await gameService.startGame(30);

        expect(result).not.toBeNull();
        
    });
    
    it('handle errors when creating a game', async () => {
        const mockGame = {
            GameName: 'Test Game',
            Author: '',
            DivisorWordPairs: [{ FizzBuzzRuleId: 1, Divisor: 3, Word: 'Fizz' }],
        };

        vi.mocked(axios.post).mockRejectedValue({
            response: {
                status: 500,
                data: { message: 'Internal Server Error' },
            },
            isAxiosError: true,
        });
        
    });
    
});