import SelfMessageView from "../views/self-message-view.js";
import OtherMessageView from "../views/other-message-view.js";

export default class ChatPresenter{
    #messageView
    #model
    
   constructor(model, messageView) {
        this.#messageView = messageView;
        this.#model = model
    }

    logMessages(){
        
    }
    
    async init(id){
        this.chat = await this.#model.getChat(id);
        console.log(this.chat);
        if (!this.chat.succeeded) {
            console.log("error");
        }
        else{
  //          window.history.pushState(null, null, '/chats/' + this.chat.data.id);
        }
            let messagesContainer = document.getElementById('messagesList');
            messagesContainer.innerHTML = `
        <div class="d-flex justify-content-center">
            <div class="spinner-border" role="status">
                <span class="visually-hidden">Loading...</span>
            </div>
        </div>
    `;
            const us = JSON.parse(localStorage.getItem('user'));
            setTimeout(()=>{
                let messagesContainer = document.getElementById('messagesList');
                let input = document.getElementById('input');
                messagesContainer.innerHTML = '';

                messagesContainer.style.justifyContent = '';
                messagesContainer.style.alignItems = '';
                messagesContainer.style.display = '';
                this.chat.data.messages.forEach(message => {
                    if(message.idUser == us.Id) {
                        let url = us.PhotoUrl;
                        let element = new SelfMessageView(message, url);
                        messagesContainer.innerHTML += element.template
                    }
                    else{
                        let url = this.chat.data.users.find(u => u.id == message.idUser).avtarUrl
                        let element = new OtherMessageView(message, url);
                        messagesContainer.innerHTML += element.template
                        
                    }
                })
                input.hidden = false;
            }, 500);
    }
    
}