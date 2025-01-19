import {remove, render} from "../framework/render.js";
import MessageInputView from "../views/message-input-view.js";

export default class MessageInputPresenter
{

    #container;
    #button;
    #onButtonClick;

    constructor({container}){
        this.#container = container;
    }

    init({onButtonClick}){
        this.#onButtonClick = onButtonClick;
        this.#button = new MessageInputView({onClick: this.#onButtonClick});
        render(this.#button, this.#container);
    }

    enableButton() {
        this.#button.setDisabled(false);
    }

    destroy(){
        if(this.#button != null) {
            remove(this.#button);
        }
    }
    
    disableButton() {
        this.#button.setDisabled(true);
    }
}