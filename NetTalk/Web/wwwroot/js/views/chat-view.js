import AbstractView from "../framework/view/abstract-view.js";

export default class ChatView extends AbstractView{
    #container
    constructor() {
        super();
        this.#container = document.getElementById("container");
    }
    get template() {
        return "";
    }

    scrollToBottom() {
        this.#container.scrollTop = this.#container.scrollHeight;
    }
}