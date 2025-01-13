import {ApiService} from "./api-service.js";

export default class AuthApiService extends ApiService{
    async login(authData){
        const response = await this._load(
            {
                url: 'login', 
                method: 'POST', 
                body: JSON.stringify(authData),
                headers: new Headers({'Content-Type': 'application/json'})
            });
        return ApiService.parseResponse(response);
    }
    
    async register(authData) {
        const response = await this._load({
            url: 'register', 
            method: 'POST',
            body: JSON.stringify(authData),
            headers: new Headers({'Content-Type': 'application/json'})
        });
        return ApiService.parseResponse(response);
    }
}

