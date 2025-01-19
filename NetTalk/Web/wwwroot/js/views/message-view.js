import {formatDate} from "../utils/common.js";
import AbstractView from "../framework/view/abstract-view.js";
function createTemple(message){
    
    return `<div class="d-flex flex-row justify-content-end">
                    <div>
                        <p class="small p-2 me-3 mb-1 text-white rounded-3 bg-sky">${message.text}</p>
                        <p class="small me-3 mb-3 rounded-3 text-muted">${formatDate(message.createdDate)}</p>
                    </div>
                    <img src="${message.user.url}"
                         alt="avatar 1" style="width: 45px; height: 100%;">
                </div>`
}

export default class MessageView extends AbstractView{
    
    #message
    constructor({message, type}) {
        super();
        this.#message = message;
    }
    get template() {
        return createTemple(this.#message);
    }
    
}