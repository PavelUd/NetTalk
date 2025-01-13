import {decodeJwtToken} from "../utils/common.js";

export default class AuthPresenter{

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
        if (!result.succeeded) {
            this.#authElem.showMessage("danger", "Ошибка Регистрации");
        }
        else {
            let user = decodeJwtToken(result.data)
            localStorage.setItem('user', JSON.stringify(user));
            window.location.href = '/';
        }
    }
    async handleLogin(username, password) {
        try {
            const result = await this.#authModel.login(
                {
                    login: username,
                    password: password
                }
            );
            let user = decodeJwtToken(result.data)
            localStorage.setItem('user', JSON.stringify(user));
            window.location.href = '/'
        }
        catch (e){
            this.#authElem.showMessage("danger", "Ошибка авторизации. Проверьте логин и пароль!!");
        }
    }
}