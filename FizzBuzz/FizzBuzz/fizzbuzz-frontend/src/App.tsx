import React from 'react';
import { Route, Routes } from 'react-router-dom';
import { useParams } from 'react-router-dom';
import HomePage from './Pages/HomePage';
import GamePage from './Pages/GamePage';

const App: React.FC = () => {
    return (
        <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/game/:gameId" element={<GamePage />} />

        </Routes>

    );
};

export default App;
