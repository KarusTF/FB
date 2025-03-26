import * as React from 'react';
import * as ReactDOM from 'react-dom/client';
import App from './App';
import { BrowserRouter } from 'react-router-dom'; // Import BrowserRouter

// Create the root and render the app
const root = ReactDOM.createRoot(document.getElementById('root') as HTMLElement); // Get the root element

root.render(
    <BrowserRouter>
        <App />
    </BrowserRouter>
);
