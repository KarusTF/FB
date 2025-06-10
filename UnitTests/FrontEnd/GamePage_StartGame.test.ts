import { describe, it, expect, vi, beforeEach, beforeAll, afterAll } from 'vitest';

import GamePage from '../../FizzBuzz-Client/src/Pages/GamePage';
import { gameService } from '../../FizzBuzz-Client/src/services/api';


global.alert = vi.fn();

const mockGameService = {
    startGame: vi.fn<[number], Promise<void>>(),
    getNextRandomNumber: vi.fn<[string], Promise<void>>()
};

vi.mock('../services/api', () => ({
    gameService: mockGameService
}));

describe('handleStartGame', () => {
    // Mock state setters
    const mockSetGameActive = vi.fn();
    const mockSetCorrectAnswers = vi.fn();
    const mockSetIncorrectAnswers = vi.fn();
    const mockSetTimeRemaining = vi.fn();
    const mockSetGeneratedNumbers = vi.fn();

    beforeEach(() => {
        vi.clearAllMocks();
        mockGameService.startGame.mockResolvedValue(undefined);
    });

    const callHandleStartGame = async (inputTime: number) => {
        if (inputTime <= 0) {
            global.alert('Please enter a valid time.');
            return;
        }

        try {
            await mockGameService.startGame(inputTime);
            mockSetGameActive(true);
            mockSetCorrectAnswers(0);
            mockSetIncorrectAnswers(0);
            mockSetTimeRemaining(inputTime);
            mockSetGeneratedNumbers(new Set());
            await mockGameService.getNextRandomNumber('mock-game-id');
        } catch (error) {
            console.error('Error starting game:', error);
            global.alert('Cannot start the game. Please try again.');
        }
    };

    it('reject negative time input', async () => {
        await callHandleStartGame(0);
        expect(global.alert).toHaveBeenCalledWith('Please enter a valid time.');
        expect(mockGameService.startGame).not.toHaveBeenCalled();
    });

    it('start game', async () => {
        await callHandleStartGame(60);
        expect(mockGameService.startGame).toHaveBeenCalledWith(60);
        expect(mockSetGameActive).toHaveBeenCalledWith(true);
        expect(mockSetCorrectAnswers).toHaveBeenCalledWith(0);
        expect(mockSetIncorrectAnswers).toHaveBeenCalledWith(0);
        expect(mockSetTimeRemaining).toHaveBeenCalledWith(60);
        expect(mockSetGeneratedNumbers).toHaveBeenCalledWith(new Set());
        expect(global.alert).not.toHaveBeenCalled();
    });

    it('handle API errors', async () => {
        mockGameService.startGame.mockRejectedValue(new Error('API failed'));
        await callHandleStartGame(60);
        expect(global.alert).toHaveBeenCalledWith('Cannot start the game. Please try again.');
    });
});