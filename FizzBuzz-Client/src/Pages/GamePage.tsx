// pages/GamePage.tsx
import * as React from 'react';
import { useEffect, useState, useRef } from 'react';
import { useParams } from 'react-router-dom';
import { gameService } from '../services/api';
import { Game, AnswerSubmissionDTO, AnswerResultDTO } from '../Models/game';

const GamePage: React.FC = () => {
    const { gameId } = useParams<{ gameId: string }>();
    const [gameData, setGameData] = useState<Game | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [gameActive, setGameActive] = useState<boolean>(false);
    const [timeRemaining, setTimeRemaining] = useState<number>(60);
    const [inputTime, setInputTime] = useState<number>(60);
    const [correctAnswers, setCorrectAnswers] = useState<number>(0);
    const [incorrectAnswers, setIncorrectAnswers] = useState<number>(0);
    const [currentNumber, setCurrentNumber] = useState<number | null>(null);
    const [generatedNumbers, setGeneratedNumbers] = useState<Set<number>>(new Set());
    const [fetchingNumber, setFetchingNumber] = useState<boolean>(false); // Loading state for fetching next number

    const answerInputRef = useRef<HTMLInputElement>(null);

    // get game data
    useEffect(() => {
        if (gameId) {
            gameService.getGameDefinition(gameId)
                .then((data) => {
                    setGameData(data);
                    setLoading(false);
                })
                .catch((error) => {
                    console.error('Error fetching game data:', error);
                    alert('Cannot fetch game data. Please try again.');
                    setLoading(false);
                });
        }
    }, [gameId]);

    useEffect(() => {
        if (gameActive && timeRemaining > 0) {
            const timer = setInterval(() => {
                setTimeRemaining((prevTime) => prevTime - 1);
            }, 1000);
            return () => clearInterval(timer);
        } else if (timeRemaining === 0 && gameActive) { 
            setGameActive(false);
            alert('Time is up.');
        }
    }, [gameActive, timeRemaining]);

    // Start the game
    const handleStartGame = async () => {
        if (inputTime <= 0) {
            alert('Please enter a valid time.');
            return;
        }

        try {
            const response = await gameService.startGame(inputTime);
            setGameActive(true);
            setCorrectAnswers(0);
            setIncorrectAnswers(0);
            setTimeRemaining(inputTime);
            setGeneratedNumbers(new Set()); // Reset generated numbers when starting a new game
            await getNextRandomNumber(response.gameId); // Fetch the first random number
        } catch (error) {
            console.error('Error starting game:', error);
            alert('Cannot start the game. Please try again.');
        }
    };

    //Get random number
    const getNextRandomNumber = async (gameId: string) => {
        setFetchingNumber(true); 
        try {
            let uniqueNumberFound = false;
            let attempts = 0;
            const maxAttempts = 10;

            while (!uniqueNumberFound && attempts < maxAttempts) {
                const newNumber = Math.floor(Math.random() * 100) + 1;
                

                if (!generatedNumbers.has(newNumber)) {
                    setCurrentNumber(newNumber);
                    setGeneratedNumbers((prev) => new Set(prev).add(newNumber)); 
                    uniqueNumberFound = true;
                } else {
                    attempts++;
                }
            }

            if (!uniqueNumberFound) {
                alert('Failed to generate a unique number. Please try again.');
            }
        } catch (error) {
            console.error('Error fetching next random number:', error);
            alert('Failed to fetch the next number. Please try again.');
        } finally {
            setFetchingNumber(false); 
        }
    };

    // Submit the answer
    const handleSubmitAnswer = async (answer: string) => {
        if (currentNumber !== null && gameId) {
            if (!answer.trim()) {
                alert('Please enter an answer.');
                return;
            }

            const submission: AnswerSubmissionDTO = {
                number: currentNumber,
                answer,
            };

            try {
                const result: AnswerResultDTO = await gameService.submitAnswer(gameId, submission);
                if (result.isCorrect) {
                    setCorrectAnswers((prev) => prev + 1);
                } else {
                    setIncorrectAnswers((prev) => prev + 1);
                }
                await getNextRandomNumber(gameId);
                if (answerInputRef.current) {
                    answerInputRef.current.value = '';
                }
            } catch (error) {
                console.error('Error submitting answer:', error);
                alert('Failed to submit your answer. Please try again.');
            }
        }
    };

    
    const handleTimeInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = parseInt(e.target.value, 10);
        if (!isNaN(value)) {
            setInputTime(value);
        }
    };

    if (loading) {
        return <div>Loading game data...</div>;
    }

    if (!gameData) {
        return <div>Game not found.</div>;
    }

    return (
        <div>
            <h1>{gameData.gameName} - Let's start</h1>
            <p>Author: {gameData.author}</p>

            <h3>Divisor and Word Pairs:</h3>
            <ul>
                {gameData.divisorWordPairs.map((pair) => (
                    <li>
                        <strong>Divisor:</strong> {pair.divisor} - <strong>Word:</strong> {pair.word}
                    </li>
                ))}
            </ul>

            {!gameActive && (
                <div>
                    <label>
                        Set Time (seconds):
                        <input
                            type="number"
                            value={inputTime}
                            onChange={handleTimeInputChange}
                            min="1"
                        />
                    </label>
                    <button style={{ marginLeft: '10px' }} onClick={handleStartGame}>Start Game</button>
                </div>
            )}

            {gameActive && (
                <div>
                    <p>Time Remaining: {timeRemaining}s</p>
                    <p>Current Number: {currentNumber}</p>
                    <input
                        ref={answerInputRef}
                        type="text"
                        placeholder="Enter your answer"
                        onKeyDown={(e) => {
                            if (e.key === 'Enter') {
                                
                                (async () => {
                                    await handleSubmitAnswer((e.target as HTMLInputElement).value);
                                })();
                            }
                        }}
                        disabled={fetchingNumber} 
                    />
                    {fetchingNumber && <p>Loading next number...</p>}
                    <div>
                        <p>Incorrect answers: {incorrectAnswers}</p>
                        <p>Correct answers: {correctAnswers}</p>
                    </div>
                </div>
            )}
            <div>
                <p>Total score: {correctAnswers}</p>
            </div>
        </div>
    );
};

export default GamePage;