// services/api.ts
import axios from 'axios';
import { Game, AnswerSubmissionDTO, AnswerResultDTO } from '../Models/game';

const API_BASE_URL = import.meta.env.REACT_APP_API_BASE_URL || (window.location.protocol === 'https:' ? 'https://localhost:5001' : 'http://localhost:5000');

export const gameService = {
    // Create a new game
    createGame: async (requestPayload: { GameName: string; Author: string; DivisorWordPairs: { FizzBuzzRuleId:number,Divisor: number; Word: string }[] }): Promise<Game> => {
        try {
            const response = await axios.post(`${API_BASE_URL}/api/GameDefinition`, requestPayload);
            return response.data;
        } catch (error) {
            console.error('Error creating game:', error);
            throw error;
        }
    },

    // get game names
    getGameNames: async (): Promise<string[]> => {
        try {
            const response = await axios.get(`${API_BASE_URL}/api/GameDefinition/games`);
            return response.data;
        } catch (error) {
            console.error('Error fetching game names:', error);
            throw error;
        }
    },

    // get game rule
    getGameDefinition: async (gameName: string): Promise<Game> => {
        try {
            const response = await axios.get(`${API_BASE_URL}/api/GameDefinition/${gameName}`);
            return response.data;
        } catch (error) {
            console.error('Error fetching game definition:', error);
            throw error;
        }
    },

    // start game
    startGame: async (timeLimit: number): Promise<{ gameId: string }> => {
        try {
            const response = await axios.post(`${API_BASE_URL}/api/gameplay/start`, { timeLimit });
            return response.data;
        } catch (error) {
            console.error('Error starting game:', error);
            throw error;
        }
    },
    

    // submit answer
    submitAnswer: async (gameId: string, submission: AnswerSubmissionDTO): Promise<AnswerResultDTO> => {
        try {
            const response = await axios.post(`${API_BASE_URL}/api/gameplay/submit/${gameId}`, submission);
            return response.data;
        } catch (error) {
            console.error('Error submitting answer:', error);
            throw error;
        }
    },
};