import MessageView from "../views/message-view.js";
import {remove, render} from "../framework/render.js";
import OtherMessageView from "../views/other-message-view.js";

export default class MessagePresenter{
    #container;
    #message;
    #messageElement;
    #onDelete
    #onUpdate
    constructor({container, onDelete, onUpdate}) {
        this.#container = container;
        this.#onDelete = onDelete;
        this.#onUpdate = onUpdate;
    }

    init(message, type) {
        this.#message = message;
        if(type === 'self') {
            this.#messageElement = new MessageView({
                message: this.#message,
                onDelete: this.#onDelete,
                onUpdate: this.#onUpdate
            });
        }
        else{
            this.#messageElement = new OtherMessageView({
                message: this.#message
            });
        }
        render(this.#messageElement, this.#container);
    }
    destroy = () => {
        remove(this.#messageElement);
    };
    

}