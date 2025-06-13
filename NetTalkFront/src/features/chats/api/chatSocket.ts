import * as signalR from '@microsoft/signalr'

let connection: signalR.HubConnection | null = null
export const createChatConnection = async (token: string) => {
	if (connection) {
		if (
			connection.state === signalR.HubConnectionState.Connected ||
			connection.state === signalR.HubConnectionState.Connecting
		) {
			try {
				await connection.stop()
				console.log('Предыдущее соединение остановлено.')
			} catch (error) {
				console.warn('Ошибка при остановке соединения:', error)
			}
		}
	}

	connection = new signalR.HubConnectionBuilder()
		.withUrl('http://localhost:5209/chathub', {
			accessTokenFactory: () => token,
		})
		.withAutomaticReconnect()
		.configureLogging(signalR.LogLevel.Information)
		.build()

	return connection
}

export const startChatConnection = async () => {
	if (!connection) return
	if (connection.state === signalR.HubConnectionState.Disconnected) {
		await connection.start()
	}
}

export const invateChat = async (idChat: string) => {
	if (
		!connection ||
		connection.state !== signalR.HubConnectionState.Connected
	) {
		console.warn('SignalR connection not ready. Skipping invoke.')
		return
	}

	try {
		await connection.invoke('InvateChat', idChat)
	} catch (error) {
		console.error('Ошибка при вступлении в чат:', error)
	}
}

export const deleteMessage = async (idChat: string, idMessage: string) => {
	if (
		!connection ||
		connection.state !== signalR.HubConnectionState.Connected
	) {
		console.warn('SignalR connection not ready. Skipping invoke.')
		return
	}

	try {
		console.log(idChat, idMessage)
		await connection.invoke('DeleteMessage', idChat, idMessage)
	} catch (error) {
		console.error('Ошибка при отправке сообщения:', error)
	}
}

export const sendMessage = async (idChat: string, message: string) => {
	if (
		!connection ||
		connection.state !== signalR.HubConnectionState.Connected
	) {
		console.warn('SignalR connection not ready. Skipping invoke.')
		return
	}

	try {
		console.log(idChat, message)
		await connection.invoke('SendMessage', idChat, message)
	} catch (error) {
		console.error('Ошибка при отправке сообщения:', error)
	}
}

export const receiveMessage = (callback: (message: any) => void) => {
	if (!connection) return
	connection.off('ReceiveMessage')
	connection.on('ReceiveMessage', (data: string) => {
		console.log(data)
		callback(data)
	})
}

export const receiveDeleteMessage = (callback: (data: string) => void) => {
	if (!connection) return
	connection.off('ReceiveDeleteMessage')
	connection.on('ReceiveDeleteMessage', (data: string) => {
		console.log(data)
		callback(data)
	})
}

export const updateMessage = async (
	idChat: string,
	idMessage: string,
	text: string
) => {
	if (
		!connection ||
		connection.state !== signalR.HubConnectionState.Connected
	) {
		console.warn('SignalR connection not ready. Skipping invoke.')
		return
	}

	try {
		await connection.invoke('UpdateMessage', idChat, text, idMessage)
	} catch (error) {
		console.error('Ошибка при отправке сообщения:', error)
	}
}

export const receiveUpdateMessage = (callback: (data: string) => void) => {
	if (!connection) return
	connection.off('ReceiveUpdateMessage')
	connection.on('ReceiveUpdateMessage', (data: string) => {
		console.log(data)
		callback(data)
	})
}

export const stopChatConnection = async () => {
	if (connection) {
		await connection.stop()
	}
}
