import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import GameSelect from '../Components/GameSelect';
import CreateGameForm from '../Components/CreateGameForm';
import axios from 'axios';

export const fetchGames = async (setGames: (games: { id: number, gameName: string }[]) => void) => {
    try {
        const response = await axios.get('http://localhost:5154/api/FizzBuzzRules/games');
        setGames(response.data);
    } catch (error) {
        console.error('Error fetching games:', error);
    }
};

export const handleGameSelect = (gameName: string, setSelectedGame: (game: string | null) => void) => {
    setSelectedGame(gameName);
};

export const handleBeginGame = (selectedGame: string | null, navigate: (path: string) => void) => {
    if (selectedGame) {
        navigate(`/game/${selectedGame}`);
    } else {
        alert('Please select a game first.');
    }
};

const HomePage: React.FC = () => {
    const [games, setGames] = useState<{ id: number, gameName: string }[]>([]);
    const [selectedGame, setSelectedGame] = useState<string | null>(null);
    const navigate = useNavigate();

    useEffect(() => {
        fetchGames(setGames);
    }, []);

    return (
        <div>
            <h1>Welcome to FizzBuzz</h1>
            <button onClick={() => handleBeginGame(selectedGame, navigate)}>Begin</button>
            <GameSelect onSelect={(game) => handleGameSelect(game, setSelectedGame)} />
            <CreateGameForm />
        </div>
    );
};

export default HomePage;
