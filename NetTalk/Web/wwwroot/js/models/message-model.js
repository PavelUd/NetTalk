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
            this._notify(UpdateType.EMPTY, {isError : false});
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
         this._notify(UpdateType.INIT, {isError : false});
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

    async destroy(){
        if(!this.#id){
            return;
        }
        await this.#service.leaveChat(this.#id);
        this.#messages = [];
        this.#id = 0;
    }
    
    async createChat(data){
       let chat = await this.#service.create(data); 
       let type = UpdateType.MAJOR;
       this.#messages = [];
       this._notify(type, {isError : false });
    }
    
    add(message) {
        let msg = adaptToClient(JSON.parse(message));
        let type = UpdateType.MAJOR;
        this.#messages.push(msg);
        this._notify(type, {isError : false });
    }
    
}