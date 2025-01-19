import AbstractView from "../framework/view/abstract-view.js";

function createTemple(){
    return `<div class="col-md-6 col-lg-5 col-xl-4 mb-4 mb-md-0" style="padding :0 2rem 0 1rem; border-radius: 80px;">
            <div class="input-group mb-2 search" > 
              <input type="search" class="form-control js-input" placeholder="Search" style="border-radius: 80px 0 0 80px" aria-label="Search" aria-describedby="search-addon" />
              <span class="input-group-text border-0 js-btn" id="search-addon" style="border-radius: 0 80px 80px 0">
                <i class="fas fa-search"></i>
              </span>
            </div>
          </div>`
}
export default class NavSearchView extends AbstractView{
    #button;
    #input;
    #onSearchClickHandler;
    constructor({onSearchClickHandler}) {
        super();
        this.#onSearchClickHandler = onSearchClickHandler;
        this.#button = this.element.querySelector('.js-btn');
        this.#input = this.element.querySelector('.js-input')
        this.#button.addEventListener('click', this.#onSearchClick);
    }
    get template() {
        return createTemple();
    }

    #onSearchClick = (evt) =>{
        let login = this.#input.value;
        if(login == ''){
            return;
        }
       this.#onSearchClickHandler(login);
    }
}