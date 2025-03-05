import React, { useState, useEffect } from 'react';
import axios from 'axios';

interface GameSelectProps {
    onSelect: (gameName: string) => void;
}

const GameSelect: React.FC<GameSelectProps> = ({ onSelect }) => {
    const [gameNames, setGameNames] = useState<string[]>([]);  // List of game names
    const [loading, setLoading] = useState<boolean>(true);  // Loading state

    useEffect(() => {
        // Fetch game names from the backend API
        axios
            .get('http://localhost:5154/api/FizzBuzzRules/games')
            .then((response) => {
                console.log('Fetched game names:', response.data);
                setGameNames(response.data.$values);  // Assuming response contains an array of game names
                setLoading(false);  // Turn off loading
            })
            .catch((error) => {
                console.error('Error fetching game names:', error);
                setLoading(false);  // Turn off loading on error
            });
    }, []);

    const handleButtonClick = (gameName: string) => {
        onSelect(gameName);  // Pass selected gameName back to HomePage
    };

    if (loading) {
        return <div>Loading game names...</div>;  // Show loading message
    }

    return (
        <div>
            <h3>Select a Game</h3>
            {gameNames ? (
                <select onChange={(e) => handleButtonClick(e.target.value)} defaultValue="">
                    <option value="">Select a Game</option>
                    {gameNames.map((gameName, index) => (
                        <option key={index} value={gameName}>
                            {gameName}
                        </option>
                    ))}
                </select>
            ) : (
                <p>No games available</p>  // Show message if no games found
            )}
        </div>
    );
};

export default GameSelect;
