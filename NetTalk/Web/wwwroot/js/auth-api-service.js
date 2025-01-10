class AuthApiService{
    async _load(data, url) {
        const response = await fetch(`https://localhost:7235/${url}`,{
            method: 'POST',
            body: JSON.stringify(data),
            headers: new Headers({
                'Content-Type': 'application/json',
            }),
        });
        
        return response;
    }
}

class AuthModel
{
    #service;

    constructor(service) {
        this.#service = service;
    }

    async login(authData) {
        const response = await this.#service._load(authData, "login");
        return response;
    }

    async register(authData) {
        const response = await this.#service._load(authData, "register");
        return response;
    }
}

class AuthPresenter{

    #authModel;
    #authElem;
    constructor(view, model) {
        this.#authElem = view;
        this.#authModel = model;

        this.#authElem.bindLogin(this.handleLogin.bind(this));
        this.#authElem.bindRegister(this.handleRegister.bind(this));
    }

    async handleRegister(username, password, fullname) {
        const result = await this.#authModel.register(
            {
                login: username,
                password: password,
                fullname: fullname,
            }
        );
        if (!result.ok) {
            this.#authElem.showMessage("danger", "Ошибка Регистрации");
        }
        else {
            window.location.href = '/'
        }
    }
    async handleLogin(username, password) {
        const result = await this.#authModel.login(
            {
                login: username,
                password: password
            }
        );
        if (!result.ok) {
            this.#authElem.showMessage("danger", "Ошибка авторизации. Проверьте логин и пароль!!");
        } 
        else {
            window.location.href = '/'
        }
    }
}

class AuthView {

    constructor() {
        this.usernameInput = document.getElementById("username");
        this.passwordInput = document.getElementById("password");
        this.alertBox = document.getElementById("alertBox");
    }

    bindLogin(handler) {
        const loginButton = document.getElementById("loginButton");
        if(loginButton == null){
            return;
        }
        loginButton.addEventListener("click", (evt) => {
            evt.preventDefault()
            const username = this.usernameInput.value;
            const password = this.passwordInput.value;
            handler(username, password);
        });
    }
    
    bindRegister(handler){
        const registerButton = document.getElementById("registerButton");
        const fullNameInput = document.getElementById("fullName");
        if(registerButton == null){
            return;
        }
        registerButton.addEventListener("click", (evt) => {
            evt.preventDefault();
            const username = this.usernameInput.value;
            const password = this.passwordInput.value;
            const fullName = fullNameInput.value
            handler(username, password, fullName);
        });
    }

    showMessage(type, message) {
        this.alertBox.className = `alert alert-${type}`;
        this.alertBox.innerText = message;
    }
}

const service = new AuthApiService();
const model = new AuthModel(service);
const view = new AuthView();
const presenter = new AuthPresenter(view, model);

