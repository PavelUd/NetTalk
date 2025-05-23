import {formatDate} from "../utils/common.js";
import AbstractView from "../framework/view/abstract-view.js";
function createTemple(message){
    
    return `<div class="d-flex flex-row justify-content-end mb-2">
                    <div>
                        <p class="small p-2 mb-1 text-white rounded-3 bg-sky msg-text">${message.text}</p>
                       <div class="d-flex justify-content-between align-items-center ms-2">
                            <p class="small text-muted mb-0 ms-1;">
                                ${formatDate(message.createdDate)}
                            </p>
                            <div class="ms-3 me-1">
                                <i class="bi bi-pencil edit"  style="cursor: pointer;"></i>
                                <i class="bi bi-x delete" style="cursor: pointer;"></i>
                            </div>
                        </div>
                    </div>
                    <img src="${message.user.url}"
                         alt="avatar 1" style="width: 45px; height: 100%;">
                </div>`
}

export default class MessageView extends AbstractView{
    
    #message
    #onDelete;
    #onUpdate;
    #textContainer
    constructor({message, onDelete, onUpdate}) {
        super();
        this.#onDelete = onDelete;
        this.#onUpdate = onUpdate;
        this.#message = message;
        
        this.#textContainer = this.element.querySelector(".msg-text");
        let editBtn = this.element.querySelector('.edit');
        let deleteBtn = this.element.querySelector('.delete');
        editBtn.addEventListener('click',this.#editClickHandler);
        deleteBtn.addEventListener('click',this.#deleteClickHandler);
        
    }
    get template() {
        return createTemple(this.#message);
    }

    #editClickHandler = (evt) => {
        const currentText = this.#textContainer.textContent;
        const input = document.createElement("input");
        input.type = "text";
        input.value = currentText;
        input.className = this.#textContainer.className;
        input.style.border = "none";
        input.style.outline = "none";
        input.setAttribute("data-message-id",  this.#textContainer.getAttribute("data-message-id"));
        input.addEventListener('blur', this.#editBlurHandler)
        this.#textContainer.replaceWith(input);
        input.focus();
    };
    
    #editBlurHandler = (evt) => {
        const updatedP = document.createElement("p");
        updatedP.className = evt.target.className;
        updatedP.textContent = evt.target.value;
        updatedP.classList.add('msg-text');
        this.#textContainer = updatedP;
        this.#onUpdate(this.#message.id, evt.target.value);
        evt.target.replaceWith(updatedP);
    }
    
    #deleteClickHandler = (evt) => {
       this.#onDelete(this.#message.id);
    };
    
    
}