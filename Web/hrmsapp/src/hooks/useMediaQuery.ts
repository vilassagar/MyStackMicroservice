// src/hooks/useMediaQuery.ts
import { useState, useEffect } from 'react';

export function useMediaQuery(query: string): boolean {
    const [matches, setMatches] = useState(false);

    useEffect(() => {
        // Ensure we're in the browser environment
        if (typeof window !== 'undefined') {
            const media = window.matchMedia(query);

            // Set the initial value
            setMatches(media.matches);

            // Define a callback function to handle changes
            const listener = () => {
                setMatches(media.matches);
            };

            // Listen for changes
            media.addEventListener('change', listener);

            // Clean up the listener when the component unmounts
            return () => {
                media.removeEventListener('change', listener);
            };
        }
    }, [query]);

    return matches;
}
