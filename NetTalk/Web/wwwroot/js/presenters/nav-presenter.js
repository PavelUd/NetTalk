import {remove, render} from "../framework/render.js";
import {UpdateType} from "../utils/const.js";
import NavChatView from "../views/nav-view.js";
import NavSearchView from "../views/search-nav-view.js";

export default class NavPresenter{
    #container;
    #navChatElement;
    #model
    #chatsModel
    #info
    constructor({container, model, chatsModel}) {
        this.#container = container;
        this.#chatsModel = chatsModel;
        this.#model = model;
        this.#renderSearch();
        this.#model.addObserver(this.#modelEventHandler);
    }

    #getInfo(){
        return this.#model.getInfo();
    }
    
    #modelEventHandler = (updateType, data) => {
        switch (updateType) {
            case UpdateType.INIT:
                this.#info=  this.#getInfo();
                this.#renderChatNav(this.#info)
                break;
            case UpdateType.EMPTY:
                this.#info=  this.#getInfo();
                this.#renderChatNav(this.#info)
                break;
        }
    }
    
    #renderSearch(){
        render(new NavSearchView({onSearchClickHandler: this.#onSearchClickHandler, onInputClearHandler: this.#onInputClearHandler}), this.#container);
    }
    #renderChatNav(summary) {
        this.destroy()
        this.#navChatElement = new NavChatView({
            summary: summary,
        });
        render(this.#navChatElement, this.#container);
    }
    destroy = () => {
        if(this.#navChatElement != null) {
            remove(this.#navChatElement);
        }
    };

    #onSearchClickHandler = (login) => {
        this.#chatsModel.search(login);
    }

    #onInputClearHandler = () =>{
        this.#chatsModel.init();
    }
}