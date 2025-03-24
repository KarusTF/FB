// pages/HomePage.tsx
import * as React from 'react';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import GameSelect from '../Components/GameSelect';
import CreateGameForm from '../Components/CreateGameForm';
import { gameService } from '../services/api';

const HomePage: React.FC = () => {
    const [games, setGames] = useState<string[]>([]);
    const [selectedGame, setSelectedGame] = useState<string | null>(null);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchGames = async () => {
            try {
                const gameNames = await gameService.getGameNames();
                setGames(gameNames);
            } catch (error) {
                alert('Failed to fetch games. Please try again.');
            }
        };
        fetchGames();
    }, []);

    const handleBeginGame = () => {
        if (selectedGame) {
            navigate(`/game/${selectedGame}`);
        } else {
            alert('Please select a game first.');
        }
    };

    return (
        <div>
            <h1>Welcome to FizzBuzz</h1>
            <button onClick={handleBeginGame}>Begin</button>
            <GameSelect onSelect={setSelectedGame} />
            <CreateGameForm />
        </div>
    );
};

export default HomePage;