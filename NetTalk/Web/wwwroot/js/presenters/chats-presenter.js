import LoadingView from "../views/load-view.js";
import ErrorView from "../views/error-view.js";
import {remove, render, RenderPosition} from "../framework/render.js";
import ChatSummaryPresenter from "./chat-summary-presenter.js";
import {UpdateType} from "../utils/const.js";

export default class ChatsPresenter{
    #container
    #chatModel
    #user;
    messageModel
    #isLoading = false;
    #isError = false;
    #loadingElement = new LoadingView();
    #errorElement = new ErrorView();
    #chatSummaryPresenters = new Map()
    
    constructor({container, model, messageModel}) {
        this.messageModel = messageModel;
        this.#user = JSON.parse(localStorage.getItem('user'));
        this.#container = container;
        this.#chatModel = model;
        this.#chatModel.addObserver(this.#modelEventHandler);
    }
    get summaries() {
        return this.#chatModel.getUserChats(this.#user.Id);
    }
    init(){
        this.#isLoading = false;
        this.#renderChats();
    }
    #renderChats = () =>{
        if (this.#isError) {
            this.#renderError();
            return;
        }
        if (this.#isLoading) {
            this.#renderLoader();
            return;
        }
        this.#clearMessageList()
        this.#renderMessageList()
        
    };
    #renderMessageList= () =>{
        console.log(this.summaries)
        this.summaries.forEach((summary) => this.#renderSummary(summary));
    };
    #clearMessageList = () => {
        this.#chatSummaryPresenters.forEach((presenter) => presenter.destroy());
        this.#chatSummaryPresenters.clear();
    };
    
    #renderSummary(summary){
        const summaryPresenter = new ChatSummaryPresenter({
            container: this.#container,
            onClickHandler: this.onClickHandler.bind(this),
            onClickEmptyChatHandler: this.#onClickEmptyChatHandler
        });
        
        summaryPresenter.init(summary);
        this.#chatSummaryPresenters.set(summary.id,  summaryPresenter)
    }

    #modelEventHandler = (updateType, data) => {
        switch (updateType) {
            case UpdateType.INIT:
                this.#renderChats();
                break;
            case UpdateType.MAJOR:{
                console.log("hello");
                this.#renderChats();
                break;
            }
        }
    }
    
    #renderLoader = () => {
        render(this.#loadingElement, this.#container, RenderPosition.AFTERBEGIN);
    };

    #renderError = () => {
        render(this.#errorElement, this.#container, RenderPosition.AFTERBEGIN);
    };
    
    onClickHandler = async  (idChat) => 
    {
        await this.messageModel.destroy();
        await this.messageModel.init({id: idChat});
    }

    #onClickEmptyChatHandler = async ({userId, url, name}) =>
    {
        await this.messageModel.destroy();
        await this.messageModel.initEmptyChat({userId, url, name});
    }
}