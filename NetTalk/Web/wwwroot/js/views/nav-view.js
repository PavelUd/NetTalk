import AbstractView from "../framework/view/abstract-view.js";

function createTemple(summary){
    return `<div class="col-md-6 col-lg-7 col-xl-8 chat">
                <div class= "navbar-content chat" id="ava">
                 <div class="avatar">
                    <img src="${summary.url}" alt="Chat Avatar">
                </div>
            <div class="title">${summary.name}</div>
        </div>
      </div>`
}
export default class NavChatView extends AbstractView{

    #summary
    constructor({summary}) {
        super();
        this.#summary = summary
    }
    get template() {
        return createTemple(this.#summary);
    }
    
}