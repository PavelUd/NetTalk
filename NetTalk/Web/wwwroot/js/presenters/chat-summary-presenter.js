import {remove, render, replace} from "../framework/render.js";
import ChatSummaryView from "../views/chat-summary-view.js";

export default class ChatSummaryPresenter{
    #container;
    #summary;
    #summaryElement;
    #onClickHandler
    #onClickEmptyChatHandler
    isActive;
    constructor({container, onClickHandler,onClickEmptyChatHandler}) {
        this.#onClickHandler = async (idChat) => { 
           await onClickHandler(idChat);
           this.isActive = true;
        }
        this.#container = container;
        this.#onClickEmptyChatHandler = async ({userId, url, name}) => {
            onClickEmptyChatHandler({userId, url, name})
            this.isActive = true;
        }
    }

    
    
    init(message) {
        this.#summary = message;
        this.#summaryElement = new ChatSummaryView({
                summary: this.#summary,
                onClickHandler: this.#onClickHandler,
                onClickEmptyChatHandler: this.#onClickEmptyChatHandler
        });
        render(this.#summaryElement, this.#container);
    }

    
    
    addUserChat = (idChat) =>{
        this.#summary = {
            ...this.#summary,
            id: idChat
        };
        const summaryElement = new ChatSummaryView({
            summary: this.#summary,
            onClickHandler: this.#onClickHandler,
            onClickEmptyChatHandler: this.#onClickEmptyChatHandler,
            isActive: this.isActive
        });
        
        replace(summaryElement, this.#summaryElement);
        console.log(summaryElement)
        this.#summaryElement = summaryElement;
    }
    destroy = () => {
        remove(this.#summaryElement);
    };
}