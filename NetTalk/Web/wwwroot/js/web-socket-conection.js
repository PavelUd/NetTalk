const url = window.location.href; // "https://localhost:7235/chats/1"
const chatId = url.match(/\/chats\/(\d+)/)?.[1];
let connection
if(!isNaN(chatId)) {
    connection = new signalR.HubConnectionBuilder()
        .withUrl(`/chathub`)
        .build();

    await connection.start()
        .then(() => connection.invoke("JoinPrivateChat", chatId)
            .catch(err => console.error(err.toString())));
}
const initSelfMessage = (message, url) => {
    console.log(message.Text);
   return `<div class="d-flex flex-row justify-content-end">
        <div>
            <p class="small p-2 me-3 mb-1 text-white rounded-3 bg-primary">${message.Text}</p>
            <p class="small me-3 mb-3 rounded-3 text-muted">${formatDate(message.CreatedDate)}</p>
        </div>
        <img src="${url}"
             alt="avatar 1" style="width: 45px; height: 100%;">
    </div>`
}

const initOtherMessage = (message, url) => {
    
    return `<div class="d-flex flex-row justify-content-start">
                          <img src="${url}"
                               alt="avatar 1" style="width: 45px; height: 100%;">
                          <div>
                            <p class="small p-2 ms-3 mb-1 rounded-3" style="background: #bebebf">${message.Text}</p>
                            <p class="small ms-3 mb-3 rounded-3 text-muted float-end">${formatDate(message.CreatedDate)}</p>
                          </div>
                        </div>`
}

function formatDate(isoDate) {
    const date = new Date(isoDate);
    
    const time = date.toLocaleTimeString('ru-RU', {
        hour: '2-digit',
        minute: '2-digit'
    });
    
    const day = date.getDate(); 
    const month = date.toLocaleDateString('ru-RU', { month: 'long' }); 

    return `${time} | ${day} ${month}`;
}

connection.on("ReceiveMessage", (data, url) => {
    const messagesList = document.getElementById("messagesList");
    const container = document.getElementById("container");
    const message = JSON.parse(data);
    const us = JSON.parse(localStorage.getItem('user'));
    const msgDiv = message.IdUser == us.Id ? initSelfMessage(message, url) : initOtherMessage(message, url);
    messagesList.innerHTML += msgDiv;
    container.scrollTop = container.scrollHeight;
});

document.getElementById("sendMessageButton").addEventListener("click", () => {
    const messageElement = document.getElementById("exampleFormControlInput2");
    const message = messageElement.value;
    const url = messageElement.dataset.url;
    const idChat = messageElement.dataset.id;
    connection.invoke("SendMessage", idChat, message, url)
        .catch(err => console.error(err.toString()));
});


