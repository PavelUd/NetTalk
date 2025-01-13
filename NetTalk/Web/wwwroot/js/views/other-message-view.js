import {formatDate} from "../utils/common.js";
function createTemplate(message, url){
    return `<div class="d-flex flex-row justify-content-start">
                          <img src="${url}"
                               alt="avatar 1" style="width: 45px; height: 100%;">
                          <div>
                            <p class="small p-2 ms-3 mb-1 rounded-3" style="background: #bebebf">${message.text}</p>
                            <p class="small ms-3 mb-3 rounded-3 text-muted float-end">${formatDate(message.createdDate)}</p>
                          </div>
                        </div>`
}
export default class OtherMessageView {

    #message
    #url

    constructor(message, url) {
        this.#message = message;
        this.#url = url;
    }

    get template() {
        return createTemplate(this.#message, this.#url)
    }
}
