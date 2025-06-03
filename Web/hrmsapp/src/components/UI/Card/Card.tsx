import React from 'react';
import type { ReactNode } from 'react';

interface CardProps {
    title?: string;
    children: ReactNode;
    className?: string;
    footer?: ReactNode;
}

export const Card: React.FC<CardProps> = ({
    title,
    children,
    className = '',
    footer
}) => {
    return (
        <div className={`bg-background dark:bg-background-dark rounded-lg shadow-md overflow-hidden ${className}`}>
            {title && (
                <div className="border-background-dark border-b px-4 py-3 dark:border-background-light">
                    <h3 className="text-text text-lg font-medium dark:text-text-light">{title}</h3>
                </div>
            )}
            <div className="p-4">
                {children}
            </div>
            {footer && (
                <div className="bg-background-light border-background-dark border-t px-4 py-3 dark:bg-background dark:border-background-light">
                    {footer}
                </div>
            )}
        </div>
    );
};