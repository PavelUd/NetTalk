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
    
   
    receive(callback){
        this.#connection.off("ReceiveMessage");
        this.#connection.on("ReceiveMessage", (data) => {
            callback(data)
        });
    }
    
    async create(data){
        console.log(data)
 //       return ApiService.parseResponse(response);
    }
    
    async search(login){
        const response = await this._load(
            {
                url: `api/chats/search?Login=${login}`,
            });
        return ApiService.parseResponse(response);
    }
    
    async leaveChat(id){
        console.log(id);
        await this.#connection.invoke("LeavePrivateChat", id)
            .catch(err => console.error(err.toString()));
    }
    async disconnect() {
        await this.#connection.stop()
            .then(() => console.log("Disconnected"))
            .catch(err => console.error("Error disconnecting:", err));
    }
}