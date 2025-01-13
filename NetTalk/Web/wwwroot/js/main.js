import AuthApiService from "./services/auth-api-service.js";
import AuthModel from "./models/auth-model.js";
import AuthView from "./views/auth-view.js";
import AuthPresenter from "./presenters/auth-presenter.js";


const END_POINT = "https://localhost:7235"

const service = new AuthApiService(END_POINT, "");
const model = new AuthModel(service);
const view = new AuthView();
const presenter = new AuthPresenter(view, model);