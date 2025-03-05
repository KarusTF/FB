// src/TestFrontEnd/HomePageTest.test.tsx
import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import sinon from 'sinon';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import HomePage from '../Pages/HomePage';
import GameSelect from '../Components/GameSelect';

// Mock the useNavigate hook
const mockNavigate = sinon.stub();

// Mock axios to avoid actual API calls
sinon.stub(axios, 'get').resolves({
    data: [
        { id: 1, gameName: 'Game 1' },
        { id: 2, gameName: 'Game 2' }
    ]
});

describe('HomePage Component Tests', () => {

    beforeEach(() => {
        // Reset any mocks before each test
        mockNavigate.reset();
    });

    it('should fetch and display games on load', async () => {
        render(
            <BrowserRouter>
                <HomePage />
            </BrowserRouter>
        );

        await waitFor(() => screen.getByText('Game 1')); // wait for the game to be rendered
        expect(screen.getByText('Game 1')).toBeInTheDocument();
        expect(screen.getByText('Game 2')).toBeInTheDocument();
    });

    it('should navigate when a game is selected and "Begin" is clicked', async () => {
        render(
            <BrowserRouter>
                <HomePage />
            </BrowserRouter>
        );

        await waitFor(() => screen.getByText('Game 1'));

        // Simulate selecting a game (this assumes that GameSelect is interacting with an onSelect function)
        const gameSelectButton = screen.getByText('Game 1');
        fireEvent.click(gameSelectButton);

        // Simulate clicking the "Begin" button
        const beginButton = screen.getByText('Begin');
        fireEvent.click(beginButton);

        // Check if navigate was called with the correct game path
        expect(mockNavigate.calledWith('/game/Game 1')).toBe(true);
    });

    it('should alert if no game is selected and "Begin" is clicked', async () => {
        render(
            <BrowserRouter>
                <HomePage />
            </BrowserRouter>
        );

        await waitFor(() => screen.getByText('Game 1'));

        // Click the "Begin" button without selecting a game
        const beginButton = screen.getByText('Begin');
        const alertSpy = sinon.spy(window, 'alert');  // Mock window alert

        fireEvent.click(beginButton);

        // Check if alert was triggered
        expect(alertSpy.calledWith('Please select a game first.')).toBe(true);

        alertSpy.restore(); // Restore the original alert function
    });

});
