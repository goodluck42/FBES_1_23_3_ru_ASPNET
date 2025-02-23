import {HubConnectionBuilder} from '@microsoft/signalr'

export class SignalRClient {
    constructor() {
        this._hubConnection = new HubConnectionBuilder().withUrl('http://26.76.43.135:5032/chat').build();
    }

    async send(chatMessageInfo) {
        try {
            await this._hubConnection.invoke("SendMessage", chatMessageInfo);
        } catch (err) {
            console.error(err);
        }
    }

    onReceive(callback) {
        this._hubConnection.on("ReceiveMessage", callback);
    }

    async start() {
        await this._hubConnection.start();
    }
}