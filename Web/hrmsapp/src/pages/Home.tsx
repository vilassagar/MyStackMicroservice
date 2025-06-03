// src/pages/Home.tsx
import React from 'react';
import { Card } from '../components/common/UI/Card/Card';
import { Button } from '../components/common/UI/Button/Button';


 const Home: React.FC = () => {
    return (
        <div className="bg-background-light min-h-screen dark:bg-background-dark">
            <header className="bg-background shadow-sm dark:bg-background-dark">
                <div className="mx-auto max-w-7xl px-4 py-4 sm:px-6 lg:px-8">
                    <h1 className="text-text text-2xl font-bold dark:text-text-light">My App</h1>
                </div>
            </header>

            <main className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
                <div className="grid grid-cols-1 gap-6 md:grid-cols-3">
                    <div className="md:col-span-2">
                        <Card
                            title="Welcome to our themed application"
                            className="mb-6"
                            footer={
                                <div className="flex justify-end space-x-2">
                                    <Button variant="ghost">Cancel</Button>
                                    <Button>Save</Button>
                                </div>
                            }
                        >
                            <p className="text-text-light mb-4 dark:text-text">
                                This is a sample application demonstrating a React architecture with TypeScript and Tailwind CSS with theme customization.
                            </p>
                            <div className="flex space-x-2">
                                <Button>Primary Button</Button>
                                <Button variant="secondary">Secondary</Button>
                                <Button variant="outline">Outline</Button>
                                <Button variant="ghost">Ghost</Button>
                            </div>
                        </Card>

                        <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
                            {[1, 2, 3, 4].map((item) => (
                                <Card key={item} className="h-40">
                                    <div className="flex h-full items-center justify-center">
                                        <p className="text-text-light dark:text-text">Card {item}</p>
                                    </div>
                                </Card>
                            ))}
                        </div>
                    </div>

                   
                </div>
            </main>
        </div>
    );
};
export default  Home;