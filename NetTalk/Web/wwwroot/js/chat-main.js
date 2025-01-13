import ChatModel from "./models/chat-model.js";
import ChatAPIService from "./services/chat-api-service.js";
import ChatPresenter from "./presenters/chat-presenter.js";
import SelfMessageView from "./views/self-message-view.js";

const END_POINT = "https://localhost:7235";

const chatService = new ChatAPIService(END_POINT, "");
const chatModel = new ChatModel(chatService)
const chatPresenter = new ChatPresenter(chatModel, new SelfMessageView())