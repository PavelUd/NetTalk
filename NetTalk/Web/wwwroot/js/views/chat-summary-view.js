import AbstractView from "../framework/view/abstract-view.js";


function createTemple(summary, isActive){

    return `<li class="p-2 border-bottom chatLink ${isActive ? "active" : ""}" data-id = "${summary.id}">
                                 <span id="chatLink" href="/chats/${summary.id}" class="d-flex justify-content-between ">
                                   <div class="d-flex flex-row">
                                     <div>
                                       <img
                                         src="${summary.url}"
                                         alt="avatar" class="d-flex align-self-center me-3" width="60">
                                       <span class="badge bg-warning badge-dot"></span>
                                     </div>
                                     <div class="pt-1" >
                                       <p class="fw-bold mb-0 ">${summary.name}</p>
                                       <p class="small" >Недавно в сообществе</p>
                                     </div>
                                   </div>
                                 </span>
                               </li>`
}
export default class ChatSummaryView extends AbstractView{

    #chatSummary
    #onClickHandler
    #onClickEmptyChatHandler
    #isActive
    constructor({summary, onClickHandler, onClickEmptyChatHandler, isActive = false}) {
        super();
        this.#chatSummary = summary;
        this.#isActive = isActive
        this.#onClickEmptyChatHandler = onClickEmptyChatHandler
        this.#onClickHandler = onClickHandler
        this.element.addEventListener('click', this.#onClick);
    }
    
    get template() {
        return createTemple(this.#chatSummary, this.#isActive);
    }
    
    #onClick = (evt) =>{
        evt.preventDefault();
        const activeElement = evt.currentTarget.parentNode.querySelector('.active');
        if (activeElement) {
            activeElement.classList.remove('active');
        }
        evt.currentTarget.classList.add('active');
        let idChat = evt.currentTarget.dataset.id;
        if(idChat == -1){
            this.#onClickEmptyChatHandler(this.#chatSummary)
        }
        else {
            this.#onClickHandler(idChat);
        }
    }
}