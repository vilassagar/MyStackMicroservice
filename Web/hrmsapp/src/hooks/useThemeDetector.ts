// src/hooks/useThemeDetector.ts
import { useEffect, useState } from 'react';

export function useThemeDetector(): boolean {
    const [isDarkTheme, setIsDarkTheme] = useState(false);

    useEffect(() => {
        const darkMediaQuery = window.matchMedia('(prefers-color-scheme: dark)');

        // Set initial value
        setIsDarkTheme(darkMediaQuery.matches);

        // Define a callback function to handle changes
        const listener = (e: MediaQueryListEvent) => {
            setIsDarkTheme(e.matches);
        };

        // Add listener
        darkMediaQuery.addEventListener('change', listener);

        // Clean up
        return () => {
            darkMediaQuery.removeEventListener('change', listener);
        };
    }, []);

    return isDarkTheme;
}
