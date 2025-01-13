export default class ChatModel
{
    #service
    constructor(service) {
        this.#service = service;
    }
    getChat(id){
       return  this.#service.getChatById(id);
    }
}