// pages/HomePage.tsx
import * as React from 'react';
import {  useState } from 'react';
import { useNavigate } from 'react-router-dom';
import GameSelect from '../Components/GameSelect';
import CreateGameForm from '../Components/CreateGameForm';


const HomePage: React.FC = () => {
    const [selectedGame, setSelectedGame] = useState<string | null>(null);
    const navigate = useNavigate();

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