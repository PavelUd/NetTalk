import {ApiService} from "./api-service.js";

export default class ChatAPIService extends ApiService
{
    
    #connection
    async getChatById(id){
        const response = await this._load(
            {
                url: `api/chats/${id}`,
            });
        return ApiService.parseResponse(response);
    }

   async getUserChats(){
       const response = await this._load(
           {
               url: `api/chats/`,
           });
       return ApiService.parseResponse(response);
    }
    
    async connect(){
        this.#connection = new signalR.HubConnectionBuilder()
            .withUrl(`/chathub`).build();
        await this.#connection.start()
    }
    
    async joinChat(id){
        console.log(id);
        await this.#connection.invoke("JoinPrivateChat", id.toString())
                .catch(err => console.error(err.toString()));
    }
    
    async send(message, id){
        this.#connection.invoke("SendMessage", id.toString(), message)
            .catch(err => console.error(err.toString()));
    }

    async updateMessage(text,idMessage, id){
        this.#connection.invoke("UpdateMessage", id.toString(),idMessage.toString(), text)
            .catch(err => console.error(err.toString()));
    }

    async deleteMessage(id, idMessage){
        this.#connection.invoke("DeleteMessage", id.toString(), idMessage.toString())
            .catch(err => console.error(err.toString()));
    }
    
    receiveDeleteMessage(callback){
        this.#connection.off("ReceiveDeleteMessage");
        this.#connection.on("ReceiveDeleteMessage", (data) => {
            callback(data)
        });
    }

    receiveUpdateMessage(callback){
        this.#connection.off("ReceiveUpdateMessage");
        this.#connection.on("ReceiveUpdateMessage", (data) => {
            callback(data)
        });
    }
   
    receive(callback) {
        this.#connection.off("ReceiveMessage");
        this.#connection.on("ReceiveMessage", (data) => {
            callback(data)
        });
    }

    receiveInitChatFromOtherUser(callback){
        this.#connection.off("ReceiveInitChat");
        this.#connection.on("ReceiveInitChat", (data) => {
            callback(data)
        });
    }
    
    receiveInitChat(callback){
        this.#connection.off("InitChat");
        this.#connection.on("InitChat", (data) => {
            callback(data)
        });
    }
    
    async create(message, users){
        
        await this.#connection.invoke("InitPrivateChat", message,users)
            .catch(err => console.error(err.toString()));
    }
    
    async search(login){
        const response = await this._load(
            {
                url: `api/chats/search?Login=${login}`,
            });
        return ApiService.parseResponse(response);
    }
    
    async leaveChat(id){
        await this.#connection.invoke("LeavePrivateChat", id)  
            .catch(err => console.error(err.toString()));
    }
    async disconnect() {
        await this.#connection.stop()
            .then(() => console.log("Disconnected"))
            .catch(err => console.error("Error disconnecting:", err));
    }
}