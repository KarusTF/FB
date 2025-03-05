import React, { useEffect, useState, useRef } from 'react';
import axios from 'axios';
import { useParams } from 'react-router-dom';

const GamePage: React.FC = () => {
    const { gameId } = useParams<{ gameId: string }>();
    const [gameData, setGameData] = useState<any>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [gameActive, setGameActive] = useState<boolean>(false);
    const [timeRemaining, setTimeRemaining] = useState<number>(60);
    const [inputTime, setInputTime] = useState<number>(60);
    const [correctAnswers, setCorrectAnswers] = useState<number>(0);
    const [incorrectAnswers, setIncorrectAnswers] = useState<number>(0);
    const [currentNumber, setCurrentNumber] = useState<number | null>(null);

    const answerInputRef = useRef<HTMLInputElement>(null); // Ref for input field

    useEffect(() => {
        axios
            .get(`http://localhost:5154/api/fizzbuzzrules/${gameId}`)
            .then((response) => {
                setGameData(response.data);
                setLoading(false);
            })
            .catch((error) => {
                console.error('Error fetching game data:', error);
                setLoading(false);
            });
    }, [gameId]);

    useEffect(() => {
        if (gameActive && timeRemaining > 0) {
            const timer = setInterval(() => {
                setTimeRemaining((prevTime) => prevTime - 1);
            }, 1000);
            return () => clearInterval(timer);
        } else if (timeRemaining === 0) {
            setGameActive(false);
            alert('Time is up!');
        }
    }, [gameActive, timeRemaining]);

    const handleStartGame = () => {
        if (inputTime <= 0) {
            alert('Please enter a valid time.');
            return;
        }
        setGameActive(true);
        setCorrectAnswers(0);
        setIncorrectAnswers(0);
        setTimeRemaining(inputTime);
        fetchNextRandomNumber();
    };

    const fetchNextRandomNumber = () => {
        axios
            .get(`http://localhost:5154/api/fizzbuzzgame/next/${gameId}`)
            .then((response) => {
                setCurrentNumber(response.data.randomNumber);
            })
            .catch((error) => {
                console.error('Error fetching next random number:', error);
            });
    };

    const handleSubmitAnswer = (answer: string) => {
        if (currentNumber !== null) {
            axios
                .post(`http://localhost:5154/api/fizzbuzzgame/submit/${gameId}`, {
                    number: currentNumber,
                    answer,
                })
                .then((response) => {
                    if (response.data.isCorrect) {
                        setCorrectAnswers((prev) => prev + 1);
                    } else {
                        setIncorrectAnswers((prev) => prev + 1);
                    }
                    fetchNextRandomNumber();
                    if (answerInputRef.current) {
                        answerInputRef.current.value = ''; // Clear input field
                    }
                })
                .catch((error) => {
                    console.error('Error submitting answer:', error);
                });
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
            <h1>{gameData.gameName} - Game Page</h1>
            <p>Author: {gameData.author}</p>

            <h3>Divisor and Word Pairs:</h3>
            <ul>
                {gameData.divisorWordPairs.$values.map((pair: any) => (
                    <li key={pair.id}>
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
                    <button onClick={handleStartGame}>Start Game</button>
                </div>
            )}

            {gameActive && (
                <div>
                    <p>Time Remaining: {timeRemaining}s</p>
                    <p>Current Number: {currentNumber}</p>
                    <input
                        ref={answerInputRef} // Set ref
                        type="text"
                        placeholder="Enter your answer"
                        onKeyDown={(e) => {
                            if (e.key === 'Enter') {
                                handleSubmitAnswer((e.target as HTMLInputElement).value);
                            }
                        }}
                    />
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
