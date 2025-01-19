import Observable from "../framework/observable.js";
import {UpdateType} from "../utils/const.js";
import {adaptUserToChatSummary} from "../utils/adapter.js";

export default class ChatSummaryModel extends Observable
{
    #service
    #chats

    constructor(service) {
        super();
        this.#service = service;
    }
    
    getUserChats(){
        return this.#chats;
    }

    async init() {
        try {
            let data = await this.#service.getUserChats();
            this.#chats = data.data;
            this._notify(UpdateType.INIT, {isError: false});
        }
        catch (error){
            this.#chats = [];
            this._notify(UpdateType.INIT, {isError : true, error });
        }
    }
    
    async search(name){
        try {
            let data =  await this.#service.search(name);
            let res = data.data.map(elem => adaptUserToChatSummary(elem));
            this.#chats = data.data.map(elem => adaptUserToChatSummary(elem));
            this._notify(UpdateType.MAJOR, {isError: false});
        }
        catch (error){
            this.#chats = [];
            this._notify(UpdateType.MAJOR, {isError : true, error });
        }
    }
}