const otherUsers = document.querySelectorAll('.other-user') 

const createChat = async (body,headers= new Headers()) => {
       let method = 'POST';

        const response = await fetch(
            `https://localhost:7235/api/chats`,
            {method, body, headers},
        );

        try {
            return response.json();
        } catch (err) {
            throw err;
        }
}

const redirect= async (evt) => {
    evt.preventDefault();
    let elem = evt.target.closest('.other-user');
    let otherUserId = elem.dataset.id;
    const user = JSON.parse(localStorage.getItem('user'));
    let data = {
        name: "1",
        type: "personal",
        idUsers: [ otherUserId, user.Id]
    };
    let result = await createChat(JSON.stringify(data), new Headers({'Content-Type': 'application/json'}));
    if(result.succeeded){
        window.location.href = `/chats/${result.data}`;
    }
    else{
        console.log(result);
    }
}
if(otherUsers != null) {
    otherUsers.forEach(us => us.addEventListener('click', redirect))
}