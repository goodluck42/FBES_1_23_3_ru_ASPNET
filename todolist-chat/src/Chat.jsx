import {use, useEffect, useRef, useState} from "react";
import {ChatMessage} from "./ChatMessage.jsx";
import './SignalRClient.js'
import {SignalRClient} from "./SignalRClient.js";


export function Chat() {
    const [messages, setMessages] = useState([]); // {message: "Text", sender: "Ali"}
    const [userName, setUserName] = useState("");
    const signalRClient = useRef(new SignalRClient());
    const messageInput = useRef();

    function sendMessageHandler(e) {
        console.log("sent");
        signalRClient.current.send({
            sender: userName,
            message: messageInput.current.value,
        });

        messageInput.current.value = "";
    }

    signalRClient.current.onReceive((chatMessageInfo) => {
        console.log("received");

        setMessages([...messages, chatMessageInfo]);
    });

    useEffect(() => {

        signalRClient.current.start();
        setUserName(prompt("Enter username: "))

        // setMessages([{
        //     message: "Hello",
        //     sender: "Ali"
        // }, {
        //     message: "Hello",
        //     sender: "Ali"
        // }])
    }, [])

    return <>
        <div id="chat-username">{userName}</div>
        <div id="chat-wrapper">
            <div id="chat">
                {messages.map((message, idx) => <ChatMessage key={idx} chatMessageInfo={message}/>)}
            </div>
            <input ref={messageInput} type="text" id="chat-message" placeholder="Message..."/>
            <input type="button" id="chat-send" value="Send" onClick={sendMessageHandler}/>
        </div>
    </>;
}