import React from 'react';
import ReactDOM from 'react-dom/client'; // Import the new react-dom/client module
import App from './App';
import { BrowserRouter } from 'react-router-dom'; // Import BrowserRouter

// Create the root and render the app
const root = ReactDOM.createRoot(document.getElementById('root') as HTMLElement); // Get the root element

root.render(
    <BrowserRouter>
        <App />
    </BrowserRouter>
);
