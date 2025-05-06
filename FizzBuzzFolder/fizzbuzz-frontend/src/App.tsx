import { BrowserRouter, Routes, Route } from 'react-router-dom';
import HomePage from './Pages/HomePage';
import GamePage from './Pages/GamePage';

const App = () => {
    return (
            <Routes>
                <Route path="/" element={<HomePage />} />
                <Route path="/game/:gameId" element={<GamePage />} />
            </Routes>
    );
};

export default App;