// src/TestFrontEnd/HomePageTest.test.tsx
import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import sinon from 'sinon';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import HomePage from '../Pages/HomePage';
import GameSelect from '../Components/GameSelect';

const mockNavigate = sinon.stub();

sinon.stub(axios, 'get').resolves({
    data: [
        { id: 1, gameName: 'Game 1' },
        { id: 2, gameName: 'Game 2' }
    ]
});

describe('HomePage Component Tests', () => {

    beforeEach(() => {
        mockNavigate.reset();
    });

    it('should fetch and display games on load', async () => {
        render(
            <BrowserRouter>
                <HomePage />
            </BrowserRouter>
        );

        await waitFor(() => screen.getByText('Game 1'));
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

        const gameSelectButton = screen.getByText('Game 1');
        fireEvent.click(gameSelectButton);

        const beginButton = screen.getByText('Begin');
        fireEvent.click(beginButton);

        expect(mockNavigate.calledWith('/game/Game 1')).toBe(true);
    });

    it('should alert if no game is selected and "Begin" is clicked', async () => {
        render(
            <BrowserRouter>
                <HomePage />
            </BrowserRouter>
        );

        await waitFor(() => screen.getByText('Game 1'));

        const beginButton = screen.getByText('Begin');
        const alertSpy = sinon.spy(window, 'alert');  // Mock window alert

        fireEvent.click(beginButton);
        expect(alertSpy.calledWith('Please select a game first.')).toBe(true);

        alertSpy.restore(); 
    });

});
