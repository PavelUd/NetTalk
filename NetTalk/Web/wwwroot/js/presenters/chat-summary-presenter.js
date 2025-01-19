
import {remove, render} from "../framework/render.js";
import ChatSummaryView from "../views/chat-summary-view.js";

export default class ChatSummaryPresenter{
    #container;
    #summary;
    #summaryElement;
    #onClickHandler
    #onClickEmptyChatHandler
    constructor({container, onClickHandler,onClickEmptyChatHandler}) {
        this.#onClickHandler = onClickHandler;
        this.#container = container;
        this.#onClickEmptyChatHandler = onClickEmptyChatHandler
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
    destroy = () => {
        remove(this.#summaryElement);
    };
}