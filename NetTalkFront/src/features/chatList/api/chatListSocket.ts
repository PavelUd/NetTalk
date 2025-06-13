import * as signalR from '@microsoft/signalr'

let connection: signalR.HubConnection
export const createChatConnection = (token: string) => {
	connection = new signalR.HubConnectionBuilder()
		.withUrl('http://localhost:5209/chatlisthub', {
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

export const invokeGetChats = async () => {
	if (!connection) return

	// Ожидаем подключения
	let retries = 0
	while (
		connection.state !== signalR.HubConnectionState.Connected &&
		retries < 20
	) {
		await new Promise(res => setTimeout(res, 100)) // подождём 100мс
		retries++
	}

	if (connection.state !== signalR.HubConnectionState.Connected) {
		console.warn('SignalR connection not ready. Skipping invoke.')
		return
	}

	try {
		await connection.invoke('GetChats')
	} catch (error) {
		console.error('invokeGetChats failed:', error)
	}
}

export const subscribeToChats = (callback: (chats: any) => void) => {
	if (!connection) return

	connection.off('ReceiveChatsList')
	connection.on('ReceiveChatsList', (data: string) => {
		callback(data)
	})
}

export const stopChatConnection = async () => {
	if (connection) {
		await connection.stop()
	}
}
