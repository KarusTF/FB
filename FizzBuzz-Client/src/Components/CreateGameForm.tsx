// components/CreateGameForm.tsx
import * as React from 'react';
import  { useState } from 'react';
import { DivisorWordPair } from '../Models/game';
import { gameService } from '../services/api';

const CreateGameForm: React.FC = () => {
    const [GameName, setGameName] = useState('');
    const [Author, setAuthor] = useState('');
    const [DivisorWordPairs, setDivisorWordPairs] = useState<DivisorWordPair[]>([
        {fizzBuzzRuleId: 0,divisor: 0, word: '' },
        {fizzBuzzRuleId: 0,divisor: 0, word: '' },
    ]);
    const [error, setError] = useState('');
    const [successMessage, setSuccessMessage] = useState('');

    const handleDivisorWordChange = (index: number, field: keyof DivisorWordPair, value: string | number) => {
        const updatedPairs = [...DivisorWordPairs];
        updatedPairs[index] = { ...updatedPairs[index], [field]: value };
        setDivisorWordPairs(updatedPairs);
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        
        if (DivisorWordPairs.some(pair => pair.divisor <= 0)) {
            setError('Divisors must be positive numbers.');
            return;
        }

        const requestPayload = {
            GameName:GameName,
            Author: Author,
            DivisorWordPairs: DivisorWordPairs.map(pair => ({
                FizzBuzzRuleId: 0,
                Divisor: pair.divisor,
                Word: pair.word
            })),
        };

        console.log("Payload:", requestPayload);
        try {
            const response = await gameService.createGame(requestPayload);
            if (response.gameName==requestPayload.GameName){
                setSuccessMessage(`Game created successfully.`);
                setError('');
            }
            else{
                setError('Failed to create game.');
            }
            setGameName('');
            setAuthor('');
            setDivisorWordPairs([
                { fizzBuzzRuleId: 0, divisor: 0, word: '' },
                { fizzBuzzRuleId: 0, divisor: 0, word: '' }
            ]);
        } catch (error) {
            setError('Failed to create game. Please try again.');
            console.error(error);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <h3>Create New Game</h3>
            <input
                type="text"
                placeholder="Game Name"
                value={GameName}
                onChange={(e) => setGameName(e.target.value)}
                required
            />
            <input
                type="text"
                placeholder="Author"
                value={Author}
                onChange={(e) => setAuthor(e.target.value)}
                required
            />
            {DivisorWordPairs.map((pair, index) => (
                <div key={index}>
                    <h4>Divisor-Word Pair {index + 1}</h4>
                    <input 
                        type="number"
                        placeholder="Divisor"
                        value={pair.divisor}
                        onChange={(e) => handleDivisorWordChange(index, 'divisor', parseInt(e.target.value))}
                        min="1"
                        max="100"
                        required
                    />
                    <input 
                        type="text"
                        placeholder="Word"
                        value={pair.word}
                        onChange={(e) => handleDivisorWordChange(index, 'word', e.target.value)}
                        required
                    />
                </div>
            ))}
            {error && <p style={{ color: 'red' }}>{error}</p>}
            {successMessage && <p style={{ color: 'green' }}>{successMessage}</p>}
            <button style={{ marginTop: '35px' }} type="submit">Create Game</button>
        </form>
    );
};

export default CreateGameForm; 