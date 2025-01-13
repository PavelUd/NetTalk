import {ApiService} from "./api-service.js";

export default class ChatAPIService extends ApiService
{
    async getChatById(id){
        const response = await this._load(
            {
                url: `chats-data/${id}`,
                method: 'GET',
                headers: new Headers({'Content-Type': 'application/json'})
            });
        return ApiService.parseResponse(response);
    }
}