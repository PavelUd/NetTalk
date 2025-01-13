export default class AuthModel
{
    #service;

    constructor(service) {
        this.#service = service;
    }

    async login(authData) {
        try {
            return  await this.#service.login(authData);
        }
        catch {
            throw new Error('Ошибка входа');
        }
    }

    async register(authData) {
        try {
            return await this.#service.register(authData);
        }
        catch {
            throw new Error('Ошибка регистрации');
        }
    }
}