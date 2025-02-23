export function ChatMessage({chatMessageInfo}) {
    return <div className="chat-message">
        <div>{chatMessageInfo.sender}</div>
        <div>{chatMessageInfo.message}</div>
    </div>;
}