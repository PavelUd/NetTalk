import AbstractView from "../framework/view/abstract-view.js";

const createNewPointButtonTemplate = () => `<div class="text-muted d-flex justify-content-start align-items-center pe-3 pt-3 mt-2"> 
                <input type="text" class="form-control form-control  sendMessageInput"
                       placeholder="Type message"><a class="ms-1 sendMessageButton"><i class="fas fa-paper-plane fa-2x" style="color: #8a2be2"></i>
                </a></div>
`;
export default class MessageInputView extends AbstractView{
    #handleClick = null;
    #elemButton
    #elemInput
    constructor({ onClick }) {
        super();
        this.#handleClick = onClick;
        this.#elemButton = this.element.querySelector('.sendMessageButton');
        this.#elemInput = this.element.querySelector('.sendMessageInput');
        this.#elemButton.addEventListener('click',  this.#clickHandler);
    }
    get template() {
        return createNewPointButtonTemplate();
    }

    setDisabled = (isDisabled) => {
        this.element.disabled = isDisabled;
    };

    #clickHandler = (evt) => {
        evt.preventDefault();
        let message = this.#elemInput.value;
        this.#handleClick(message);
        this.#elemInput.value ="";
    };
    
}