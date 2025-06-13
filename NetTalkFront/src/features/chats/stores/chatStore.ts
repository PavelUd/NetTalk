import {
	createChatConnection,
	deleteMessage,
	invateChat,
	receiveDeleteMessage,
	receiveMessage,
	receiveUpdateMessage,
	sendMessage,
	startChatConnection,
	updateMessage,
} from '@/features/chats/api/chatSocket'
import { chatSummaries } from '@/mocks/chatSummary'
import { IChat } from '@/models/chat'
import { IChatSummary } from '@/models/chatSummary'
import { IMessage } from '@/models/message'
import { IUser } from '@/models/user'
import { create } from 'zustand'
import {
	createDirect,
	getChatById,
	getDirectByIdUser,
} from '../api/ChatService'

import { adaptToMessage, adaptToUser } from '@/utils/adapter'
import * as signalR from '@microsoft/signalr'

let connection: signalR.HubConnection

interface ISendMessageResponse {
	Id: string
	IdChat: string
	Text: string
	Sender: IUser
	UpdatedDate: string
	CreatedDate: string
}

interface ChatStore {
	chats: IChatSummary[]
	currentChatId: string | null
	currentChat: IChat | null

	setCurrentChat: (chatId: string, token: string) => void
	setMessages: (message: IMessage) => void
	setCurrentDirectByUserId: (idOtherUser: string, name: string) => Promise<void>
	sendMessage: (idChat: string, message: string) => Promise<void>
	deleteMessage: (idChat: string, idMessage: string) => Promise<void>
	receiveDeleteMessage: () => void
	receiveMessages: (callback: () => void) => void
	receiveUpdateMessage: () => void
	updateMessage: (idChat: string, idMessage: string, message: string) => void
}

export const useChatStore = create<ChatStore>(set => ({
	chats: chatSummaries,
	currentChatId: null,
	currentChat: null,
	setCurrentChat: async (chatId: string, token: string) => {
		connection = await createChatConnection(token)
		await invateChat(token)
		startChatConnection()

		set({
			currentChatId: chatId,
			currentChat: await getChatById(chatId),
		})
	},

	setMessages: (message: IMessage) => {
		set(state => ({
			currentChat: state.currentChat
				? {
						...state.currentChat,
						messages: [...state.currentChat.messages, message],
				  }
				: null,
		}))
	},

	setCurrentDirectByUserId: async (idOtherUser: string, name: string) => {
		try {
			let chat = await getDirectByIdUser(idOtherUser)
			if (!chat) {
				chat = await createDirect(idOtherUser)
			}
			chat.name = name
			set({ currentChat: chat, currentChatId: chat.id })
		} catch (error) {
			console.error('Error fetching direct chat:', error)
		}
	},

	deleteMessage: async (idChat: string, idMessage: string) => {
		await deleteMessage(idChat, idMessage)
	},

	receiveDeleteMessage: () => {
		receiveDeleteMessage((idDeletedMessage: string) => {
			console.log('Deleted message ID:', idDeletedMessage)
			set(state => {
				if (!state.currentChat) return {}

				return {
					currentChat: {
						...state.currentChat,
						messages: state.currentChat.messages.filter(
							message => message.id !== JSON.parse(idDeletedMessage)
						),
					},
				}
			})
		})
	},

	sendMessage: async (idChat: string, message: string) => {
		await sendMessage(idChat, message)
	},
	updateMessage: async (idChat: string, idMessage: string, message: string) => {
		await updateMessage(idChat, idMessage, message)
	},
	receiveUpdateMessage: async () => {
		receiveUpdateMessage(async (json: string) => {
			try {
				const data = JSON.parse(json) as ISendMessageResponse
				console.log(data)
				const parsed = await adaptToMessage(
					data.Id,
					data.Sender.Id,
					data.Text,
					data.CreatedDate,
					data.UpdatedDate,
					await adaptToUser(
						data.Sender.Id,
						data.Sender.Name,
						'',
						data.Sender.Avatar
					)
				)
				console.log(parsed)
				set(state => {
					if (!state.currentChat) return {}

					// заменяем существующее сообщение, сохранив порядок
					const updatedMessages = state.currentChat.messages.map(m =>
						m.id === parsed.id ? parsed : m
					)

					const hasMessage = state.currentChat.messages.some(
						m => m.id === parsed.id
					)

					return {
						currentChat: {
							...state.currentChat,
							messages: hasMessage
								? updatedMessages
								: [...state.currentChat.messages, parsed],
						},
					}
				})
			} catch (e) {
				console.error('Ошибка при парсинге сообщения:', e)
			}
		})
	},
	receiveMessages: (callback: () => void) => {
		if (!connection) return

		receiveMessage(async (json: string) => {
			try {
				const data = JSON.parse(json) as ISendMessageResponse
				const parsed = await adaptToMessage(
					data.Id,
					data.Sender.Id,
					data.Text,
					data.CreatedDate,
					data.UpdatedDate,
					await adaptToUser(
						data.Sender.Id,
						data.Sender.Name,
						'',
						data.Sender.Avatar
					)
				)
				console.log(parsed)
				set(state => ({
					currentChat: state.currentChat
						? {
								...state.currentChat,
								messages: [...state.currentChat.messages, parsed],
						  }
						: null,
				}))
				callback()
			} catch (e) {
				console.error('Ошибка при парсинге сообщения:', e)
			}
		})
	},
}))
