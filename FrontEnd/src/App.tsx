import React from 'react';
import Chat from './Chat';

const App: React.FC = () => {
    return (
        <div className="bg-gray-50 min-h-screen">
            <header className="bg-white shadow p-4">
                <h1 className="text-xl font-bold text-center text-gray-800">Chat with Assistant</h1>
            </header>
            <main>
                <Chat />
            </main>
        </div>
    );
};

export default App
