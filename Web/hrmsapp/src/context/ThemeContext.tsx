// src/context/ThemeContext.tsx
import React, { createContext, useState, useEffect, ReactNode } from 'react';
import { Theme, ThemeContextType, ThemeMode } from '../types/theme';
import { defaultTheme, darkTheme } from '../utils/themes';

export const ThemeContext = createContext<ThemeContextType>({
    currentTheme: defaultTheme,
    themeMode: 'light',
    setTheme: () => { },
    setThemeMode: () => { },
});

interface ThemeProviderProps {
    children: ReactNode;
}

export const ThemeProvider: React.FC<ThemeProviderProps> = ({ children }) => {
    const [currentTheme, setCurrentTheme] = useState<Theme>(defaultTheme);
    const [themeMode, setThemeMode] = useState<ThemeMode>('light');

    // Initialize theme from local storage if available
    useEffect(() => {
        try {
            // Get saved theme as JSON object (if exists)
            const savedThemeStr = localStorage.getItem('theme');
            if (savedThemeStr) {
                const savedTheme = JSON.parse(savedThemeStr);
                setCurrentTheme(savedTheme);
            }

            // Get saved mode as a plain string (NOT JSON)
            const savedMode = localStorage.getItem('themeMode') as ThemeMode | null;
            if (savedMode && (savedMode === 'light' || savedMode === 'dark' || savedMode === 'system')) {
                setThemeMode(savedMode);
            } else if (window.matchMedia('(prefers-color-scheme: dark)').matches) {
                setThemeMode('system');
            }
        } catch (error) {
            console.error('Error loading theme from localStorage:', error);
            // Fallback to default theme if there's an error
            setCurrentTheme(defaultTheme);
            setThemeMode('light');
        }
    }, []);

    // Apply the theme CSS variables to the document root
    useEffect(() => {
        try {
            const root = document.documentElement;
            const theme = themeMode === 'dark' || (themeMode === 'system' && window.matchMedia('(prefers-color-scheme: dark)').matches)
                ? darkTheme
                : currentTheme;

            // Set CSS variables
            root.style.setProperty('--color-primary', theme.colors.primary);
            root.style.setProperty('--color-primary-light', theme.colors.primaryLight);
            root.style.setProperty('--color-primary-dark', theme.colors.primaryDark);
            root.style.setProperty('--color-secondary', theme.colors.secondary);
            root.style.setProperty('--color-secondary-light', theme.colors.secondaryLight);
            root.style.setProperty('--color-secondary-dark', theme.colors.secondaryDark);
            root.style.setProperty('--color-background', theme.colors.background);
            root.style.setProperty('--color-background-light', theme.colors.backgroundLight);
            root.style.setProperty('--color-background-dark', theme.colors.backgroundDark);
            root.style.setProperty('--color-text', theme.colors.text);
            root.style.setProperty('--color-text-light', theme.colors.textLight);
            root.style.setProperty('--color-text-dark', theme.colors.textDark);

            // Set dark mode class
            if (themeMode === 'dark' || (themeMode === 'system' && window.matchMedia('(prefers-color-scheme: dark)').matches)) {
                document.documentElement.classList.add('dark');
            } else {
                document.documentElement.classList.remove('dark');
            }

            // Save to local storage - theme as JSON, themeMode as plain string
            localStorage.setItem('theme', JSON.stringify(currentTheme));
            localStorage.setItem('themeMode', themeMode);
        } catch (error) {
            console.error('Error applying theme:', error);
        }
    }, [currentTheme, themeMode]);

    // Listen for system theme changes
    useEffect(() => {
        if (themeMode === 'system') {
            const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
            const handleChange = () => {
                document.documentElement.classList.toggle('dark', mediaQuery.matches);
            };

            mediaQuery.addEventListener('change', handleChange);
            return () => mediaQuery.removeEventListener('change', handleChange);
        }
    }, [themeMode]);

    const setThemeHandler = (theme: Theme) => {
        setCurrentTheme(theme);
    };

    return (
        <ThemeContext.Provider value={{
            currentTheme,
            themeMode,
            setTheme: setThemeHandler,
            setThemeMode
        }}>
            {children}
        </ThemeContext.Provider>
    );
};