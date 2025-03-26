import * as React from 'react';
import GameSelect from '../../fizzbuzz-frontend/src/Components/GameSelect';
import CreateGameForm from '../../fizzbuzz-frontend/src/Components/CreateGameForm';

const HomePageForTesting: React.FC = () => {
    
    return (
        <div>
            <h1>Welcome to FizzBuzz</h1>
            <button>Begin</button>
            <GameSelect onSelect={() => {}} />
            <CreateGameForm />
        </div>
    );
};

export default HomePageForTesting;