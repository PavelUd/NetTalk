export function decodeJwtToken(token){
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const payloadinit = atob(base64);
    return JSON.parse(payloadinit);
}

export function formatDate(isoDate) {
    const date = new Date(isoDate);

    const time = date.toLocaleTimeString('ru-RU', {
        hour: '2-digit',
        minute: '2-digit'
    });

    const day = date.getDate();
    const month = date.toLocaleDateString('ru-RU', { month: 'long' });

    return `${time} | ${day} ${month}`;
}