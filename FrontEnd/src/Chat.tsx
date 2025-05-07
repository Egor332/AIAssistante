import React, {useState, useRef, useEffect} from 'react';

interface Message {
    role: "user" | "model";
    text: string;
}

interface ChatHistory {
    history: Message[];
}

const Chat: React.FC = () => {
    const [messages, setMessages] = useState<Message[]>([]);
    const [inputValue, setInputValue] = useState<string>("");
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const messagesEndRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
    }, [messages]);

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setInputValue(e.target.value);
    };

    const sendMessage = async () => {
        if (inputValue.trim() === '') return;


        const userMessage: Message = {role: "user", text: inputValue};
        const updatedMessages = [...messages, userMessage];
        setMessages(updatedMessages);
        setInputValue('');
        setIsLoading(true);

        try {
            const chatHistory: ChatHistory = {
                history: updatedMessages
            };

            const response = await fetch(`${import.meta.env.VITE_API_URL}/Messages/chat-history`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(chatHistory),
            });

            if (!response.ok) {
                console.log("Something went wrong");
            }

            const text = await response.json();

            const assistantMessage: Message = {
                role: 'model',
                text: text
            };
            setMessages([...updatedMessages, assistantMessage]);
        } catch (error) {
            console.error('Error:', error);
            // Add error message
            setMessages([
                ...updatedMessages,
                {role: 'model', text: 'Sorry, an error occurred. Please try again.'}
            ]);
        } finally {
            setIsLoading(false);
        }
    }

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        sendMessage();
    }

    return (
        <div className="flex flex-col h-screen max-w-2xl mx-auto p-4">
            <div className="flex-1 overflow-y-auto mb-4 border border-gray-300 rounded p-4">
                {messages.map((message, index) => (
                    <div
                        key={index}
                        className={`mb-4 p-3 rounded-lg ${
                            message.role === 'user'
                                ? 'bg-blue-100 ml-auto max-w-xs md:max-w-md'
                                : 'bg-gray-100 mr-auto max-w-xs md:max-w-md'
                        }`}
                    >
                        <div className="font-bold">
                            {message.role === 'user' ? 'You' : 'Assistant'}
                        </div>
                        <div className="whitespace-pre-wrap">{message.text}</div>
                    </div>
                ))}
                {isLoading && (
                    <div className="bg-gray-100 p-3 rounded-lg mr-auto max-w-xs md:max-w-md">
                        <div className="font-bold">Assistant</div>
                        <div>Thinking...</div>
                    </div>
                )}
                <div ref={messagesEndRef} />
            </div>

            <form onSubmit={handleSubmit} className="flex">
                <input
                    type="text"
                    value={inputValue}
                    onChange={handleInputChange}
                    placeholder="Type your message..."
                    className="flex-1 p-2 border border-gray-300 rounded-l focus:outline-none focus:border-blue-500"
                    disabled={isLoading}
                />
                <button
                    type="submit"
                    className="bg-blue-500 text-white p-2 rounded-r disabled:bg-blue-300"
                    disabled={isLoading || inputValue.trim() === ''}
                >
                    Send
                </button>
            </form>
        </div>
    );
};

export default Chat;