import MessagePresenter from "./message-presenter.js";
import {remove, render, RenderPosition} from "../framework/render.js";
import ErrorView from "../views/error-view.js";
import LoadingView from "../views/load-view.js";
import {UpdateType} from "../utils/const.js";
import ChatView from "../views/chat-view.js";

export default class ChatPresenter{
    #messagesModel
    #container;
    #user;
    #inputButtonPresenter
    #errorElement = new ErrorView();
    #messagePresenters = new Map()
    #loadingElement = new LoadingView();
    #chatView =new ChatView();
    #isLoading = true;
    #isError = false;

    constructor({container, messagesModel, inputButtonPresenter}) {
        this.#user = JSON.parse(localStorage.getItem('user'));
        this.#messagesModel = messagesModel;
        this.#container = container
        this.#messagesModel.addObserver(this.#modelEventHandler);
        this.#inputButtonPresenter = inputButtonPresenter;
    }

    get messages() {
        return this.#messagesModel.getAll();
    }

    #renderChat = () =>{
        if (this.#isError) {
            this.#renderError();
            return;
        }
        if (this.#isLoading) {
            this.#renderLoader();
            return;
        }
        this.#clearChat();
        this.#renderMessageList();
        this.#inputButtonPresenter.init(
            {
                onButtonClick: this.#handleNewPointClick
            });
        this.#chatView.scrollToBottom();
    };
    #clearChat = () =>{
        this.#clearMessageList();
        this.#inputButtonPresenter.destroy()
    }

    #renderEmptyChat = (idUser)=>{
        this.#clearChat();
        this.#inputButtonPresenter.init(
            {
                onButtonClick: async (data) => { 
                    await this.#messagesModel.createChat(data,[ idUser, parseInt(this.#user.Id) ]) 
                }
            });
    }
    
    async destroy(){
        this.#messagesModel.removeObserver(this.#modelEventHandler);
        await this.#messagesModel.destroy();
        this.#clearChat();
    }
    
   #renderMessageList= () =>{
       this.messages.forEach((message) => this.#renderMessage(message));
   };
    #clearMessageList = () => {
        this.#messagePresenters.forEach((presenter) => presenter.destroy());
        this.#messagePresenters.clear();
    };
    
    #modelEventHandler = (updateType, data) =>{
        switch (updateType) {
            case UpdateType.INIT:
                if (data.error) {
                    this.#isError = true;
                } else {
                    this.#isLoading = false;
                    this.#isError = false;
                    remove(this.#loadingElement);
                }
                this.#renderChat();
                break;
            case UpdateType.MAJOR:{
                this.#clearChat();
                this.#isLoading = false;
                this.#renderChat();
                break;
            }   
            
            case UpdateType.EMPTY:{
                this.#renderEmptyChat(data.idUser);
                break;
                
            }
        }
    }
    
   #renderMessage = (message) =>{
       const messagePresenter = new MessagePresenter({
           container: this.#container
       });
       let type = this.#user.Id == message.user.idUser ? "self" : "other"
       messagePresenter.init(message, type);
       this.#messagePresenters.set(message.id,  messagePresenter);
   }

    #handleNewPointClick = (message) => {
        this.#messagesModel.send(message);
    }
    #renderLoader = () => {
        render(this.#loadingElement, this.#container, RenderPosition.AFTERBEGIN);
    };

    #renderError = () => {
        render(this.#errorElement, this.#container, RenderPosition.AFTERBEGIN);
    };
    
}