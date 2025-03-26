// models/game.ts
export interface DivisorWordPair {
    fizzBuzzRuleId : number;
    divisor: number;
    word: string;
}

export interface Game {
    id: number;
    gameName: string;
    author: string;
    divisorWordPairs: DivisorWordPair[];
}

export interface AnswerSubmissionDTO {
    number: number;
    answer: string;
}

export interface AnswerResultDTO {
    isCorrect: boolean;
    correctAnswers: number;
    incorrectAnswers: number;
}