import MessageView from "../views/message-view.js";
import {remove, render} from "../framework/render.js";
import OtherMessageView from "../views/other-message-view.js";

export default class MessagePresenter{
    #container;
    #message;
    #messageElement;
    constructor({container}) {
        this.#container = container;
    }

    init(message, type) {
        this.#message = message;
        if(type === 'self') {
            this.#messageElement = new MessageView({
                message: this.#message
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