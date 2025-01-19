import {formatDate} from "../utils/common.js";
import AbstractView from "../framework/view/abstract-view.js";
function createTemplate(message){
    return `<div class="d-flex flex-row justify-content-start">
                          <img src="${message.user.url}"
                               alt="avatar 1" style="width: 45px; height: 100%;">
                          <div>
                            <p class="small p-2 ms-3 mb-1 rounded-3" style="background: #eef0f1">${message.text}</p>
                            <p class="small ms-3 mb-3 rounded-3 text-muted float-end">${formatDate(message.createdDate)}</p>
                          </div>
                        </div>`
}
export default class OtherMessageView extends AbstractView {

    #message

    constructor({message, type}) {
        super();
        this.#message = message;
    }

    get template() {
        return createTemplate(this.#message);
    }
}