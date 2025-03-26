import { describe, it, expect, vi } from 'vitest';
import { renderToStaticMarkup } from 'react-dom/server';
import React from 'react';
import HomePageForTesting from './HomePageForTesting';

vi.mock('../../fizzbuzz-frontend/src/Components/GameSelect', () => ({
    default: () => <div className="game-select-mock">GameSelect Mock</div>
}));

vi.mock('../../fizzbuzz-frontend/src/Components/CreateGameForm', () => ({
    default: () => <div className="create-form-mock">CreateGameForm Mock</div>
}));

describe('HomePageForTesting - Static HTML Tests', () => {
    it('renders basic structure without errors', () => {
        const html = renderToStaticMarkup(<HomePageForTesting />);
        expect(html).toMatchSnapshot();
    });

    it('contains all main elements', () => {
        const html = renderToStaticMarkup(<HomePageForTesting />);

        expect(html).toContain('<h1>Welcome to FizzBuzz</h1>');
        expect(html).toContain('<button>Begin</button>');
        expect(html).toContain('GameSelect Mock');
        expect(html).toContain('CreateGameForm Mock');
    });

    it('has elements in correct order', () => {
        const html = renderToStaticMarkup(<HomePageForTesting />);
        const elements = [
            { name: 'h1', content: 'Welcome to FizzBuzz' },
            { name: 'button', content: 'Begin' },
            { name: 'div', content: 'GameSelect Mock' },
            { name: 'div', content: 'CreateGameForm Mock' }
        ];

        let lastIndex = -1;
        elements.forEach(element => {
            const index = html.indexOf(element.content);
            expect(index).toBeGreaterThan(lastIndex);
            lastIndex = index;
        });
    });
});