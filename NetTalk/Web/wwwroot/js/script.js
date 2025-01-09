document.addEventListener('DOMContentLoaded', () => {
    const messageForm = document.getElementById('message-form');
    const messageInput = document.getElementById('message-input');
    const messages = document.getElementById('messages');

    messageForm.addEventListener('submit', (event) => {
        event.preventDefault();

        const messageText = messageInput.value.trim();
        if (messageText) {
            addMessage(messageText, 'sent');
            messageInput.value = '';
        }
    });

    function addMessage(text, type) {
        const messageDiv = document.createElement('div');
        messageDiv.classList.add('message', `message-${type}`);

        const contentDiv = document.createElement('div');
        contentDiv.classList.add('content');
        contentDiv.textContent = text;

        messageDiv.appendChild(contentDiv);
        messages.appendChild(messageDiv);

        messages.scrollTop = messages.scrollHeight;
    }
    
    setTimeout(() => {
        addMessage('Привет! Как я могу вам помочь?', 'received');
    }, 1000);
});