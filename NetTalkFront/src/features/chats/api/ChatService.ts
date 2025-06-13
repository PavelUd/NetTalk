import { $fetch } from '@/api/api.fetch'
import { IChat } from '@/models/chat'

type ChatResponse = {
	data: IChat
}

export const createDirect = async (idOtherUser: string) => {
	const response = await $fetch.post<ChatResponse>(
		'chats/direct',
		{ idOtherUser: idOtherUser },
		true
	)
	console.log(response)
	return response.data
}

export const getDirectByIdUser = async (idOtherUser: string) => {
	const response = await $fetch.get<ChatResponse>(
		`chats/direct/users/${idOtherUser}`,
		true
	)
	return response.data
}

export const getChatById = async (id: string) => {
	const response = await $fetch.get<ChatResponse>(`chats/${id}`, true)
	console.log(response.data)
	return response.data
}
