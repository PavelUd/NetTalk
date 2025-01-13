import {formatDate} from "../utils/common.js";
function createTemple(message, url){
    
    return `<div class="d-flex flex-row justify-content-end">
                    <div>
                        <p class="small p-2 me-3 mb-1 text-white rounded-3 bg-primary">${message.text}</p>
                        <p class="small me-3 mb-3 rounded-3 text-muted">${formatDate(message.createdDate)}</p>
                    </div>
                    <img src="${url}"
                         alt="avatar 1" style="width: 45px; height: 100%;">
                </div>`
}

export default class SelfMessageView {
    
    #message
    #url
    constructor(message, url) {
        this.#message = message;
        this.#url = url;
    }
    get template() {
        return createTemple(this.#message, this.#url);
    }
    
}