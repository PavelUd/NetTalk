
import ChatAPIService from "./services/chat-api-service.js";
import ChatPresenter from "./presenters/chat-presenter.js";
import MessageModel from "./models/message-model.js";
import MessageInputPresenter from "./presenters/message-input-presenter.js";
import ChatsPresenter from "./presenters/chats-presenter.js";
import ChatSummaryModel from "./models/chat-model.js";
import NavPresenter from "./presenters/nav-presenter.js";


const END_POINT = "https://localhost:7235";
const chatContainer = document.getElementById("messagesList");
const summariesContainer = document.querySelector('.summary-list')
const inputButtonContainer = document.getElementById('input');
const navChatElement = document.querySelector('.navbar')

const chatService = new ChatAPIService(END_POINT, "");

const messageModel = new MessageModel(chatService);
const chatModel = new ChatSummaryModel(chatService)


const inputButtonPresenter = new MessageInputPresenter({container: inputButtonContainer});

await chatService.connect();
let summariesPresenter = new ChatsPresenter({
    container: summariesContainer,
    model: chatModel,
    messageModel: messageModel
})

let chatPresenter = new ChatPresenter(
    {
        container: chatContainer,
        messagesModel: messageModel,
        inputButtonPresenter: inputButtonPresenter
})

let navPresenter = new NavPresenter(
    {
        container : navChatElement,
        chatsModel: chatModel, 
        model : messageModel
    }
)

await chatModel.init()
summariesPresenter.init()

