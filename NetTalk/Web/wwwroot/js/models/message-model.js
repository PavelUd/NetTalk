import Observable from "../framework/observable.js";
import {UpdateType} from "../utils/const.js";
import {adaptToClient} from "../utils/adapter.js";

export default class MessageModel extends Observable
{
    #service
    #info
    #messages
    #id
    constructor(service) {
        super();
        this.#service = service;
    }
    async initEmptyChat({userId, url, name}){
        try{
            this.#info = {name: name, url: url }
            this.#service.receiveInitChat(this.initId.bind(this));
            this._notify(UpdateType.EMPTY, {isError : false, idUser: userId});
        }
        catch (error) {
            this.#messages = [];
            this._notify(UpdateType.EMPTY, {isError : true, error });
        }
    }
    async init({id}) {
        try {
            
         let messages = await this.#service.getChatById(id);
         this.#id = id;
         this.#messages = messages.data.messages;
         this.#info = {name: messages.data.name, url:messages.data.url }
         await this.#service.joinChat(this.#id);
         this.#service.receive(this.add.bind(this));
         this.#service.receiveUpdateMessage(this.handleReceivedUpdateMessage.bind(this));
         this.#service.receiveDeleteMessage(this.handleReceivedDeleteMessage.bind(this));
         this._notify(UpdateType.INIT, {isError : false, idChat: id});
    }            
    catch (error) {
        this.#messages = [];
        this._notify(UpdateType.INIT, {isError : true, error });
        }
    }
    getAll(){
        return this.#messages;
    }
    
    getInfo(){
        return this.#info;
    }
    
    async send(message) {
        await this.#service.send(message, this.#id);
    }
    
    async deleteMsg(id){
        await this.#service.deleteMessage(this.#id, id);
    }
    
    async sendUpdateMessage(id, text){
        await this.#service.updateMessage(text,id, this.#id);
    }

    async destroy(){
        if(!this.#id){
            return;
        }
        await this.#service.leaveChat(this.#id);
        this.#messages = [];
        this.#id = 0;
    }
    
    async createChat(data, users){
       await this.#service.create(data, users);
       this.#service.receive(this.add.bind(this));
        this.#service.receiveUpdateMessage(this.handleReceivedUpdateMessage.bind(this));
        this.#service.receiveDeleteMessage(this.handleReceivedDeleteMessage.bind(this));
    }
    
    add(message) {
        let msg = adaptToClient(JSON.parse(message));
        let type = UpdateType.MAJOR;
        this.#messages.push(msg);
        this._notify(type, {isError : false });
    }

    handleReceivedUpdateMessage(message) {
        let msg = adaptToClient(JSON.parse(message));
        let type = UpdateType.MAJOR;
        const index = this.#messages.findIndex(m => m.id === msg.id);
        if (index !== -1) {
            this.#messages[index] = msg;
        }
        this._notify(type, {isError : false });
    }
    
    handleReceivedDeleteMessage(id) {
        let type = UpdateType.MAJOR;
        this.#messages = this.#messages.filter(msg => String(msg.id) !== JSON.parse(id));
        this._notify(type, {isError : false });
    }
    
    async initId(id){
       await this.init({id})
    }
}