import { vi, describe, it, expect, beforeEach, afterAll } from 'vitest';

describe('getNextRandomNumber', () => {
    const mockSetCurrentNumber = vi.fn();
    const mockSetGeneratedNumbers = vi.fn();
    const mockSetFetchingNumber = vi.fn();
    let mockGeneratedNumbers = new Set<number>();
    let alertMessages: string[] = [];

    const mockAlert = (message: string) => {
        alertMessages.push(message);
    };
    const mockConsoleError = vi.spyOn(console, 'error').mockImplementation(() => {});

    const originalMathRandom = Math.random;

    beforeEach(() => {
        vi.clearAllMocks();
        alertMessages = [];
        mockGeneratedNumbers = new Set();
        mockSetGeneratedNumbers.mockImplementation((updater) => {
            mockGeneratedNumbers = updater(mockGeneratedNumbers);
        });
    });

    afterAll(() => {
        Math.random = originalMathRandom;
    });

    const callGetNextRandomNumber = async (gameId: string) => {
        mockSetFetchingNumber(true);
        try {
            let uniqueNumberFound = false;
            let attempts = 0;
            const maxAttempts = 10;

            while (!uniqueNumberFound && attempts < maxAttempts) {
                const newNumber = Math.floor(Math.random() * 100) + 1;

                if (!mockGeneratedNumbers.has(newNumber)) {
                    mockSetCurrentNumber(newNumber);
                    mockSetGeneratedNumbers((prev) => new Set(prev).add(newNumber));
                    uniqueNumberFound = true;
                } else {
                    attempts++;
                }
            }

            if (!uniqueNumberFound) {
                mockAlert('Failed to generate a unique number. Please try again.');
            }
        } catch (error) {
            console.error('Error fetching next random number:', error);
            mockAlert('Failed to fetch the next number. Please try again.');
        } finally {
            mockSetFetchingNumber(false);
        }
    };

    it('generate a unique number when none exist', async () => {
        //return 0.5 (50 + 1 = 51)
        Math.random = vi.fn(() => 0.5);

        await callGetNextRandomNumber('game-123');

        expect(mockSetCurrentNumber).toHaveBeenCalledWith(51);
        expect(mockGeneratedNumbers.has(51)).toBe(true);
        expect(mockSetFetchingNumber).toHaveBeenCalledWith(true);
        expect(mockSetFetchingNumber).toHaveBeenCalledWith(false);
        expect(alertMessages).toEqual([]);
    });

    it('find unique number after some attempts', async () => {
        mockGeneratedNumbers = new Set([51, 52, 53]);
        //0.5 (51 - existing), 0.6 (61 - new)
        Math.random = vi.fn()
            .mockReturnValueOnce(0.5)
            .mockReturnValueOnce(0.6);

        await callGetNextRandomNumber('game-123');

        expect(mockSetCurrentNumber).toHaveBeenCalledWith(61);
        expect(mockGeneratedNumbers.has(61)).toBe(true);
    });

    it('should handle failure to find unique number', async () => {
        mockGeneratedNumbers = new Set(Array.from({length: 100}, (_, i) => i + 1));
        Math.random = vi.fn(() => 0.5);

        await callGetNextRandomNumber('game-123');

        expect(alertMessages).toEqual(['Failed to generate a unique number. Please try again.']);
        expect(mockSetFetchingNumber).toHaveBeenCalledWith(false);
    });

    it('should handle errors', async () => {
        Math.random = vi.fn(() => { throw new Error('Random error') });

        await callGetNextRandomNumber('game-123');

        expect(mockConsoleError).toHaveBeenCalled();
        expect(alertMessages).toEqual(['Failed to fetch the next number. Please try again.']);
        expect(mockSetFetchingNumber).toHaveBeenCalledWith(false);
    });
});