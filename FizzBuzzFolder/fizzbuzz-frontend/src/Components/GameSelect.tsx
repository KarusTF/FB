// components/GameSelect.tsx
import * as React from 'react';
import { useState, useEffect } from 'react';
import { gameService } from '../services/api'; // Use the service layer

interface GameSelectProps {
    onSelect: (gameName: string) => void;
}

const GameSelect: React.FC<GameSelectProps> = ({ onSelect }) => {
    const [gameNames, setGameNames] = useState<string[]>([]); // List of game names
    const [loading, setLoading] = useState<boolean>(true); // Loading state

    // Fetch game names on component mount
    useEffect(() => {
        gameService.getGameNames()
            .then((data) => {
                setGameNames(data);
                setLoading(false);
            })
            .catch((error) => {
                console.error('Error fetching game names:', error);
                alert('Failed to fetch game names. Please try again later.');
                setLoading(false);
            });
    }, []);

    const handleButtonClick = (gameName: string) => {
        onSelect(gameName); // Pass selected gameName back to HomePage
    };

    if (loading) {
        return <div>Loading game names...</div>; // Show loading message
    }

    return (
        <div>
            <h3>Select a Game</h3>
            {gameNames.length > 0 ? (
                <select onChange={(e) => handleButtonClick(e.target.value)} defaultValue="">
                    <option value="">Select a Game</option>
                    {gameNames.map((gameName, index) => (
                        <option key={index} value={gameName}>
                            {gameName}
                        </option>
                    ))}
                </select>
            ) : (
                <p>No games available</p> // Show message if no games found
            )}
        </div>
    );
};

export default GameSelect;