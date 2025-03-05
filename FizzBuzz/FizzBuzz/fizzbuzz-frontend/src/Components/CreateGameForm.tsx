import React, { useState } from 'react';
import axios from 'axios';

const CreateGameForm: React.FC = () => {
    const [gameName, setGameName] = useState('');
    const [author, setAuthor] = useState('');
    const [divisorWordPairs, setDivisorWordPairs] = useState([
        { divisor: 0, word: '' },
        { divisor: 0, word: '' },
    ]);
    const [error, setError] = useState('');
    const [successMessage, setSuccessMessage] = useState('');

    // Handle the changes in divisor or word input
    const handleDivisorWordChange = (index: number, field: string, value: string) => {
        const updatedDivisorWordPairs = [...divisorWordPairs]; // Create a shallow copy of the array
        updatedDivisorWordPairs[index] = {
            ...updatedDivisorWordPairs[index], // Copy the existing pair object
            [field]: value, // Update the specific field
        };
        setDivisorWordPairs(updatedDivisorWordPairs); // Set the updated state
    };

    // Validate that all divisors are positive numbers
    const isValidDivisors = () => {
        return divisorWordPairs.every(pair => pair.divisor > 0);
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        // Must be positive number
        if (!isValidDivisors()) {
            setError('Divisors must be positive numbers');
            return;
        }

        setError(''); // Clear previous errors

        try {
            const response = await axios.post('http://localhost:5154/api/fizzbuzzrules', {
                gameName: gameName,  
                author: author,     
                DPs: divisorWordPairs
            });

            if (response.data.gameName && response.data.author) {
                setSuccessMessage(`Game Created Successfully! Game Name: ${response.data.gameName}, Author: ${response.data.author}`);
            } else {
                setError('Error: Missing required fields in the response.');
            }
        } catch (error) {
            console.error('Error creating game:', error);
            setError('Error creating game.');
        }

    };


    return (
        <form onSubmit={handleSubmit}>
            <h3>Create New Game</h3>

            {/* Game Name */}
            <input
                type="text"
                placeholder="Game Name"
                value={gameName}
                onChange={(e) => setGameName(e.target.value)}
                required
            />

            {/* Author */}
            <input
                type="text"
                placeholder="Author"
                value={author}
                onChange={(e) => setAuthor(e.target.value)}
                required
            />

            {/* Divisor-Word Pair 1 */}
            <div>
                <h4>Divisor-Word Pair 1</h4>
                <input
                    type="number"
                    placeholder="Divisor 1"
                    value={divisorWordPairs[0].divisor}
                    onChange={(e) => handleDivisorWordChange(0, 'divisor', e.target.value)}
                    min="1"
                    required
                />
                <input
                    type="text"
                    placeholder="Word 1"
                    value={divisorWordPairs[0].word}
                    onChange={(e) => handleDivisorWordChange(0, 'word', e.target.value)}
                    required
                />
            </div>

            {/* Divisor-Word Pair 2 */}
            <div>
                <h4>Divisor-Word Pair 2</h4>
                <input
                    type="number"
                    placeholder="Divisor 2"
                    value={divisorWordPairs[1].divisor}
                    onChange={(e) => handleDivisorWordChange(1, 'divisor', e.target.value)}
                    min="1"
                    required
                />
                <input
                    type="text"
                    placeholder="Word 2"
                    value={divisorWordPairs[1].word}
                    onChange={(e) => handleDivisorWordChange(1, 'word', e.target.value)}
                    required
                />
            </div>

            {/* Error and Success Messages */}
            {error && <p style={{ color: 'red' }}>{error}</p>}
            {successMessage && <p style={{ color: 'green' }}>{successMessage}</p>}

            <button type="submit">Create Game</button>
        </form>
    );
};

export default CreateGameForm;
